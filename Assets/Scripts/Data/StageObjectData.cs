using System;
using System.Collections.Generic;
// Data Convertor 로 생성.
public partial class StageObjectData : Singleton<StageObjectData>, IData
{
    public List<StageObject> StageObject { get; set; }

    public void LoadData()
    {
        StageObject = DataManager.Instance.LoadData<StageObject>("StageObject", StageObject);
    }

    public StageObject GetData(int index)
    {
        return StageObject.Find(d => d.Index == index);
    }
}

