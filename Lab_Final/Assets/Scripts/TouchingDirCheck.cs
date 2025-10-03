using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class TouchingDirCheck : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private float _checkGroundRadius = 0.2f;
    [SerializeField] private float _checkCliffRadius= 0.2f;
    [SerializeField] private float _checkWallDistance = 0.05f;
    [SerializeField] private float _checkCeilingDistance = 0.75f;
    [SerializeField] private LayerMask _groundAndWallLayer;
    [SerializeField] private Transform _groundCheckArea;
    [SerializeField] private Transform _cliffCheckArea;
    #endregion

    #region Private Fields
    private CapsuleCollider2D _capsuleCollider;
    private Animator _animator;
    #endregion

    #region Properties
    private bool _isGrounded;
    public bool IsGrounded
    {
        get => _isGrounded;
        private set
        {
            _isGrounded = value;
            _animator.SetBool("isGrounded", value);
        }
    }
    private bool _isOnCliff;
    public bool IsOnCliff
    {
        get => _isOnCliff;
        private set => _isOnCliff = value;
    }

    private bool _isOnWall;
    public bool IsOnWall
    {
        get => _isOnWall;
        private set
        {
            _isOnWall = value;
            _animator.SetBool("isOnWall", value);
        }
    }

    private bool _isOnCeiling;
    public bool IsOnCeiling
    {
        get => _isOnCeiling;
        private set
        {
            _isOnCeiling = value;
            _animator.SetBool("isOnCeiling", value);
        }
    }

    private Vector2 WallCheckDirection => transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        CheckGround();
        CheckCliff();
        CheckWall();
        CheckCeiling();
    }

    private void OnDrawGizmos()
    {
        DrawGroundGizmo();
        DrawCliffGizmo();
        DrawWallGizmo();
        DrawCeilingGizmo();

    }
    #endregion

    #region Checks
    private void CheckGround()
    {
        if (_groundCheckArea != null)
            IsGrounded = Physics2D.OverlapCircle(_groundCheckArea.position, _checkGroundRadius, _groundAndWallLayer);
    }
    private void CheckCliff()
    {
        if (_cliffCheckArea != null)
            IsOnCliff = !Physics2D.OverlapCircle(_cliffCheckArea.position, _checkCliffRadius, _groundAndWallLayer);
    }

    private void CheckWall()
    {
        Vector2 origin = _capsuleCollider.bounds.center;
        RaycastHit2D hit = Physics2D.Raycast(origin, WallCheckDirection, _checkWallDistance, _groundAndWallLayer);
        IsOnWall = hit.collider != null;
    }

    private void CheckCeiling()
    {
        Vector2 origin = _capsuleCollider.bounds.center;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.up, _checkCeilingDistance, _groundAndWallLayer);
        IsOnCeiling = hit.collider != null;
    }
    #endregion

    #region Gizmos
    private void DrawGroundGizmo()
    {
        if (_groundCheckArea == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheckArea.position, _checkGroundRadius);
    }
    private void DrawCliffGizmo()
    {
        if (_cliffCheckArea == null) return;
        Gizmos.color = Color.yellow; // Màu khác dễ phân biệt
        Gizmos.DrawWireSphere(_cliffCheckArea.position, _checkCliffRadius);
    }
    private void DrawWallGizmo()
    {
        if (_capsuleCollider == null) return;
        Gizmos.color = Color.blue;
        Vector3 start = _capsuleCollider.bounds.center;
        Vector3 end = start + (Vector3)WallCheckDirection * _checkWallDistance;
        Gizmos.DrawLine(start, end);
    }

    private void DrawCeilingGizmo()
    {
        if (_capsuleCollider == null) return;
        Gizmos.color = Color.green;
        Vector3 start = _capsuleCollider.bounds.center;
        Vector3 end = start + Vector3.up * _checkCeilingDistance;
        Gizmos.DrawLine(start, end);
    }

   

    #endregion
}
