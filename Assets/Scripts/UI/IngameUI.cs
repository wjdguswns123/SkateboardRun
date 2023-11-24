using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUI : MonoBehaviour
{
    /// <summary>
    /// 점프 버튼 터치 처리.
    /// </summary>
    public void OnJumpButtonClick()
    {
        IngameManager.Instance.PlayJump();
    }
}
