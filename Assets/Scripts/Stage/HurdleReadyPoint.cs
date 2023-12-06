using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurdleReadyPoint : MonoBehaviour
{
    public List<int> tempSkillIndexs;

    private List<int> _skillIndexs;

    /// <summary>
    /// 장애물 스킬 설정.
    /// </summary>
    public void Set()
    {
        _skillIndexs = tempSkillIndexs;
    }

    /// <summary>
    /// 플레이어가 도달했을 때 스킬 버튼 활성화.
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
