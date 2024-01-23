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
    /// 결과 UI 보여주기.
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
    /// 버튼 UI 보여주기.
    /// </summary>
    private void ShowSelectButtons()
    {
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
