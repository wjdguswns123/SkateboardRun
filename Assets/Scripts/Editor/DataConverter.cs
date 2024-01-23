using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using Xamasoft.JsonClassGenerator;

public class DataConverter : EditorWindow
{
    private const string EXCEL_PATH = "/../Data/";              // 테이블 엑셀 파일 경로.
    private const string JSON_PATH = "/Resources/Datas/";       // josn 데이터 파일 저장 경로.
    private const string CS_FILE_NAME = "{0}.cs";               // cs 파일 이름 형식.
    private const string CS_DATA_FILE_NAME = "{0}Data.cs";      // 데이터 관리 cs 파일 이름 형식.

    private static string[] _excelFiles;
    private static bool[] _excelFilesSelect;

    private static string[] _jsonFiles;
    private static bool[] __jsonFilesSelect;

    [MenuItem("Tools/Data Converter")]
    public static void ShowWindow()
    {
        string excelPath = Application.dataPath + EXCEL_PATH;
        _excelFiles = Directory.GetFiles(excelPath);
        _excelFilesSelect = new bool[_excelFiles.Length];

        string jsonPath = Application.dataPath + JSON_PATH;
        _jsonFiles = Directory.GetFiles(jsonPath).Where(s => !s.Contains(".meta")).ToArray();
        __jsonFilesSelect = new bool[_jsonFiles.Length];

        DataConverter window = (DataConverter)EditorWindow.GetWindow(typeof(DataConverter));
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical();
            {
                for (int i = 0; i < _excelFiles.Length; ++i)
                {
                    FileInfo fi = new FileInfo(_excelFiles[i]);
                    string className = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
                    _excelFilesSelect[i] = GUILayout.Toggle(_excelFilesSelect[i], className);
                }

                if (GUILayout.Button("Data Convert"))
                {
                    ExcelToJson();
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            {
                for (int j = 0; j < _jsonFiles.Length; ++j)
                {
                    FileInfo fi = new FileInfo(_jsonFiles[j]);
                    string fileName = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
                    __jsonFilesSelect[j] = GUILayout.Toggle(__jsonFilesSelect[j], fileName);
                }

                if (GUILayout.Button("Create cs File"))
                {
                    JsonToCs();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// Excel -> Json 변환.
    /// </summary>
    private void ExcelToJson()
    {
        for (int i = 0; i < _excelFiles.Length; ++i)
        {
            if(_excelFilesSelect[i])
            {
                FileInfo fi = new FileInfo(_excelFiles[i]);
                string className = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);

                string v = string.Empty;

                Excel excel = ExcelHelper.LoadExcel(_excelFiles[i]);

                StringWriter sw = new StringWriter();
                JsonWriter writer = new JsonTextWriter(sw);
                writer.WriteStartObject();

                foreach (ExcelTable table in excel.Tables)
                {
                    if (table.TableName.StartsWith("#"))
                    {
                        continue;
                    }

                    writer.WritePropertyName(table.TableName);
                    writer.WriteStartArray();

                    for (int j = 3; j <= table.NumberOfRows; ++j)
                    {
                        writer.WriteStartObject();

                        for (int k = 1; k <= table.NumberOfColumns; ++k)
                        {
                            string propName = table.GetValue(1, k).ToString();
                            string propType = table.GetValue(2, k).ToString();

                            if (propName.StartsWith("#"))
                            {
                                continue;
                            }
                            if (string.IsNullOrEmpty(propName) || string.IsNullOrEmpty(propType))
                            {
                                continue;
                            }

                            writer.WritePropertyName(propName);
                            v = table.GetValue(j, k).ToString();

                            switch (propType)
                            {
                                case "int":
                                case "bool":
                                    {
                                        int value = v.Length > 0 ? int.Parse(v) : 0;
                                        writer.WriteValue(value);
                                    }
                                    break;
                                case "float":
                                    {
                                        float value = v.Length > 0 ? float.Parse(v) : 0;
                                        writer.WriteValue(value);
                                    }
                                    break;
                                default:
                                    {
                                        writer.WriteValue(v);
                                    }
                                    break;
                            }

                            Debug.Log(propName + " - " + propType);
                        }

                        writer.WriteEndObject();
                    }

                    writer.WriteEndArray();
                }

                writer.WriteEndObject();

                string outputDir = Application.dataPath + JSON_PATH;
                string outputPath = Path.Combine(outputDir, className + ".json");
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                string originContent = string.Empty;
                if (File.Exists(outputPath))
                {
                    byte[] bytes = File.ReadAllBytes(outputPath);
                    UTF8Encoding encoding = new UTF8Encoding();
                    originContent = encoding.GetString(bytes);
                }

                string content = sw.ToString();
                if (originContent != content)
                {
                    File.WriteAllText(outputPath, content);
                }
            }
        }
    }

    /// <summary>
    /// Json -> cs 파일 변환.
    /// </summary>
    private void JsonToCs()
    {
        string error = string.Empty;

        JsonClassGenerator generator = new JsonClassGenerator();

        string csPath = Application.dataPath + "/Scripts/Data/";
        if (!Directory.Exists(csPath))
        {
            Directory.CreateDirectory(csPath);
        }

        for (int i = 0; i < _jsonFiles.Length; ++i)
        {
            if(__jsonFilesSelect[i])
            {
                FileInfo fi = new FileInfo(_jsonFiles[i]);
                string className = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);

                string json = File.ReadAllText(_jsonFiles[i]);

                StringBuilder sb = generator.GenerateClasses(json, out error);

                if (sb != null && !string.IsNullOrEmpty(sb.ToString()))
                {
                    string filePath = Path.Combine(csPath, string.Format(CS_FILE_NAME, className));
                    string originContent = string.Empty;
                    if (File.Exists(filePath))
                    {
                        byte[] bytes = File.ReadAllBytes(filePath);
                        UTF8Encoding encoding = new UTF8Encoding();
                        originContent = encoding.GetString(bytes);
                    }

                    string content = sb.ToString();
                    if (originContent != content)
                    {
                        File.WriteAllText(filePath, content);
                    }

                    string dataFilePath = Path.Combine(csPath, string.Format(CS_DATA_FILE_NAME, className));
                    StringBuilder dataSb = generator.GenerateDataClass(json, className);

                    File.WriteAllText(dataFilePath, dataSb.ToString());
                }
            }
        }
    }
}
