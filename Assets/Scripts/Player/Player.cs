using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    private Transform _spriteTransform;
    [SerializeField]
    private Animator _animator;

    #endregion

    private Rigidbody2D _rigidBody;

    private float _moveSpeed;   // �̵� �ӵ�.
    private float _jumpPower;   // ���� �Ŀ� ��ġ. ���� ���� ���� ��ȭ �� �� ��������...
    private bool _isJump;

    private void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();

        _jumpPower = 700;
        _moveSpeed = 4f;
        _isJump = false;
    }

    private void Update()
    {
        if(IngameManager.Instance.GameState == Enums.eGameState.Playing)
        {
            Vector3 velocity = _rigidBody.velocity;
            Vector3 bottomRot = Vector3.zero;
            float yVel = velocity.y;        // �������� �� ���Ǵ� y�� �ӵ� ����.

            // �ٴ� �Ǻ�.
            RaycastHit2D rayHit = Physics2D.Raycast(this.transform.position, Vector3.down, 2f, LayerMask.GetMask("Background"));
            if (rayHit.collider != null)
            {
                // �ٴ� ���� ���.
                var angle = Vector2.SignedAngle(Vector2.up, rayHit.normal);
                bottomRot = Vector3.forward * angle;

                if (_isJump && yVel < 0f && rayHit.distance < 0.2f)
                {
                    _isJump = false;

                    if (_animator != null)
                    {
                        _animator.SetBool(ConstantValues.ANIMATOR_BOOL_JUMP_OLLIE, false);
                    }
                }

                _spriteTransform.localRotation = _isJump ? Quaternion.identity : Quaternion.Euler(bottomRot);
            }
            _rigidBody.velocity = _rigidBody.transform.right * _moveSpeed + bottomRot * _moveSpeed + Vector3.up * yVel;

            //Debug.DrawRay(this.transform.position, _rigidBody.velocity, Color.red);
        }
    }

    /// <summary>
    /// �÷��̾� ���� ����.
    /// </summary>
    public void Jump()
    {
        _isJump = true;

        if(_animator != null)
        {
            _animator.SetBool(ConstantValues.ANIMATOR_BOOL_JUMP_OLLIE, true);
        }

        Vector3 velocity = _rigidBody.velocity;
        velocity.y = 0f;
        _rigidBody.velocity = velocity;
        _rigidBody.AddForce(new Vector2(0f, _jumpPower));
    }

    /// <summary>
    /// �÷��̾� �̵� ����.
    /// </summary>
    public void Stop()
    {
        _rigidBody.gravityScale = 0f;
        _rigidBody.velocity = Vector3.zero;
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
