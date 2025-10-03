using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private DamagableAndHealable _playerHealth;
    [SerializeField] private int _decimalPlaces = 1; // số chữ số thập phân hiển thị
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        if (_playerHealth == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("No player found in the scene. Make sure it has the tag 'Player'.");
                return;
            }

            _playerHealth = player.GetComponent<DamagableAndHealable>();
        }
    }

    private void OnEnable()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnHealthChanged.AddListener(OnPlayerHealthChanged);
            UpdateHealthUI(_playerHealth.CurrentHealth, _playerHealth.MaxHealth);
        }
    }

    private void OnDisable()
    {
        if (_playerHealth != null)
            _playerHealth.OnHealthChanged.RemoveListener(OnPlayerHealthChanged);
    }
    #endregion

    #region Private Methods
    private void OnPlayerHealthChanged(float newHealth, float maxHealth)
    {
        UpdateHealthUI(newHealth, maxHealth);
    }

    private void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        _healthSlider.value = currentHealth / maxHealth;
        string format = "F" + _decimalPlaces;
        _healthText.text = $"{currentHealth.ToString(format)} / {maxHealth.ToString(format)}";
    }
    #endregion
}
