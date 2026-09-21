using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject enemy;

    [Header("Spawn System")]
    public Transform[] spawnPoints;
    public int activeEnemyCount;
    public int maxEnemyCount;
    public int enemyKills = 0;
    public int upgradeThreshold = 5;
    public float spawnSpeed = 2f;
    public float spawnStart = 3f;

    [Header("Panels")]
    public GameObject upgradePanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnStart, spawnSpeed);
    }

    public void SpawnEnemy()
    {
        if (activeEnemyCount >= maxEnemyCount)
            return;
        else
        {
            activeEnemyCount++;
            int spawnChoice = Random.Range(0, spawnPoints.Length); //Random.Range is mininum inclusive and maximum exclusive, no need for a -1
            Instantiate(enemy, spawnPoints[spawnChoice].position, spawnPoints[spawnChoice].rotation);
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0.0f;
    }
    public void UnpauseGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    private void OpenUpgradePanel()
    {
        upgradePanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void CloseUpgradePanel()
    {
        upgradePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void EnemyKillCount()
    {
        enemyKills++;
        if (enemyKills >= upgradeThreshold)
        {
            enemyKills = 0;
            OpenUpgradePanel();
        }
    }
}
