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
    /// ��� UI �����ֱ�.
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
    /// ��ư UI �����ֱ�.
    /// </summary>
    private void ShowSelectButtons()
    {
        _successText.gameObject.SetActive(false);
        _failText.gameObject.SetActive(false);
        _buttonGroup.SetActive(true);
    }

    /// <summary>
    /// ������ ��ư ��ġ ó��.
    /// </summary>
    public void OnExitButtonClick()
    {
        UIManager.Instance.CloseUI(this.gameObject);
        IngameManager.Instance.ExitGame();
    }

    /// <summary>
    /// ����� ��ư ��ġ ó��.
    /// </summary>
    public void OnRestartButtonClick()
    {
        UIManager.Instance.CloseUI(this.gameObject);
        IngameManager.Instance.RestartGame();
    }
}
