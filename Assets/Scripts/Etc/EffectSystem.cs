using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    private float _duration;

    #endregion
    private void OnEnable()
    {
        StartCoroutine(DelayFinish());
    }

    /// <summary>
    /// ����Ʈ ��� �ð� �� ��Ȱ��ȭ ó��.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayFinish()
    {
        yield return YieldCache.WaitForSeconds(_duration);

        this.gameObject.SetActive(false);
    }
}
