using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    private GameObject _startPoint;
    [SerializeField]
    private GameObject _endPoint;
    [SerializeField]
    private BackgroundController _bgCtrl;
    [SerializeField]
    private HurdleReadyPoint[] _hurdleReadyPoints;

    #endregion

    private float _stageLength;

    public BackgroundController BGController { get { return _bgCtrl; } }

    private void Start()
    {
        _stageLength = _endPoint.transform.position.x - _startPoint.transform.position.x;   // �������� ��ü ���� ���.

        for(int i = 0; i < _hurdleReadyPoints.Length; ++i)
        {
            _hurdleReadyPoints[i].Set();
        }
    }

    /// <summary>
    /// �÷��̾� �������� ��ġ.
    /// </summary>
    /// <param name="playerPos"></param>
    public void SetStartPoint(Transform playerPos)
    {
        playerPos.position = _startPoint.transform.position;
    }

    /// <summary>
    /// ���� ��ġ�� ���� ���.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public float GetCurrentRate(float pos)
    {
        return (pos - _startPoint.transform.position.x) / _stageLength;
    }

    /// <summary>
    /// ���� �̵��� �Ÿ� ��ȯ.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public float GetMoveLength(float pos)
    {
        return pos - _startPoint.transform.position.x;
    }
}
