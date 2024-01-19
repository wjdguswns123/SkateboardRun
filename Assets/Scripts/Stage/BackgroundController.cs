using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    private SpriteRenderer[] _backgroundImages;

    #endregion

    private float _changeMoveValue;
    private float _prevChangeLength;
    private float _moveValue;

    private void Start()
    {
        _changeMoveValue = _backgroundImages[1].transform.localPosition.x - _backgroundImages[0].transform.localPosition.x;
        _prevChangeLength = 0f;
    }

    /// <summary>
    /// ��� ��ġ �ʱ�ȭ.
    /// </summary>
    /// <param name="startPos"></param>
    public void Init(Vector3 startPos)
    {
        _moveValue = 0f;
        _prevChangeLength = 0f;

        Vector3 bgPos = this.transform.localPosition;
        bgPos.x = startPos.x + 5f;
        this.transform.localPosition = bgPos;
    }

    /// <summary>
    /// ��� ������Ʈ.
    /// </summary>
    /// <param name="moveLength"></param>
    public void UpdateBackground(float moveLength)
    {
        if (_prevChangeLength + _changeMoveValue < moveLength - _moveValue)
        {
            // �÷��̾ ��� ũ�⸸ŭ �̵��ϸ�, ��� ��ġ ����.
            _prevChangeLength = moveLength;
            _moveValue = 0f;

            var pos = this.transform.localPosition;
            pos.x += _changeMoveValue;
            this.transform.localPosition = pos;
        }
        else
        {
            // ��� �̹����� �������� ���������� �̵��� �ʾ� ���̵��� �ϱ� ����, ��� �̹����� ��ġ ������Ʈ. 
            Vector3 bgPos = this.transform.localPosition;
            var farBGMove = 0.5f * Time.deltaTime;
            bgPos.x += farBGMove;
            _moveValue += farBGMove;
            this.transform.localPosition = bgPos;
        }
    }
}
