using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    private Transform _moveParent;
    [SerializeField]
    private Transform[] _backgroundObjs;

    #endregion

    private float _moveSpeed = 0.05f;

    private bool _isMoveBackground;

    private float _tempValue = 0f;
    private int _backIndex = 0;

    private void Start()
    {
        _isMoveBackground = true;
    }

    private void Update()
    {
        if(_isMoveBackground)
        {
            _moveParent.Translate(Vector3.left * _moveSpeed);

            _tempValue += _moveSpeed;
            if (_tempValue >= 20f)
            {
                var pos = _backgroundObjs[_backIndex].transform.localPosition;
                pos.x += 40f;
                _backgroundObjs[_backIndex].transform.localPosition = pos;

                _tempValue = 0f;
                _backIndex = _backIndex == 0 ? 1 : 0;
            }
        }
    }

    /// <summary>
    /// 배경 이동 정지.
    /// </summary>
    public void StopBackground()
    {
        _isMoveBackground = false;
    }
}
