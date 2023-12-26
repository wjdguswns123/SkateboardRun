using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemUIManager : SingletonMonoBehaviour<SystemUIManager>
{
    #region Inspector

    [SerializeField]
    private GameObject _loadingUI;

    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// 로딩 UI 출력.
    /// </summary>
    public void ShowLoadingUI()
    {
        _loadingUI.gameObject.SetActive(true);
    }

    /// <summary>
    /// 로딩 UI 숨기기.
    /// </summary>
    public void HideLoadingUI()
    {
        _loadingUI.gameObject.SetActive(false);
    }
}
