using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameResultUI : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    private TMP_Text _successText;
    [SerializeField]
    private TMP_Text _failText;

    [SerializeField]
    private GameObject _buttonGroup;

    #endregion

    /// <summary>
    /// 결과 UI 보여주기.
    /// </summary>
    /// <param name="isSuccess"></param>
    public void ShowResult(bool isSuccess)
    {
        _successText.gameObject.SetActive(isSuccess);
        _failText.gameObject.SetActive(!isSuccess);
        _buttonGroup.SetActive(false);

        Invoke("ShowSelectButtons", 3f);
    }

    /// <summary>
    /// 버튼 UI 보여주기.
    /// </summary>
    private void ShowSelectButtons()
    {
        _successText.gameObject.SetActive(false);
        _failText.gameObject.SetActive(false);
        _buttonGroup.SetActive(true);
    }

    /// <summary>
    /// 나가기 버튼 터치 처리.
    /// </summary>
    public void OnExitButtonClick()
    {
        UIManager.Instance.CloseUI(this.gameObject);
        IngameManager.Instance.ExitGame();
    }

    /// <summary>
    /// 재시작 버튼 터치 처리.
    /// </summary>
    public void OnRestartButtonClick()
    {
        UIManager.Instance.CloseUI(this.gameObject);
        IngameManager.Instance.RestartGame();
    }
}
