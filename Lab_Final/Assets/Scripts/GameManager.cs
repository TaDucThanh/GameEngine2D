using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject[] enemies;
    private GameObject[] bosses;

    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        bosses = GameObject.FindGameObjectsWithTag("Boss");
    }

    void Update()
    {
        bool anyEnemyAlive = false;

        foreach (var enemy in enemies)
        {
            if (enemy != null) 
            {
                anyEnemyAlive = true;
                break;
            }
        }

        bool anyBossAlive = false;

        foreach (var boss in bosses)
        {
            if (boss != null)
            {
                anyBossAlive = true;
                break;
            }
        }

        if (!anyEnemyAlive && !anyBossAlive)
        {
            Destroy(PlayerController.Instance);
            Destroy(gameObject);
            Destroy(GameplayUIManager.Instance);
            SceneManager.LoadScene("VictoryScene");
        }
    }
}
