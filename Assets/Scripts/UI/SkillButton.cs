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
    /// 스킬 버튼에 스킬 설정.
    /// </summary>
    /// <param name="skillIndex"></param>
    public void Set(int skillIndex)
    {
        _currentSkillIndex = skillIndex;
        _tempSkillNameText.text = "스킬" + skillIndex;
    }

    /// <summary>
    /// 현재 설정된 스킬 인덱스 반환.
    /// </summary>
    /// <returns></returns>
    public int GetSkillIndex()
    {
        return _currentSkillIndex;
    }
}
