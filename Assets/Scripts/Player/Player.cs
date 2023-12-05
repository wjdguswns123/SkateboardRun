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

    private float _moveSpeed;   // 이동 속도.
    private float _jumpPower;   // 점프 파워 수치. 레벨 업을 통해 강화 할 수 있을지도...
    private ePlayerState _state;

    private void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();

        _jumpPower = 700;
        _moveSpeed = 4f;
    }

    /// <summary>
    /// 플레이어 초기화.
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
            float yVel = velocity.y;        // 점프했을 때 사용되는 y축 속도 저장.

            // 바닥 판별.
            RaycastHit2D rayHit = Physics2D.Raycast(this.transform.position, Vector3.down, 2f, LayerMask.GetMask("Background"));
            if (rayHit.collider != null)
            {
                // 바닥 각도 계산.
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
    /// 플레이어 점프 실행.
    /// </summary>
    public void Jump()
    {
        // 점프 중/낙하 중 일 땐 점프 안함.
        if(_state == ePlayerState.Jump || _state == ePlayerState.Fall)
        {
            return;
        }

        _state = ePlayerState.Jump;

        if (_animator != null)
        {
            _animator.SetBool(ConstantValues.ANIMATOR_BOOL_JUMP_OLLIE, true);
        }

        // 내리막 경사일 때 y 속도가 0 이하인 것을 점프 시 0으로 초기화.
        Vector3 velocity = _rigidBody.velocity;
        velocity.y = 0.1f;
        _rigidBody.velocity = velocity;
        _rigidBody.AddForce(new Vector2(0f, _jumpPower));
    }

    /// <summary>
    /// 플레이어 이동 정지.
    /// </summary>
    public void Stop()
    {
        _rigidBody.Sleep();
    }

    /// <summary>
    /// 트리거 접촉 처리.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case ConstantValues.TAG_HURDLE:
                IngameManager.Instance.EndGame(false);       // 장애물에 충돌하면 게임 종료.
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
    /// 트리거 접촉 나가기 처리.
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
