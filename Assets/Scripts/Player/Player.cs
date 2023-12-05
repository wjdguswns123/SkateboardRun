using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    private enum ePlayerState { Run, Jump, Fall }

    #region Inspector

    [SerializeField]
    private Transform _spriteTransform;
    [SerializeField]
    private Animator _animator;

    #endregion

    private Rigidbody2D _rigidBody;

    private float _moveSpeed;   // �̵� �ӵ�.
    private float _jumpPower;   // ���� �Ŀ� ��ġ. ���� ���� ���� ��ȭ �� �� ��������...
    private ePlayerState _state;

    private void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();

        _jumpPower = 700;
        _moveSpeed = 4f;
    }

    /// <summary>
    /// �÷��̾� �ʱ�ȭ.
    /// </summary>
    public void Init()
    {
        _state = ePlayerState.Run;
        _rigidBody.WakeUp();

        if (_animator != null)
        {
            _animator.SetBool(ConstantValues.ANIMATOR_BOOL_JUMP_OLLIE, false);
        }
    }

    private void FixedUpdate()
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

                if (_state == ePlayerState.Jump && yVel <= 0f && rayHit.distance < 0.2f)
                {
                    _state = ePlayerState.Run;

                    if (_animator != null)
                    {
                        _animator.SetBool(ConstantValues.ANIMATOR_BOOL_JUMP_OLLIE, false);
                    }
                }

                _spriteTransform.localRotation = _state == ePlayerState.Jump ? Quaternion.identity : Quaternion.Euler(bottomRot);
            }

            if(_state != ePlayerState.Fall)
            {
                _rigidBody.velocity = _rigidBody.transform.right * _moveSpeed + bottomRot * _moveSpeed + Vector3.up * yVel;
            }
            //Debug.DrawRay(this.transform.position, _rigidBody.velocity, Color.red);
        }
    }

    /// <summary>
    /// �÷��̾� ���� ����.
    /// </summary>
    public void Jump()
    {
        // ���� ��/���� �� �� �� ���� ����.
        if(_state == ePlayerState.Jump || _state == ePlayerState.Fall)
        {
            return;
        }

        _state = ePlayerState.Jump;

        if (_animator != null)
        {
            _animator.SetBool(ConstantValues.ANIMATOR_BOOL_JUMP_OLLIE, true);
        }

        // ������ ����� �� y �ӵ��� 0 ������ ���� ���� �� 0���� �ʱ�ȭ.
        Vector3 velocity = _rigidBody.velocity;
        velocity.y = 0.1f;
        _rigidBody.velocity = velocity;
        _rigidBody.AddForce(new Vector2(0f, _jumpPower));
    }

    /// <summary>
    /// �÷��̾� �̵� ����.
    /// </summary>
    public void Stop()
    {
        _rigidBody.Sleep();
    }

    /// <summary>
    /// Ʈ���� ���� ó��.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case ConstantValues.TAG_HURDLE:
                IngameManager.Instance.EndGame(false);       // ��ֹ��� �浹�ϸ� ���� ����.
                break;
            case ConstantValues.TAG_HOLE:
                _state = ePlayerState.Fall;
                break;
            case ConstantValues.TAG_END_POINT:
                IngameManager.Instance.EndGame(true);
                break;
        }
    }

    /// <summary>
    /// Ʈ���� ���� ������ ó��.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag(ConstantValues.TAG_HOLE))
        {
            IngameManager.Instance.EndGame(false);
        }
    }
}
