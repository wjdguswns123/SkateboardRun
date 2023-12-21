using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    /// <summary>
    /// ī�޶� ���� ����.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="range"></param>
    public void Play(float time = 1f, float range = 1f)
    {
        StartCoroutine(PlayShake(time, range));
    }

    /// <summary>
    /// ī�޶� ���� ó��.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    private IEnumerator PlayShake(float time, float range)
    {
        float tick = 0f;
        float randomStart = Random.Range(-10f, 10f);    // �縰 ������ �������� �������� �����ؼ� ��鸮�� ���� �Ź� �ٸ�����.

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
