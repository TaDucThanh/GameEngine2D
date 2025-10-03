using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

[ExecuteAlways]
public class DetectionZone : MonoBehaviour
{
    #region Fields
    private Collider2D _collider;
    private readonly List<Collider2D> _detectedColliders = new List<Collider2D>();
    private List<Collider2D> results = new List<Collider2D>();
    #endregion

    #region Properties
    public IReadOnlyList<Collider2D> DetectedColliders => _detectedColliders;

    public bool HasAliveTarget
    {
        get
        {
            foreach (var collider in _detectedColliders)
            {
                var damagableandhealable = collider.GetComponent<DamagableAndHealable>();
                if (damagableandhealable != null && damagableandhealable.IsAlive)
                    return true;
            }
            return false;
        }
    }
    public bool HasAnyTarget => _detectedColliders.Count > 0;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        if (_collider == null)
            Debug.LogError($"{nameof(DetectionZone)} requires a Collider2D component.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_detectedColliders.Contains(collision))
            _detectedColliders.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _detectedColliders.Remove(collision);
    }
    private void OnDrawGizmos()
    {
        if (_collider == null)
            _collider = GetComponent<Collider2D>();

        if (_collider == null) return;

        Gizmos.color = Color.yellow;

        if (_collider is BoxCollider2D box)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(box.offset, box.size);
        }
        else if (_collider is CircleCollider2D circle)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireSphere(circle.offset, circle.radius);
        }
        else if (_collider is CapsuleCollider2D capsule)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Vector3 size = new Vector3(capsule.size.x, capsule.size.y, 0f);
            Gizmos.DrawWireCube(capsule.offset, size);
        }
    }
    #endregion
}
