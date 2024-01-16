using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    private GameObject _startPoint;
    [SerializeField]
    private GameObject _endPoint;
    [SerializeField]
    private HurdleReadyPoint[] _hurdleReadyPoints;

    #endregion

    private float _stageLength;

    private void Start()
    {
        _stageLength = _endPoint.transform.position.x - _startPoint.transform.position.x;   // 스테이지 전체 길이 계산.

        for(int i = 0; i < _hurdleReadyPoints.Length; ++i)
        {
            _hurdleReadyPoints[i].Set();
        }
    }

    /// <summary>
    /// 플레이어 시작점에 위치.
    /// </summary>
    /// <param name="playerPos"></param>
    public void SetStartPoint(Transform playerPos)
    {
        playerPos.position = _startPoint.transform.position;
    }

    /// <summary>
    /// 현재 위치의 비율 계산.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public float GetCurrentRate(float pos)
    {
        return (pos - _startPoint.transform.position.x) / _stageLength;
    }

    public float GetMoveLength(float pos)
    {
        return pos - _startPoint.transform.position.x;
    }
}
