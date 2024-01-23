using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResultUI : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    private GameObject _successPart;
    [SerializeField]
    private GameObject _failPart;

    [SerializeField]
    private Image[] _starImages;

    [SerializeField]
    private GameObject _buttonGroup;

    #endregion

    /// <summary>
    /// ��� UI �����ֱ�.
    /// </summary>
    /// <param name="isSuccess"></param>
    /// <param name="star"></param>
    public void ShowResult(bool isSuccess, int star = 0)
    {
        _successPart.SetActive(isSuccess);
        _failPart.SetActive(!isSuccess);
        _buttonGroup.SetActive(false);

        if(isSuccess)
        {
            for(int i = 0; i < _starImages.Length; ++i)
            {
                _starImages[i].gameObject.SetActive(i < star);
            }
        }

        Invoke("ShowSelectButtons", 3f);
    }

    /// <summary>
    /// ��ư UI �����ֱ�.
    /// </summary>
    private void ShowSelectButtons()
    {
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
