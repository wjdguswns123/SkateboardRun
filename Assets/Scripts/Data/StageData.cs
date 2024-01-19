using System;
using System.Collections.Generic;
// Data Convertor 로 생성.
public partial class StageData : Singleton<StageData>, IData
{
    public List<Stage> Stage { get; set; }

    public void LoadData()
    {
        Stage = DataManager.Instance.LoadData<Stage>("Stage", Stage);
    }

    public Stage GetData(int index)
    {
        return Stage.Find(d => d.Index == index);
    }
}

