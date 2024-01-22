using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    #region Inspector

    [SerializeField]
    private Canvas _uiCanvas;

    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// UI ·Îµù.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject LoadUI(string name)
    {
        string path = "UI/" + name;
        var ui = Instantiate(Resources.Load(path)) as GameObject;
        
        RectTransform rt = ui.GetComponent<RectTransform>();
        rt.SetParent(_uiCanvas.transform);
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        return ui;
    }

    /// <summary>
    /// UI ´Ý±â.
    /// </summary>
    /// <param name="ui"></param>
    public void CloseUI(GameObject ui)
    {
        Destroy(ui);
    }
}
