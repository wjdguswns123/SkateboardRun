using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    private float _popPower;    // 알리 높이 파워 수치. 레벨 업을 통해 강화 할 수 있을지도...

    private void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();

        _popPower = 300f;
    }

    /// <summary>
    /// 플레이어 알리 점프 실행.
    /// </summary>
    public void Ollie()
    {
        _rigidBody.AddForce(new Vector2(0f, _popPower));
    }
}
