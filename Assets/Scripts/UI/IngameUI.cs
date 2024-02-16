using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class IngameUI : MonoBehaviour
{
    private const float SKILLBUTTON_DRAG_VALUE = 10f;

    #region Inspector

    [SerializeField]
    private Button _jumpButton;
    [SerializeField]
    private Button _grindButton;

    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private SkillButton[] _skillButtons;

    #endregion

    private Vector2 _jumpBtnPressPosition;

    /// <summary>
    /// UI �ʱ�ȭ.
    /// </summary>
    public void Init()
    {
        _jumpBtnPressPosition = Vector2.zero;
        _scoreText.text = "0";
        DisableAllSkillButton();
        SetGrindMode(false);
    }

    /// <summary>
    /// ��� ��ų ��ư ��Ȱ��ȭ.
    /// </summary>
    private void DisableAllSkillButton()
    {
        for (int i = 0; i < _skillButtons.Length; ++i)
        {
            _skillButtons[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ���� �ؽ�Ʈ ����.
    /// </summary>
    /// <param name="score"></param>
    public void SetScoreUI(int score)
    {
        _scoreText.text = score.ToString("n0");
    }

    /// <summary>
    /// ����� ��ų ��ư Ȱ��ȭ.
    /// </summary>
    /// <param name="skillIndexs"></param>
    public void SetActiveSkillButton(List<int> skillIndexs)
    {
        for (int i = 0; i < skillIndexs.Count; ++i)
        {
            _skillButtons[i].gameObject.SetActive(true);
            _skillButtons[i].Set(skillIndexs[i]);
        }
    }

    /// <summary>
    /// �׶��ε� ��� ����.
    /// </summary>
    /// <param name="enable"></param>
    public void SetGrindMode(bool enable)
    {
        _jumpButton.gameObject.SetActive(!enable);
        _grindButton.gameObject.SetActive(enable);
    }

    /// <summary>
    /// ���� ��ư ��ġ ó��.
    /// </summary>
    public void OnJumpButtonClick()
    {
        if(_jumpBtnPressPosition == Vector2.zero)
        {
            Debug.Log("��ư Ŭ��!");
            IngameManager.Instance.PlayJump(0);
            DisableAllSkillButton();
        }
    }

    public void OnJumpButtonDragBegin(BaseEventData data)
    {
        _jumpBtnPressPosition = Input.mousePosition;
        Debug.Log("start pos : " + _jumpBtnPressPosition);
    }

    /// <summary>
    /// ���� ��ư �巡�� ���� ó��.
    /// </summary>
    /// <param name="data"></param>
    public void OnJumpButtonDragEnd(BaseEventData data)
    {
        //// �巡�� ���� ������ ��ǥ ��ġ�� UI �ִ� �� Ȯ��.
        //var eventData = new PointerEventData(EventSystem.current);
        //eventData.position = Input.mousePosition;
        //Debug.Log("end pos : " + Input.mousePosition);

        //var results = new List<RaycastResult>();
        //EventSystem.current.RaycastAll(eventData, results);

        //if(results.Count > 0)
        //{
        //    var skillBtn = results[0].gameObject.GetComponent<SkillButton>();
        //    if(skillBtn != null)
        //    {
        //        IngameManager.Instance.PlayJump(skillBtn.GetSkillIndex());
        //    }
        //    else
        //    {
        //        IngameManager.Instance.PlayJump(0);
        //    }
        //}
        //else
        //{
        //    IngameManager.Instance.PlayJump(0);
        //}
        //DisableAllSkillButton();

        Debug.Log("end pos : " + Input.mousePosition);
        var mousePos = Input.mousePosition;
        float moveX = _jumpBtnPressPosition.x - mousePos.x;
        float moveY = mousePos.y - _jumpBtnPressPosition.y;

        if (moveY > moveX && moveY > SKILLBUTTON_DRAG_VALUE)
        {
            Debug.Log("skill 1 : " + (mousePos.y - _jumpBtnPressPosition.y));
            IngameManager.Instance.PlayJump(_skillButtons[0].GetSkillIndex());
        }
        else if (moveX > moveY && moveX > SKILLBUTTON_DRAG_VALUE)
        {
            Debug.Log("skill 2 : " + (_jumpBtnPressPosition.x - mousePos.x));
            IngameManager.Instance.PlayJump(_skillButtons[1].GetSkillIndex());
        }
        else
        {
            IngameManager.Instance.PlayJump(0);
        }
        DisableAllSkillButton();

        _jumpBtnPressPosition = Vector2.zero;
    }

    /// <summary>
    /// �׶��ε� ��ư ������ ó��.
    /// </summary>
    /// <param name="data"></param>
    public void OnGrindButtonDown(BaseEventData data)
    {
        IngameManager.Instance.PlayGrind();
    }

    /// <summary>
    /// �׶��ε� ��ư ���� ó��.
    /// </summary>
    /// <param name="data"></param>
    public void OnGrindButtonUp(BaseEventData data)
    {
        IngameManager.Instance.EndGrind();
    }

    /// <summary>
    /// �Ͻ� ���� ��ư ��ġ ó��.
    /// </summary>
    public void OnPauseButtonClick()
    {
        IngameManager.Instance.SetGamePause(true);
        PopupManager.Instance.OpenPopup("GamePausePopup");
    }
}
