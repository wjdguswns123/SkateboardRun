using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePausePopup : MonoBehaviour
{
    /// <summary>
    /// ������ ��ư ��ġ ó��.
    /// </summary>
    public void OnExitButtonClick()
    {
        PopupManager.Instance.ClosePopup(this.gameObject);
        IngameManager.Instance.ExitGame();
    }

    /// <summary>
    /// ����� ��ư ��ġ ó��.
    /// </summary>
    public void OnRestartButtonClick()
    {
        PopupManager.Instance.ClosePopup(this.gameObject);
        IngameManager.Instance.RestartGame();
    }

    /// <summary>
    /// ����ϱ� ��ư ��ġ ó��.
    /// </summary>
    public void OnResumeButtonClick()
    {
        PopupManager.Instance.ClosePopup(this.gameObject);
        IngameManager.Instance.SetGamePause(false);
    }
}
