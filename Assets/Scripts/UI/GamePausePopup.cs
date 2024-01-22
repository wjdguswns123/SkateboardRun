using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePausePopup : MonoBehaviour
{
    /// <summary>
    /// 나가기 버튼 터치 처리.
    /// </summary>
    public void OnExitButtonClick()
    {
        PopupManager.Instance.ClosePopup(this.gameObject);
        IngameManager.Instance.ExitGame();
    }

    /// <summary>
    /// 재시작 버튼 터치 처리.
    /// </summary>
    public void OnRestartButtonClick()
    {
        PopupManager.Instance.ClosePopup(this.gameObject);
        IngameManager.Instance.RestartGame();
    }

    /// <summary>
    /// 계속하기 버튼 터치 처리.
    /// </summary>
    public void OnResumeButtonClick()
    {
        PopupManager.Instance.ClosePopup(this.gameObject);
        IngameManager.Instance.SetGamePause(false);
    }
}
