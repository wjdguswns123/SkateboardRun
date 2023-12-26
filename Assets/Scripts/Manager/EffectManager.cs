using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingletonMonoBehaviour<EffectManager>
{
    private const int EFFECT_PRELOAD_COUNT = 3;

    private Dictionary<string, List<EffectSystem>> _effectPool;

    /// <summary>
    /// 이펙트 프리로딩.
    /// </summary>
    /// <returns></returns>
    public IEnumerator PreloadEffect()
    {
        if (_effectPool == null)
        {
            _effectPool = new Dictionary<string, List<EffectSystem>>();
        }
        _effectPool.Clear();

        var effectObjs = Resources.LoadAll("Effects");
        for (int i = 0; i < effectObjs.Length; ++i)
        {
            var effectList = new List<EffectSystem>();
            _effectPool.Add(effectObjs[i].name, effectList);
            for (int  j = 0; j < EFFECT_PRELOAD_COUNT; ++j)
            {
                GameObject effect = Instantiate(effectObjs[i]) as GameObject;
                effect.SetActive(false);
                effectList.Add(effect.GetComponent<EffectSystem>());

                yield return null;
            }
        }
    }

    /// <summary>
    /// 이펙트 불러오기.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public EffectSystem LoadEffect(string name, Transform parent = null, Vector3 pos = default(Vector3))
    {
        if(_effectPool.ContainsKey(name))
        {
            var effectList = _effectPool[name];
            for(int i = 0; i < effectList.Count; ++i)
            {
                var effect = effectList[i];
                if (!effect.gameObject.activeSelf)
                {
                    effect.transform.SetParent(parent);
                    effect.transform.localPosition = pos;
                    effect.transform.localScale = Vector3.one;

                    return effect;
                }
            }
        }

        return null;
    }
}
