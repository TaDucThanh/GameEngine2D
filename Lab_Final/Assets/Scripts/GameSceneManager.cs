using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    private static string _lastScene = "MenuScene";
    public string LastScene { get => _lastScene; set => _lastScene = value; }
    public bool Ingameplayscene => GameplayUIManager.Instance != null;
    #region Singleton
    public static GameSceneManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    

    #region Public Methods
    public void PlayGame()
    {
        
        if (!Ingameplayscene)
        {
            SceneManager.LoadScene("Level1Scene");
            AudioManager.Instance?.PlayMusic("GameplayBackgroundMusic");
            Time.timeScale = 1f;
        }
        else
        {
            SceneManager.LoadScene(LastScene);
            Time.timeScale = 0f;
            AudioManager.Instance?.PlayMusic("GameplayBackgroundMusic");
        }
        
    }
    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
        Time.timeScale = 1f;
        AudioManager.Instance?.PlayMusic("GameplayBackgroundMusic");
    }
    public void RestartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        SceneManager.sceneLoaded += OnSceneReloaded;
    }
    private void OnSceneReloaded(Scene scene, LoadSceneMode mode)
    {
        var player = PlayerController.Instance;
        if (player != null)
        {
            var damageableandhealable = player.GetComponent<DamagableAndHealable>();
            damageableandhealable.ResetRunTimeData();
        }
        SceneManager.sceneLoaded -= OnSceneReloaded;
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // thoát khi ch?y trong editor
#endif
    }
    #endregion
}
