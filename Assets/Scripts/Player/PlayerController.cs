using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private enum ePlayerState { Run, Jump, Grind, Fall }

    #region Inspector

    [SerializeField]
    private Transform _spriteTransform;
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Transform _centerEffectPosition;
    [SerializeField]
    private Transform _bottomEffectPosition;

    #endregion

    private Rigidbody2D _rigidBody;

    private float _moveSpeed;   // 이동 속도.
    private float _jumpPower;   // 점프 파워 수치. 레벨 업을 통해 강화 할 수 있을지도...
    private ePlayerState _state;

    private float _grindInputTick;          // 그라인드 버튼 입력 대기 틱.
    private bool _isPressGrindButton;       // 그라인드 버튼 터치 중인지.
    private Collider2D _isCurrentGrindCollider;     // 현재 그라인드 중인 기물 콜라이더.

    private void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();

        _jumpPower = 730;
        _moveSpeed = 5f;
    }

    /// <summary>
    /// 플레이어 초기화.
    /// </summary>
    public void Init()
    {
        _state = ePlayerState.Run;
        _rigidBody.WakeUp();

        _grindInputTick = 0f;
        _isPressGrindButton = false;
        _isCurrentGrindCollider = null;

        if (_animator != null)
        {
            _animator.Rebind();
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
                    if(rayHit.collider.CompareTag(ConstantValues.TAG_GRIND_OBJECT))
                    {
                        // 그라인드 기물에 착지하면 그라인드 모드로.
                        _state = ePlayerState.Grind;
                        _grindInputTick = 0f;
                        _isCurrentGrindCollider = rayHit.collider;
                        IngameManager.Instance.SetGrindMode(true);
                        if(_animator != null)
                        {
                            _animator.SetBool(ConstantValues.ANIMATOR_BOOL_JUMP_OLLIE, false);
                            _animator.SetBool(ConstantValues.ANIMATOR_BOOL_JUMP_SKILL, false);
                            _animator.SetBool(ConstantValues.ANIMATOR_BOOL_GRIND, true);
                        }
                    }
                    else
                    {
                        EffectSystem landingEffect = EffectManager.Instance.LoadEffect("Effect_Landing", pos: _bottomEffectPosition.position);
                        landingEffect.gameObject.SetActive(true);
                        SetRun(true);
                    }
                }
                else if(_state == ePlayerState.Grind)
                {
                    // 그라인드 기물에서 벗어났으면, 다시 달리기 모드로.
                    if (!rayHit.collider.CompareTag(ConstantValues.TAG_GRIND_OBJECT))
                    {
                        SetRun();
                        IngameManager.Instance.SetGrindMode(false);
                    }
                    else if(!_isPressGrindButton)
                    {
                        // 그라인드 시작 후, 0.5 초 버튼 입력 대기 시간.
                        if(_grindInputTick < 0.5f)
                        {
                            _grindInputTick += Time.deltaTime;
                        }
                        else
                        {
                            rayHit.collider.enabled = false;
                            SetRun();
                            IngameManager.Instance.SetGrindMode(false);
                        }
                    }
                }

                _spriteTransform.localRotation = _state == ePlayerState.Jump ? Quaternion.identity : Quaternion.Euler(bottomRot);
            }

            if (_state != ePlayerState.Fall)
            {
                _rigidBody.velocity = _rigidBody.transform.right * _moveSpeed + bottomRot * _moveSpeed + Vector3.up * yVel;
            }
            Debug.DrawRay(this.transform.position, _rigidBody.velocity, Color.red);
        }
    }

    /// <summary>
    /// 플레이어 점프 실행.
    /// </summary>
    /// <param name="skillIndex"></param>
    public void Jump(int skillIndex)
    {
        // 점프 중/낙하 중 일 땐 점프 안함.
        if(_state == ePlayerState.Jump || _state == ePlayerState.Fall)
        {
            return;
        }

        _state = ePlayerState.Jump;

        if (_animator != null)
        {
            switch(skillIndex)
            {
                case 0:
                    _animator.SetBool(ConstantValues.ANIMATOR_BOOL_JUMP_OLLIE, true);
                    break;
                case 1:
                case 2:
                    _animator.SetBool(ConstantValues.ANIMATOR_BOOL_JUMP_SKILL, true);
                    break;
            }
        }

        // 내리막 경사일 때 y 속도가 0 이하인 것을 점프 시 0으로 초기화.
        Vector3 velocity = _rigidBody.velocity;
        velocity.y = 0.1f;
        _rigidBody.velocity = velocity;
        _rigidBody.AddForce(new Vector2(0f, _jumpPower));
    }

    /// <summary>
    /// 그라인드 유지 시작.
    /// </summary>
    public void Grind()
    {
        if(_state == ePlayerState.Grind && !_isPressGrindButton)
        {
            _isPressGrindButton = true;
        }
    }

    /// <summary>
    /// 그라인드 종료.
    /// </summary>
    public void EndGrind()
    {
        if (_state == ePlayerState.Grind && _isPressGrindButton)
        {
            _isPressGrindButton = false;
            _isCurrentGrindCollider.enabled = false;
            _isCurrentGrindCollider = null;
            SetRun();
            IngameManager.Instance.SetGrindMode(false);
        }
    }

    /// <summary>
    /// 기본 달리기 모드로 설정.
    /// </summary>
    /// <param name="isShake"></param>
    private void SetRun(bool isShake = false)
    {
        _state = ePlayerState.Run;
        SetRunAnimator();
        if(isShake)
        {
            CameraManager.Instance.CameraShake(time: 0.1f, range: 0.5f);
        }
    }

    /// <summary>
    /// 플레이어 이동 정지.
    /// </summary>
    public void Stop()
    {
        _rigidBody.Sleep();
    }

    /// <summary>
    /// 플레이어 사망 애니메이션 출력.
    /// </summary>
    public void Die()
    {
        if (_animator != null)
        {
            _animator.SetTrigger(ConstantValues.ANIMATOR_TRIGGER_DIE);
        }
        Stop();
    }

    /// <summary>
    /// 애니메이터 Idle 설정.
    /// </summary>
    private void SetRunAnimator()
    {
        if (_animator != null)
        {
            _animator.SetBool(ConstantValues.ANIMATOR_BOOL_JUMP_OLLIE, false);
            _animator.SetBool(ConstantValues.ANIMATOR_BOOL_JUMP_SKILL, false);
            _animator.SetBool(ConstantValues.ANIMATOR_BOOL_GRIND, false);
        }
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
            case ConstantValues.TAG_COIN:
                IngameManager.Instance.GetCoin();
                break;
            case ConstantValues.TAG_END_POINT:
                IngameManager.Instance.EndGame(true);
                break;
        }
    }

    /// <summary>
    /// 충돌박스 접촉 처리.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.tag)
        {
            case ConstantValues.TAG_GRIND_OBJECT:
                {
                    if (_state != ePlayerState.Grind)
                    {
                        IngameManager.Instance.EndGame(false);
                    }
                }
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
