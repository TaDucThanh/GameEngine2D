using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playAndContinueText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameSceneManager.Instance.Ingameplayscene)
        {
            _playAndContinueText.text = "Continue";
        }
        else
        {
            _playAndContinueText.text = "Play";
        }
    }
}
