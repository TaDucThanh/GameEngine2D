using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform), typeof(TextMeshProUGUI))]
public class HealthText : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private Vector3 _moveVector = new Vector2(0, 360);
    [SerializeField] private float _timeToFade = 1f;
    #endregion

    #region Private Fields
    private RectTransform _rectTransform;
    private TextMeshProUGUI _textMeshPro;
    private Color _startColor;
    private float _timeElapsed = 0f;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        _startColor = _textMeshPro.color;
    }

    private void Update()
    {
        MoveAndFade();
    }
    #endregion

    #region Private Methods
    private void MoveAndFade()
    {
        _rectTransform.position += _moveVector * Time.deltaTime;

        _timeElapsed += Time.deltaTime;
        float alpha = Mathf.Lerp(_startColor.a, 0f, _timeElapsed / _timeToFade);
        _textMeshPro.color = new Color(_startColor.r, _startColor.g, _startColor.b, alpha);

        if (_timeElapsed >= _timeToFade)
        {
            Destroy(gameObject);
        }
    }
    #endregion
}
