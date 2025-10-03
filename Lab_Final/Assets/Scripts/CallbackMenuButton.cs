using UnityEngine;

public class CallbackMenuButton : MonoBehaviour
{
    public void BackToMainMenuAfterVictory()
    {
        GameplayUIManager.Instance.ExitToMainMenu();
    }
}
