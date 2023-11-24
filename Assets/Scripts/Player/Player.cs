using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    private float _jumpPower;    // �˸� ���� �Ŀ� ��ġ. ���� ���� ���� ��ȭ �� �� ��������...

    private void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();

        _jumpPower = 350;
    }

    /// <summary>
    /// �÷��̾� ���� ����.
    /// </summary>
    public void Jump()
    {
        _rigidBody.AddForce(new Vector2(0f, _jumpPower));
    }

    /// <summary>
    /// �浹 ó��.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ��ֹ��� �浹�ϸ� ���� ����.
        if(collision.CompareTag("Hurdle"))
        {
            IngameManager.Instance.EndGame();
        }
    }
}
