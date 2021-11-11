/*
 * The game manager controls the win and lose conditions of the game as well as coordinate the different handlers
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager instance;
    public int startingTurrets = 3;
    public int availableTurrets;
    public float timeToStartWave = 30f;
    public GameObject spawnTurretButtons;
    public GameObject gameoverObjects;
    public GameObject victoryObjects;
    public Text scoreText;
    public Text availableTurretsText;
    public Text waveText;
    public Text livesText;
    public Text startTimer;
    int enemyCounter = 0;
    public int enemyCountToLose = 3;
    bool gameover;
    int deathCounter = 0;
    public int deadsToNewTurret = 5;
    public EnemyHandler enemyHandler;
    public TurretHandler turretHandler;
    #endregion
    #region MonoBehaviour
    private void Awake()
    {
        instance = this;
        availableTurrets = startingTurrets;

        gameoverObjects.SetActive(false);
    }

    private IEnumerator Start()
    {
        float timer = 0;
        while (timer < timeToStartWave)
        {
            timer += Time.deltaTime;

            startTimer.text = Mathf.CeilToInt(timeToStartWave - timer).ToString();

            yield return new WaitForEndOfFrame();
        }
        startTimer.gameObject.SetActive(false);
        enemyHandler.StartEnemies();
    }

    void Update()
    {
        UpdateUI();

        spawnTurretButtons.SetActive(!turretHandler.IsBuilding && availableTurrets > 0);

        CheckLoseCondition();
        CheckWindcondition();
    }
    #endregion
    #region Method
    void CheckLoseCondition()
    {
        if (enemyCounter >= enemyCountToLose)
        {
            gameover = true;
            gameoverObjects.SetActive(true);
            spawnTurretButtons.SetActive(false);
            this.enabled = false;
        }
    }

    void CheckWindcondition()
    {
        if (gameover)
            return;
        if (enemyHandler.CurrentWave >= enemyHandler.enemyWaves.Length)
        {
            gameover = true;
            victoryObjects.SetActive(true);
            spawnTurretButtons.SetActive(false);
            this.enabled = false;
        }
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + deathCounter.ToString();
        availableTurretsText.text = "Available turrets: " + availableTurrets.ToString();
        waveText.text = "Wave: " + enemyHandler.WaveCounter();
        livesText.text = "Lives: " + (enemyCounter + "/" + enemyCountToLose);

    }

    public void UpdateEnemies()
    {
        enemyCounter++;
    }

    public void UpdateDeathEnemies()
    {
        deathCounter++;

        if (deathCounter % deadsToNewTurret == 0)
        {
            availableTurrets++;
        }
    }

    public void Reload()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(this.gameObject.scene.buildIndex);
    }
    #endregion

    #region GetSet
    public int AvailableTurrets { get { return availableTurrets; } set { availableTurrets = value; } }
    public bool GameOver { get { return gameover; } }
    #endregion
}