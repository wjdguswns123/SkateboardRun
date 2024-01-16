using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<string, IData> _datas;

    public void Init()
    {
        _datas = new Dictionary<string, IData>();

        _datas.Add("Config", ConfigData.Instance);
    }

    public void LoadData()
    {
        foreach(KeyValuePair<string, IData> data in _datas)
        {
            data.Value.LoadData();
        }
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
