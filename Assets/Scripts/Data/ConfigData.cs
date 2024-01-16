using System;
using System.Collections.Generic;
// Data Convertor 로 생성.
public partial class ConfigData : Singleton<ConfigData>, IData
{
    public List<Config> Config { get; set; }

    public void LoadData()
    {
        Config = DataManager.Instance.LoadData<Config>("Config", Config);
    }

    public Config GetData(string key)
    {
        return Config.Find(d => d.Name == key);
    }
}

