using UnityEngine;

public class EnemySpawnManger : MonoBehaviour
{
    [SerializeField] private GameObject Slime1Enemy;
    [SerializeField] private GameObject Slime2Enemy;
    [SerializeField] private GameObject Boss1;
    [SerializeField] private Transform enemyFolder;
    private float spawnRadius = 20f;
    void Awake()
    {
        RoundManager.newRoundStarted += EnemySpawn;
    }
    private void EnemySpawn()
    {
        int CurrentRound = GameManager.singleton.roundManager.roundNumber;
        if (CurrentRound % 10 != 0)
        {
            for (int i = 0; i < CurrentRound * 4; i++)
            {
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                float randomDistance = Random.Range(8f, spawnRadius);

                Vector3 offset = new Vector3(randomDirection.x, 0f, randomDirection.y);
                Vector3 spawnPosition = GameManager.singleton.player.transform.position + offset * randomDistance;
                spawnPosition.y = GameManager.singleton.player.transform.position.y;

                GameObject newEnemy = Instantiate(Slime1Enemy, spawnPosition, Quaternion.identity);
                newEnemy.transform.SetParent(enemyFolder);
            }

            if (CurrentRound >= 1)
            {
                for (int i = 0; i < CurrentRound * 3; i++)
                {
                    Vector2 randomDirection = Random.insideUnitCircle.normalized;
                    float randomDistance = Random.Range(30f, 60f);

                    Vector3 offset = new Vector3(randomDirection.x, 0f, randomDirection.y);
                    Vector3 spawnPosition = GameManager.singleton.player.transform.position + offset * randomDistance;
                    spawnPosition.y = GameManager.singleton.player.transform.position.y;

                    GameObject newEnemy = Instantiate(Slime2Enemy, spawnPosition, Quaternion.identity);
                    newEnemy.transform.SetParent(enemyFolder);
                } 
            }
        }
        else
        {
            Vector3 PlayerPos = GameManager.singleton.player.transform.position;
            PlayerPos.z += 10;
            GameObject boss = Instantiate(Boss1, PlayerPos, Quaternion.identity);
            boss.transform.parent = enemyFolder.transform;
        }

    }
}