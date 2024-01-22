using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : SingletonMonoBehaviour<PopupManager>
{
    #region Inspector

    [SerializeField]
    private Canvas _canvas;

    [SerializeField]
    private Image _background;

    #endregion

    private Stack<GameObject> _popupStack;
    private Dictionary<string, GameObject> _popupPool;

    private void Awake()
    {
        _popupStack = new Stack<GameObject>();
        _popupPool = new Dictionary<string, GameObject>();

        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// ÆË¾÷ ¿­±â.
    /// </summary>
    /// <param name="popupName"></param>
    public GameObject OpenPopup(string popupName)
    {
        GameObject uiObj;
        if (_popupPool.ContainsKey(popupName))
        {
            _popupPool.TryGetValue(popupName, out uiObj);
            uiObj.SetActive(true);
        }
        else
        {
            string path = "UI/Popup/" + popupName;

            uiObj = Instantiate(Resources.Load(path)) as GameObject;
            RectTransform rt = uiObj.GetComponent<RectTransform>();
            rt.SetParent(_canvas.transform);
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            _popupPool.Add(popupName, uiObj);
        }

        _background.transform.SetParent(uiObj.transform);
        _background.transform.SetAsFirstSibling();
        _background.gameObject.SetActive(true);

        _popupStack.Push(uiObj);

        return uiObj;
    }

    /// <summary>
    /// ÆË¾÷ ´Ý±â.
    /// </summary>
    /// <param name="popup"></param>
    public void ClosePopup(GameObject popup)
    {
        if (_popupStack.Peek() == popup)
        {
            _background.transform.SetParent(_canvas.transform);
            _background.gameObject.SetActive(false);

            _popupStack.Pop();
            popup.SetActive(false);
        }
    }
}
