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
    /// �ε� UI ���.
    /// </summary>
    public void ShowLoadingUI()
    {
        _loadingUI.gameObject.SetActive(true);
    }

    /// <summary>
    /// �ε� UI �����.
    /// </summary>
    public void HideLoadingUI()
    {
        _loadingUI.gameObject.SetActive(false);
    }
}
