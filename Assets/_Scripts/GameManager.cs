using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject enemy;
    private TS_Inputs inputs;
    private bool paused;
    private GameObject player;

    [Header("Spawn System")]
    public Transform[] spawnPoints;
    public int activeEnemyCount;
    public int maxEnemyCount;
    public int enemyKills = 0;
    public int upgradeThreshold = 10;
    public float spawnSpeed = 0.5f;
    public float spawnStart = 3f;

    [Header("Panels")]
    public GameObject upgradePanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    void Awake()
    {
        paused = false;
        player = FindAnyObjectByType<PlayerController>().gameObject;
        inputs = new TS_Inputs();
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnStart, spawnSpeed);
    }

    private void Update()
    {
        if (inputs.Player.Pause.IsPressed())
        {
            if (!paused)
            {
                PauseGame();
            }
        }
    }

    private void OnEnable()
    {
        inputs.Enable();
    }
    private void OnDisable()
    {
        inputs.Disable();
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
        paused = true;
        pausePanel.SetActive(true);
        player.SetActive(false);
        Time.timeScale = 0.0f;
    }
    public void UnpauseGame()
    {
        paused = false;
        pausePanel.SetActive(false);
        player.SetActive(true);
        Time.timeScale = 1.0f;
    }
    public void RestartGame()
    {
        paused = false;
        gameOverPanel.SetActive(false);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainGame");
    }
    public void TitleScreen()
    {
        gameOverPanel.SetActive(false);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("TitleScreen");
    }
    public void GameOver()
    {
        paused = true;
        gameOverPanel.SetActive(true);
        player.SetActive(false);
        Time.timeScale = 0.0f;
    }
    private void OpenUpgradePanel()
    {
        paused = true;
        upgradePanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void CloseUpgradePanel()
    {
        paused = false;
        upgradePanel.SetActive(false);
        Time.timeScale = 1.0f;
        enemyKills = 0;
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
