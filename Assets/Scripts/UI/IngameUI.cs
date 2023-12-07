using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class IngameUI : MonoBehaviour
{
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

    /// <summary>
    /// UI �ʱ�ȭ.
    /// </summary>
    public void Init()
    {
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
        IngameManager.Instance.PlayJump(0);
        DisableAllSkillButton();
    }

    /// <summary>
    /// ���� ��ư �巡�� ���� ó��.
    /// </summary>
    /// <param name="data"></param>
    public void OnJumpButtonDragEnd(BaseEventData data)
    {
        // �巡�� ���� ������ ��ǥ ��ġ�� UI �ִ� �� Ȯ��.
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        if(results.Count > 0)
        {
            var skillBtn = results[0].gameObject.GetComponent<SkillButton>();
            if(skillBtn != null)
            {
                IngameManager.Instance.PlayJump(skillBtn.GetSkillIndex());
            }
            else
            {
                IngameManager.Instance.PlayJump(0);
            }
        }
        else
        {
            IngameManager.Instance.PlayJump(0);
        }
        DisableAllSkillButton();
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
}
