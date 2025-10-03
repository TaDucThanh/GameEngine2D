using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Shuriken : MonoBehaviour
{
    #region Fields
    [SerializeField] private float _damage = 12.3f;
    [SerializeField] private Vector2 _knockBack = new(4f, 2f);
    [SerializeField] private Vector2 _throwForce = new(30f, 0f);
    [SerializeField] private float _spinSpeed = 360f;

    private Rigidbody2D _rb;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        ResetPhysics();
        Throw();
    }

    private void Update()
    {
        Spin();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
        ReturnToPool();
    }
    #endregion

    #region Methods
    private void ResetPhysics()
    {
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;
    }

    private void Throw()
    {
        int direction = Mathf.Sign(transform.localScale.x) > 0 ? 1 : -1;
        _rb.AddForce(new Vector2(_throwForce.x * direction, _throwForce.y), ForceMode2D.Impulse);
    }

    private void Spin()
    {
        transform.Rotate(0f, 0f, _spinSpeed * Time.deltaTime, Space.Self);
    }

    private void HandleCollision(Collider2D collision)
    {
        DamagableAndHealable damagable = collision.GetComponent<DamagableAndHealable>();
        if (damagable == null) return;

        int direction = Mathf.Sign(transform.localScale.x) > 0 ? 1 : -1;
        Vector2 knockbackDir = new(_knockBack.x * direction, _knockBack.y);

        if (!damagable.Hit(_damage, knockbackDir))
            Debug.Log("Target is either dead or invincible.");
    }

    private void ReturnToPool()
    {
        SpawningObjectInPool.Instance.ReturnObject(gameObject);
    }
    #endregion
}
