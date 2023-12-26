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

    private float _moveSpeed;   // �̵� �ӵ�.
    private float _jumpPower;   // ���� �Ŀ� ��ġ. ���� ���� ���� ��ȭ �� �� ��������...
    private ePlayerState _state;

    private float _grindInputTick;          // �׶��ε� ��ư �Է� ��� ƽ.
    private bool _isPressGrindButton;       // �׶��ε� ��ư ��ġ ������.
    private Collider2D _isCurrentGrindCollider;     // ���� �׶��ε� ���� �⹰ �ݶ��̴�.

    private void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();

        _jumpPower = 730;
        _moveSpeed = 5f;
    }

    /// <summary>
    /// �÷��̾� �ʱ�ȭ.
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
                    if(rayHit.collider.CompareTag(ConstantValues.TAG_GRIND_OBJECT))
                    {
                        // �׶��ε� �⹰�� �����ϸ� �׶��ε� ����.
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
                    // �׶��ε� �⹰���� �������, �ٽ� �޸��� ����.
                    if (!rayHit.collider.CompareTag(ConstantValues.TAG_GRIND_OBJECT))
                    {
                        SetRun();
                        IngameManager.Instance.SetGrindMode(false);
                    }
                    else if(!_isPressGrindButton)
                    {
                        // �׶��ε� ���� ��, 0.5 �� ��ư �Է� ��� �ð�.
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
    /// �÷��̾� ���� ����.
    /// </summary>
    /// <param name="skillIndex"></param>
    public void Jump(int skillIndex)
    {
        // ���� ��/���� �� �� �� ���� ����.
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

        // ������ ����� �� y �ӵ��� 0 ������ ���� ���� �� 0���� �ʱ�ȭ.
        Vector3 velocity = _rigidBody.velocity;
        velocity.y = 0.1f;
        _rigidBody.velocity = velocity;
        _rigidBody.AddForce(new Vector2(0f, _jumpPower));
    }

    /// <summary>
    /// �׶��ε� ���� ����.
    /// </summary>
    public void Grind()
    {
        if(_state == ePlayerState.Grind && !_isPressGrindButton)
        {
            _isPressGrindButton = true;
        }
    }

    /// <summary>
    /// �׶��ε� ����.
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
    /// �⺻ �޸��� ���� ����.
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
    /// �÷��̾� �̵� ����.
    /// </summary>
    public void Stop()
    {
        _rigidBody.Sleep();
    }

    /// <summary>
    /// �÷��̾� ��� �ִϸ��̼� ���.
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
    /// �ִϸ����� Idle ����.
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
            case ConstantValues.TAG_COIN:
                IngameManager.Instance.GetCoin();
                break;
            case ConstantValues.TAG_END_POINT:
                IngameManager.Instance.EndGame(true);
                break;
        }
    }

    /// <summary>
    /// �浹�ڽ� ���� ó��.
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
