using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurdleReadyPoint : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    private int _skillObjectIndex;

    #endregion

    private List<int> _skillIndexs;

    /// <summary>
    /// ��ֹ� ��ų ����.
    /// </summary>
    public void Set()
    {
        var stageObjectData = StageObjectData.Instance.GetData(_skillObjectIndex);

        if(_skillIndexs == null)
        {
            _skillIndexs = new List<int>();
        }

        if(stageObjectData.UseSkill1 > 0)
        {
            _skillIndexs.Add(stageObjectData.UseSkill1);
        }
        if(stageObjectData.UseSkill2 > 0)
        {
            _skillIndexs.Add(stageObjectData.UseSkill2);
        }
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
