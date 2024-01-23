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

    private GameObject[] _coins;

    public BackgroundController BGController { get { return _bgCtrl; } }

    private void Start()
    {
        _stageLength = _endPoint.transform.position.x - _startPoint.transform.position.x;   // �������� ��ü ���� ���.

        _coins = GameObject.FindGameObjectsWithTag(ConstantValues.TAG_COIN);

        for (int i = 0; i < _hurdleReadyPoints.Length; ++i)
        {
            _hurdleReadyPoints[i].Set();
        }
    }

    /// <summary>
    /// �������� �ʱ�ȭ.
    /// </summary>
    /// <param name="playerPos"></param>
    public void Init(Transform playerPos)
    {
        playerPos.position = _startPoint.transform.position;

        for (int j = 0; j < _coins.Length; ++j)
        {
            _coins[j].SetActive(true);
        }
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
