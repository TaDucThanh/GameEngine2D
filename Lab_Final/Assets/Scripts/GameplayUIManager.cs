using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameplayUIManager : MonoBehaviour
{
    #region Singleton
    public static GameplayUIManager Instance { get; private set; }
    #endregion

    [Header("Mobile Buttons")]
    public MobileButton LeftButton;
    public MobileButton RightButton;
    public GameObject JumpButton;
    public GameObject AttackButton;
    public GameObject ProjectileButton;


    [Header("UI Health Change")]
    [SerializeField] private GameObject _damageTextPrefab;
    [SerializeField] private GameObject _healthTextPrefab;
    [Header("Game Over UI")]
    [SerializeField] private GameObject _pauseMenuUI;
    [SerializeField] private GameObject _gameOverMenuUI;

    #region Private Fields
    private Canvas _gameCanvas;
    #endregion

    #region Unity Life Cycle
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (LeftButton == null) Debug.LogWarning("LeftButton chưa gán!");
        if (RightButton == null) Debug.LogWarning("RightButton chưa gán!");

        _gameCanvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        GameEvents.characterDamaged += ShowDamageText;
        GameEvents.characterHealed += ShowHealText;
    }

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        NotDisplayMobileBTn();
#endif
    }

    private void OnDisable()
    {
        GameEvents.characterDamaged -= ShowDamageText;
        GameEvents.characterHealed -= ShowHealText;
    }
    #endregion

    #region Public Methods
        public void OnPauseGame(InputAction.CallbackContext context)
        {
        if (!context.started) return;
        Time.timeScale = 0f;
        _pauseMenuUI.SetActive(true);
    }
   
    public void OnResumeGame()
    {
        Time.timeScale = 1f;
        _pauseMenuUI.SetActive(false);
    }
    public void ExitToMainMenu()
    {
        GameSceneManager.Instance.LastScene = SceneManager.GetActiveScene().name;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }
    public void OnGameOver()
    {
        Time.timeScale = 0f;
        _gameOverMenuUI.SetActive(true);
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        _gameOverMenuUI.SetActive(false);
        GameSceneManager.Instance.RestartScene();
    }
    #endregion
    #region Private Methods
    private void NotDisplayMobileBTn()
    {
        LeftButton.gameObject.SetActive(false);
        RightButton.gameObject.SetActive(false);
        JumpButton.gameObject.SetActive(false);
        AttackButton.gameObject.SetActive(false);
        ProjectileButton.gameObject.SetActive(false);
    }
    private void ShowDamageText(GameObject character, float amount)
    {
        SpawnText(_damageTextPrefab, character.transform.position, amount);
    }

    private void ShowHealText(GameObject character, float amount)
    {
        SpawnText(_healthTextPrefab, character.transform.position, amount);
    }

    private void SpawnText(GameObject prefab, Vector3 worldPosition, float amount)
    {
        if (_gameCanvas == null || prefab == null) return;

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        TMP_Text tmpText = Instantiate(prefab, screenPosition, Quaternion.identity, _gameCanvas.transform)
            .GetComponent<TMP_Text>();
        tmpText.text = amount.ToString("F2");
    }
    #endregion
}
