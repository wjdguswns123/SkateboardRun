using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    private float _jumpPower;    // 알리 높이 파워 수치. 레벨 업을 통해 강화 할 수 있을지도...

    private void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();

        _jumpPower = 350;
    }

    /// <summary>
    /// 플레이어 점프 실행.
    /// </summary>
    public void Jump()
    {
        _rigidBody.AddForce(new Vector2(0f, _jumpPower));
    }

    /// <summary>
    /// 충돌 처리.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 장애물에 충돌하면 게임 종료.
        if(collision.CompareTag("Hurdle"))
        {
            IngameManager.Instance.EndGame();
        }
    }
}
