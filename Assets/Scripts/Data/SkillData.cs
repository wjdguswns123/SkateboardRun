using System;
using System.Collections.Generic;
// Data Convertor 로 생성.
public partial class SkillData : Singleton<SkillData>, IData
{
    public List<Skill> Skill { get; set; }

    public void LoadData()
    {
        Skill = DataManager.Instance.LoadData<Skill>("Skill", Skill);
    }

    public Skill GetData(int index)
    {
        return Skill.Find(d => d.Index == index);
    }
}

