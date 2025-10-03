using System.Collections;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour, ICharacter
{
    #region Enums
    public enum WalkAbleDirection { Left, Right }
    #endregion

    #region Serialized Fields
    [SerializeField] private DetectionZone _attackDetectionZone;
    [SerializeField] private DetectionZone _detectionZone;
    [SerializeField] public CharacterData _characterData;
    [SerializeField] private Transform _player;
    [SerializeField] private float stopRate = 0.1f;
    [SerializeField] private float _idleDuration = 2f;
    #endregion

    #region Private Fields
    private Vector2 newVelocity;
    private bool _teleportReady = false;
    private bool _justFlipped = false;
    private Coroutine _idleCoroutine;
    private Coroutine _flipAfterHitCoroutine;
    private DamagableAndHealable _damagable;
    private Rigidbody2D _rb;
    private Animator _animator;
    private TouchingDirCheck _touchingDirCheck;
    private Vector2 _walkDirectionVector;
    private WalkAbleDirection _walkDirection;
    #endregion

    #region Properties
    public WalkAbleDirection WalkDirection
    {
        get => _walkDirection;
        private set
        {
            if (_walkDirection != value)
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);

            _walkDirectionVector = (value == WalkAbleDirection.Left) ? Vector2.left : Vector2.right;
            _walkDirection = value;
        }
    }

    public CharacterData GetCharacterData() => _characterData;

    private bool _hasTarget = false;
    public bool HasAliveTarget
    {
        get => _hasTarget;
        set
        {
            _hasTarget = value;
            _animator.SetBool("hasTarget", value);
        }
    }
    private bool _playerInJumpZone = false; 
    public bool PlayerInZone
    {
        get => _playerInJumpZone;
        set
        {
            _playerInJumpZone = value;
        }
    }
    public bool BeingHit => _animator.GetBool("beingHit");
    public bool CanMove => _animator.GetBool("canMove");

    public float AttackCoolDown
    {
        get => _animator.GetFloat("attackCooldown");
        private set => _animator.SetFloat("attackCooldown", Mathf.Max(value, 0));
    }

    public bool IsMoving
    {
        get => _animator.GetBool("isMoving");
        private set => _animator.SetBool("isMoving", value);
    }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _walkDirection = transform.localScale.x > 0 ? WalkAbleDirection.Right : WalkAbleDirection.Left;
        _walkDirectionVector = (_walkDirection == WalkAbleDirection.Right) ? Vector2.right : Vector2.left;

        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _touchingDirCheck = GetComponent<TouchingDirCheck>();
        _damagable = GetComponent<DamagableAndHealable>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("⚠ Enemy không tìm thấy GameObject có tag 'Player' trong scene!");
        }
    }

    private void FixedUpdate()
    {
        if (!_damagable.IsAlive || BeingHit || !IsMoving) return;
        newVelocity = _rb.linearVelocity;
        if (!PlayerInZone && _touchingDirCheck.IsGrounded)
            Patrolling();
        else if (PlayerInZone && _touchingDirCheck.IsGrounded)
        {
            newVelocity.x = 0;
            OnIdleThenAction();
        }
        else if (PlayerInZone && !_touchingDirCheck.IsGrounded)
        {
            newVelocity.x = _walkDirectionVector.x * _characterData._jumpSpeed;
        }
        _rb.linearVelocity = new Vector2(newVelocity.x, _rb.linearVelocity.y);

    }

    private void Update()
    {
        _animator.SetFloat("yVelocity", _rb.linearVelocity.y);

        if (AttackCoolDown > 0)
            AttackCoolDown -= Time.deltaTime;

        HasAliveTarget = _attackDetectionZone.HasAnyTarget && _attackDetectionZone.HasAliveTarget;
        if (!_teleportReady)
        {
            PlayerInZone = _detectionZone.HasAnyTarget;
        }
    }
    #endregion

    #region Public Methods
    public void Patrolling()
    {

        if (CanMove)
        {
            if ((_touchingDirCheck.IsOnWall || _touchingDirCheck.IsOnCliff) && _idleCoroutine == null && !_justFlipped)
            {
                newVelocity.x = 0;
                OnIdleThenAction();
            }
            else
            {
                newVelocity.x = _walkDirectionVector.x * _characterData._speed;
            }
        }
        else
        {
            newVelocity.x = Mathf.Lerp(newVelocity.x, 0, stopRate);
        }
    }

    public void OnHit(float damage, Vector2 knockBack)
    {
        if (!_damagable.IsAlive) return;

        _rb.linearVelocity = Vector2.zero;
        _rb.linearVelocity = new Vector2(knockBack.x, _rb.linearVelocity.y + knockBack.y);

        if (_flipAfterHitCoroutine != null)
            StopCoroutine(_flipAfterHitCoroutine);

        _flipAfterHitCoroutine = StartCoroutine(FlipAfterHit());
    }

    

    #endregion

    #region Private Methods
    private void OnIdleThenAction()
    {
        if (!_damagable.IsAlive) return;
        if (_idleCoroutine == null)
            _idleCoroutine = StartCoroutine(IdleCoroutine());
    }

    private void FlipDirection(Transform target = null)
    {
        if (target != null)
        {
            if (target.position.x < transform.position.x && WalkDirection != WalkAbleDirection.Left)
                WalkDirection = WalkAbleDirection.Left;
            else if (target.position.x > transform.position.x && WalkDirection != WalkAbleDirection.Right)
                WalkDirection = WalkAbleDirection.Right;
        }
        else
        {
            WalkDirection = (WalkDirection == WalkAbleDirection.Left) ? WalkAbleDirection.Right : WalkAbleDirection.Left;
        }
    }

    private void JumpAttack()
    {
        if (!_damagable.IsAlive) return;
        float distanceFromPlayer = _player.position.x - transform.position.x;

        if (_touchingDirCheck.IsGrounded)
        {
            _animator.SetTrigger("jump");
            _rb.AddForce(new Vector2(distanceFromPlayer, _characterData._jumpForce), ForceMode2D.Impulse);
        }
    }
    private void TeleBehind()
    {
        if (_player == null) return;

        float offset = 1.5f; 

        bool playerFacingRight = _player.localScale.x > 0;

        Vector3 newPos = _player.position + (playerFacingRight ? Vector3.left : Vector3.right) * offset;

        newPos.y = _player.position.y;

        transform.position = newPos;

        FlipDirection(_player);

        StartCoroutine(DisablePlayerInZoneForSeconds(10f));
    }

    #endregion

    #region Coroutines

    private IEnumerator DisablePlayerInZoneForSeconds(float duration)
    {
        _teleportReady = true;
        PlayerInZone = false;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        PlayerInZone = _detectionZone.HasAnyTarget;
        _teleportReady = false;
    }
    private IEnumerator FlipAfterHit()
    {
        if (!_damagable.IsAlive) yield break;
        yield return new WaitUntil(() => !BeingHit);

        FlipDirection(_player);
        _flipAfterHitCoroutine = null;
    }

    private IEnumerator IdleCoroutine()
    {
        if (!_damagable.IsAlive) yield break;
        IsMoving = false;

        float idleTime = (gameObject.CompareTag("Boss"))
            ? Random.Range(2f, 10f)
            : _idleDuration;

        float timer = 0f;
        while (timer < idleTime)
        {
            if (HasAliveTarget || BeingHit)
            {
                IsMoving = true;
                _idleCoroutine = null;
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (!PlayerInZone)
        {
            FlipDirection();
            IsMoving = true;
            _justFlipped = true;
            yield return new WaitForSeconds(0.5f);
            _justFlipped = false;
            _idleCoroutine = null;
        }
        else if (PlayerInZone && gameObject.CompareTag("Boss"))
        {
            JumpAttack();
            IsMoving = true;
            yield return new WaitForSeconds(1f);
            _idleCoroutine = null;
        }
        else if (PlayerInZone && gameObject.CompareTag("Enemy"))
        {
            TeleBehind();
            IsMoving = true;
            _idleCoroutine = null;
        }
    }
    #endregion
}
