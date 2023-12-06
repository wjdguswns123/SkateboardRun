using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurdleReadyPoint : MonoBehaviour
{
    public List<int> tempSkillIndexs;

    private List<int> _skillIndexs;

    /// <summary>
    /// ��ֹ� ��ų ����.
    /// </summary>
    public void Set()
    {
        _skillIndexs = tempSkillIndexs;
    }

    /// <summary>
    /// �÷��̾ �������� �� ��ų ��ư Ȱ��ȭ.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(ConstantValues.TAG_PLAYER))
        {
            IngameManager.Instance.SetReadySkill(_skillIndexs);
        }
    }
}
