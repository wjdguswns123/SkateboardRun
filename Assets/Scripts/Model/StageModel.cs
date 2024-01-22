using System.Collections;
using System.Collections.Generic;

public class StageModel : Singleton<StageModel>
{
    public int CurrentStageIndex { get; set; }

    public void Init()
    {
        CurrentStageIndex = 99;
    }
}
