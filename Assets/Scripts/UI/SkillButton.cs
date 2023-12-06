using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillButton : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    private TMP_Text _tempSkillNameText;

    #endregion

    private int _currentSkillIndex;

    /// <summary>
    /// ��ų ��ư�� ��ų ����.
    /// </summary>
    /// <param name="skillIndex"></param>
    public void Set(int skillIndex)
    {
        _currentSkillIndex = skillIndex;
        _tempSkillNameText.text = "��ų" + skillIndex;
    }

    /// <summary>
    /// ���� ������ ��ų �ε��� ��ȯ.
    /// </summary>
    /// <returns></returns>
    public int GetSkillIndex()
    {
        return _currentSkillIndex;
    }
}
