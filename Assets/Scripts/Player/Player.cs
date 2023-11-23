using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    private float _popPower;    // �˸� ���� �Ŀ� ��ġ. ���� ���� ���� ��ȭ �� �� ��������...

    private void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();

        _popPower = 300f;
    }

    /// <summary>
    /// �÷��̾� �˸� ���� ����.
    /// </summary>
    public void Ollie()
    {
        _rigidBody.AddForce(new Vector2(0f, _popPower));
    }
}
