using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUI : MonoBehaviour
{
    /// <summary>
    /// �˸� ��ư ��ġ ó��.
    /// </summary>
    public void OnOllieButtonClick()
    {
        IngameManager.Instance.PlayOllie();
    }
}
