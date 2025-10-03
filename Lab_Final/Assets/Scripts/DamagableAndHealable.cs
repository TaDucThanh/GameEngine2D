using System;
using UnityEngine;
using UnityEngine.Events;

public class DamagableAndHealable : MonoBehaviour
{
    #region Events
    public UnityEvent<float, Vector2> OnDamaged;     // damage, knockback
    public UnityEvent<float, float> OnHealthChanged; // currentHealth, maxHealth
    #endregion

    #region Fields
    private Animator _animator;
    private ICharacter _character;
    private CharacterData _characterData;

    private float _timeSinceHit;
    private bool _isInvincible;
    private float _currentHealth;
    #endregion

    #region Properties
    private float _maxHealth = 100f;
    public float MaxHealth => _maxHealth;

    public float CurrentHealth
    {
        get => _currentHealth;
        private set
        {
            _currentHealth = value;
            OnHealthChanged?.Invoke(_currentHealth, MaxHealth);

            if (_currentHealth <= 0)
                IsAlive = false;
        }
    }
    public bool IsAlive
    {
        get => _animator.GetBool("isAlive");
        set => _animator.SetBool("isAlive", value);
    }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _character = GetComponent<ICharacter>();

        if (_character == null)
        {
            Debug.LogError("No ICharacter component found!");
            return;
        }

        _characterData = _character.GetCharacterData();
        if (_characterData == null)
        {
            Debug.LogError("CharacterData not assigned!");
            return;
        }
        _maxHealth = _characterData._maxHealth;
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        if (_isInvincible)
        {
            _timeSinceHit += Time.deltaTime;

            if (_timeSinceHit > _characterData._invincibleTime)
            {
                _isInvincible = false;
                _timeSinceHit = 0f;
            }
        }
    }
    #endregion

    #region Public Methods
    public bool Hit(float damage, Vector2 knockBack)
    {
        if (!IsAlive || _isInvincible)
            return false;

        CurrentHealth -= damage;
        _isInvincible = true;

        _animator.SetTrigger("hit");
        OnDamaged?.Invoke(damage, knockBack); // trigger knockback
        GameEvents.characterDamaged?.Invoke(gameObject, damage);

        return true;
    }

    public bool Heal(float healthRestore)
    {
        if (!IsAlive || CurrentHealth >= MaxHealth)
            return false;

        float healAmount = Mathf.Min(healthRestore, MaxHealth - CurrentHealth);
        CurrentHealth += healAmount;

        GameEvents.characterHealed?.Invoke(gameObject, healAmount);
        return true;
    }

    public void ResetRunTimeData()
    {
        Vector3 StartPos = new Vector3(-9.5f, -3.1f, 0f);
        _isInvincible = false;
        _timeSinceHit = 0f;

        IsAlive = true;

        CurrentHealth = MaxHealth;

        if (_animator != null)
        {
            _animator.Rebind();
            _animator.Update(0f);
        }
        PlayerController.Instance.SpawnPos(StartPos);
    }
    #endregion
}
