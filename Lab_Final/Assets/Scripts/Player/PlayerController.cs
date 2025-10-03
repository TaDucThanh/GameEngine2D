using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(TouchingDirCheck))]
public class PlayerController : MonoBehaviour, ICharacter
{
    public static PlayerController Instance { get; private set; }

    #region Fields

    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] public CharacterData characterData;
    [SerializeField] private float _stopRate = 0.1f;

    private DamagableAndHealable _damagable;
    private TouchingDirCheck _touchingDirCheck;
    private Rigidbody2D _rb;
    private Animator _animator;

    private Vector2 _moveInput;
    private bool _isMoving = false;
    private bool _isFacingRight = true;

    private MobileButton _leftBtn => GameplayUIManager.Instance.LeftButton;
    private MobileButton _rightBtn => GameplayUIManager.Instance.RightButton;

    #endregion

    #region Properties

    public CharacterData GetCharacterData() => characterData;

    public bool IsMoving
    {
        get => _isMoving;
        private set
        {
            _isMoving = value;
            _animator.SetBool("isMoving", value);
        }
    }

    public float Speed
    {
        get
        {
            if (!CanMove) return 0;

            if (_touchingDirCheck.IsGrounded && !_touchingDirCheck.IsOnWall)
                return characterData._speed;

            if (!_touchingDirCheck.IsGrounded)
                return characterData._jumpSpeed;

            return 0;
        }
    }

    public bool IsFacingRight
    {
        get => _isFacingRight;
        private set
        {
            if (_isFacingRight != value)
                transform.localScale *= new Vector2(-1, 1);

            _isFacingRight = value;
        }
    }

    public bool CanMove => _animator.GetBool("canMove");
    private bool BeingHit => _animator.GetBool("beingHit");

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _animator.Rebind();
        _animator.Update(0f);
        _touchingDirCheck = GetComponent<TouchingDirCheck>();
        _damagable = GetComponent<DamagableAndHealable>();
    }
    private void Start()
    {
        Vector3 StartPos = new Vector3(-9.5f, -3.1f, 0f);
        SpawnPos(StartPos);
    }
    private void FixedUpdate()
    {
        OnMoveUpdate();

        if (BeingHit) return;

        if (!IsMoving || !CanMove)
            _rb.linearVelocity = new Vector2(Mathf.Lerp(_rb.linearVelocity.x, 0, _stopRate), _rb.linearVelocity.y);
        else
            _rb.linearVelocity = new Vector2(_moveInput.x * Speed, _rb.linearVelocity.y);

        _animator.SetFloat("yVelocity", _rb.linearVelocity.y);
    }
    #endregion

    #region Actions
    public void SpawnPos(Vector3 Pos)
    {
        transform.position = new Vector3(-9.5f, -3f, 0f);
    }
    private void OnMoveUpdate()
    {
        if (!_damagable.IsAlive || BeingHit)
        {
            _moveInput = Vector2.zero;
            IsMoving = false;
            return;
        }

#if UNITY_STANDALONE || UNITY_EDITOR
        if (_playerInput != null && _playerInput.actions != null)
            _moveInput = _playerInput.actions["Move"].ReadValue<Vector2>();
#endif

#if UNITY_ANDROID || UNITY_IOS
        int dir = 0;
        if (_leftBtn.IsPressed) dir = -1;
        if (_rightBtn.IsPressed) dir = 1;
        _moveInput = new Vector2(dir, 0);
#endif

        SetFacingDirection(_moveInput.x);
        IsMoving = _moveInput != Vector2.zero;
    }
    private void SetFacingDirection(float moveInputX)
    {
        if (moveInputX > 0)
            IsFacingRight = true;
        else if (moveInputX < 0)
            IsFacingRight = false;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!_damagable.IsAlive) return;

        if (context.started && _touchingDirCheck.IsGrounded &&
            Mathf.Abs(_rb.linearVelocity.y) < 0.01f && CanMove)
        {
            _animator.SetTrigger("jump");
            _rb.AddForce(Vector2.up * characterData._jumpForce, ForceMode2D.Impulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!_damagable.IsAlive) return;

        if (context.started)
            _animator.SetTrigger("attack");
    }

    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (!_damagable.IsAlive) return;

        if (context.started)
            _animator.SetTrigger("rangedAttack");
    }

    public void OnHit(float damage, Vector2 knockBack)
    {
        if (!_damagable.IsAlive) return;

        _rb.linearVelocity = new Vector2(knockBack.x, _rb.linearVelocity.y + knockBack.y);
    }
    public void Throw()
    {
        SpawningObjectInPool.Instance.GetObject();
    }
    public void OnDeath()
    {
        GameplayUIManager.Instance.OnGameOver();
    }
    #endregion

}
