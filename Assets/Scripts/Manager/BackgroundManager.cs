using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    private SpriteRenderer[] _farBackgroundImages;

    #endregion

    private float _farBackgroundImamgeSizeX;
    private float _prevChangeValue;

    private void Start()
    {
        _farBackgroundImamgeSizeX = _farBackgroundImages[0].transform.localScale.x;
        _prevChangeValue = 0f;
    }

    public void UpdateBackground(float moveLength)
    {
        if(_prevChangeValue + _farBackgroundImamgeSizeX < moveLength)
        {
            _prevChangeValue = moveLength;

            var pos = _farBackgroundImages[0].transform.localPosition;
            pos.x += _farBackgroundImamgeSizeX * 2f;
            _farBackgroundImages[0].transform.localPosition = pos;

            var temp = _farBackgroundImages[0];
            _farBackgroundImages[0] = _farBackgroundImages[1];
            _farBackgroundImages[1] = temp;
        }
    }
}
