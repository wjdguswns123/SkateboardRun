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
    /// 이펙트 출력 시간 뒤 비활성화 처리.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayFinish()
    {
        yield return YieldCache.WaitForSeconds(_duration);

        this.gameObject.SetActive(false);
    }
}
