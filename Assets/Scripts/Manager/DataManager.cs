using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DataManager : Singleton<DataManager>
{
    public void LoadData()
    {
        ConfigData.Instance.LoadData();
    }

    public List<T> LoadData<T>(string dataName, List<T> datas)
    {
        if (datas != null && datas.Count > 0)
        {
            return datas;
        }

        datas = new List<T>();

        StringBuilder filePath = new StringBuilder();
        filePath.Append("Datas/");
        filePath.Append(dataName);
        TextAsset asset = Resources.Load<TextAsset>(filePath.ToString());
        string jsonText = asset.text;

        JObject obj = JObject.Parse(jsonText);
        var array = (JArray)obj[dataName];

        for (int i = 0; i < array.Count; ++i)
        {
            string dataText = array[i].ToString();
            dataText = dataText.Replace("Enums.", "");
            var data = JsonConvert.DeserializeObject<T>(dataText);
            datas.Add(data);
            //Debug.Log(dataText);
        }

        return datas;
    }
}
