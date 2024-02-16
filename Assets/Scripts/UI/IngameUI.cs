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
    /// UI 초기화.
    /// </summary>
    public void Init()
    {
        _jumpBtnPressPosition = Vector2.zero;
        _scoreText.text = "0";
        DisableAllSkillButton();
        SetGrindMode(false);
    }

    /// <summary>
    /// 모든 스킬 버튼 비활성화.
    /// </summary>
    private void DisableAllSkillButton()
    {
        for (int i = 0; i < _skillButtons.Length; ++i)
        {
            _skillButtons[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 점수 텍스트 설정.
    /// </summary>
    /// <param name="score"></param>
    public void SetScoreUI(int score)
    {
        _scoreText.text = score.ToString("n0");
    }

    /// <summary>
    /// 사용할 스킬 버튼 활성화.
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
    /// 그라인드 모드 설정.
    /// </summary>
    /// <param name="enable"></param>
    public void SetGrindMode(bool enable)
    {
        _jumpButton.gameObject.SetActive(!enable);
        _grindButton.gameObject.SetActive(enable);
    }

    /// <summary>
    /// 점프 버튼 터치 처리.
    /// </summary>
    public void OnJumpButtonClick()
    {
        if(_jumpBtnPressPosition == Vector2.zero)
        {
            Debug.Log("버튼 클릭!");
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
    /// 점프 버튼 드래그 종료 처리.
    /// </summary>
    /// <param name="data"></param>
    public void OnJumpButtonDragEnd(BaseEventData data)
    {
        //// 드래그 종료 시점의 좌표 위치에 UI 있는 지 확인.
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
    /// 그라인드 버튼 누르기 처리.
    /// </summary>
    /// <param name="data"></param>
    public void OnGrindButtonDown(BaseEventData data)
    {
        IngameManager.Instance.PlayGrind();
    }

    /// <summary>
    /// 그라인드 버튼 떼기 처리.
    /// </summary>
    /// <param name="data"></param>
    public void OnGrindButtonUp(BaseEventData data)
    {
        IngameManager.Instance.EndGrind();
    }

    /// <summary>
    /// 일시 정지 버튼 터치 처리.
    /// </summary>
    public void OnPauseButtonClick()
    {
        IngameManager.Instance.SetGamePause(true);
        PopupManager.Instance.OpenPopup("GamePausePopup");
    }
}
