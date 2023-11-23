using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUI : MonoBehaviour
{
    /// <summary>
    /// 알리 버튼 터치 처리.
    /// </summary>
    public void OnOllieButtonClick()
    {
        IngameManager.Instance.PlayOllie();
    }
}
