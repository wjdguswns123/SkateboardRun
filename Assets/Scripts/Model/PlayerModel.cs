using System.Collections;
using System.Collections.Generic;

public class PlayerModel : Singleton<PlayerModel>
{
    public List<int> HaveSkillList;

    public void Init()
    {
        HaveSkillList = new List<int>();

        HaveSkillList.Add(1);
    }
}
