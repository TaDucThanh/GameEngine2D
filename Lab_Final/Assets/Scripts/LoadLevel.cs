using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "Level1Scene")
            {
                Vector3 Level2StartPos = new Vector3(-17f, -3f, 0f);
                PlayerController.Instance.SpawnPos(Level2StartPos);
                GameSceneManager.Instance.LoadLevel("Level2Scene");
            }
        }
       
    }
}
