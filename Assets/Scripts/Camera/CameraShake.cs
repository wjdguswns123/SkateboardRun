using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    /// <summary>
    /// 카메라 흔들기 실행.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="range"></param>
    public void Play(float time = 1f, float range = 1f)
    {
        StartCoroutine(PlayShake(time, range));
    }

    /// <summary>
    /// 카메라 흔들기 처리.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    private IEnumerator PlayShake(float time, float range)
    {
        float tick = 0f;
        float randomStart = Random.Range(-10f, 10f);    // 펠린 노이즈 시작지점 랜덤으로 지정해서 흔들리는 유형 매번 다르도록.

        while(tick < time)
        {
            float frequency = randomStart + tick * 20f;
            var pos = new Vector3(Mathf.PerlinNoise(frequency, 0f), Mathf.PerlinNoise(0f, frequency), 0f) * range;
            this.transform.localPosition = pos;

            tick += Time.deltaTime;

            yield return null;
        }

        this.transform.localPosition = Vector3.zero;
    }
}
