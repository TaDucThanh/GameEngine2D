using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(AudioSource), typeof(Collider2D))]
public class HealthPickUp : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private HealthPickUpData _healthPickUpData;
    [SerializeField] private Vector3 _spinningAngle = new Vector3(0, 180, 0);
    #endregion

    #region Private Fields
    private AudioSource _pickupAudio;
    private SpriteRenderer _spriteRenderer;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _healthPickUpData._healthPickUpSprite;

        _pickupAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.eulerAngles += _spinningAngle * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamagableAndHealable target = collision.GetComponent<DamagableAndHealable>();
        if (target == null || target.CurrentHealth >= target.MaxHealth)
            return;

        bool healed = target.Heal(_healthPickUpData._healthRestore);
        if (healed)
        {
            PlayPickupSound();
            Destroy(gameObject);
        }
    }
    #endregion

    #region Private Methods
    private void PlayPickupSound()
    {
        if (_pickupAudio != null && _pickupAudio.clip != null)
        {
            AudioSource.PlayClipAtPoint(_pickupAudio.clip, transform.position, _pickupAudio.volume);
        }
    }
    #endregion
}
