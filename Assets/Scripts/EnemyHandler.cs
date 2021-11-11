/*
 * Control of enemy waves and spawn
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    #region Variables
    public bool startWave;

    public GameObject greenEnemy;
    public GameObject blueEnemy;
    public GameObject redEnemy;
    public GameObject spawnPosition;
    int currentWave = 0;
    public int CurrentWave { get { return currentWave; } }
    public string[] enemyWaves;


    List<GameObject> greenEnemyWaveList = new List<GameObject>();
    List<GameObject> redEnemyWaveList = new List<GameObject>();
    List<GameObject> blueEnemyWaveList = new List<GameObject>();

    #endregion
    #region MonoBehaviour
    private void Start()
    {
        GameManager.instance.enemyHandler = this;
     
    }

    void Update()
    {
        if (GameManager.instance.GameOver)
            StopAllCoroutines();
    }
    #endregion
    #region Methods
    /// <summary>
    /// Calls to start the enemy waves
    /// </summary>
    public void StartEnemies()
    {
        StartCoroutine(SpawnEnemies());
    }

    /// <summary>
    /// Uses an array of strings to form the waves that are to be spawned going one by one
    /// </summary>
    /// <returns></returns>
    public IEnumerator SpawnEnemies()
    {
        int currentEnemy = 0;

        for (int i = 0; i < enemyWaves.Length; i++)
        {
            while (currentEnemy < enemyWaves[i].Length)
            {
                switch (enemyWaves[i][currentEnemy])
                {
                    case '1':

                        Spawn(greenEnemyWaveList, greenEnemy);
                        yield return new WaitForSeconds(1f);
                        break;
                    case '2':
                        Spawn(blueEnemyWaveList, blueEnemy);
                        yield return new WaitForSeconds(1f);
                        break;
                    case '3':
                        Spawn(redEnemyWaveList, redEnemy);
                        yield return new WaitForSeconds(1f);
                        break;
                }
                currentEnemy++;
            }

            while (!CheckEnemies())
            {
                yield return new WaitForEndOfFrame();
            }


            currentWave++;
            currentEnemy = 0;
        }
    }
    /// <summary>
    /// Returns a string to show in the UI in which wave is currently being spawned
    /// </summary>
    /// <returns></returns>
    public string WaveCounter()
    {
        return (currentWave + "/" + enemyWaves.Length);
    }

    /// <summary>
    /// Returns true if all enemies are disabled and true if at least one is active
    /// </summary>
    /// <returns></returns>
    bool CheckEnemies()
    {
        for (int i = 0; i < blueEnemyWaveList.Count; i++)
        {
            if (blueEnemyWaveList[i].activeInHierarchy)
                return false;
        }

        for (int i = 0; i < greenEnemyWaveList.Count; i++)
        {
            if (greenEnemyWaveList[i].activeInHierarchy)
                return false;
        }

        for (int i = 0; i < redEnemyWaveList.Count; i++)
        {
            if (redEnemyWaveList[i].activeInHierarchy)
                return false;
        }
        return true;
    }
    /// <summary>
    /// If there is no available enemy of the type it instatiate a new one else it will use an existing copy to start the path
    /// </summary>
    /// <param name="enemyList"></param>
    /// <param name="enemy"></param>
    void Spawn(List<GameObject> enemyList, GameObject enemy)
    {

        for (int i = 0; i < enemyList.Count; i++)
        {
            if (!enemyList[i].activeInHierarchy)
            {
                enemyList[i].transform.position = spawnPosition.transform.position;
                enemyList[i].SetActive(true);
                return;
            }
        }

        enemyList.Add(Instantiate(enemy, spawnPosition.transform.position, Quaternion.identity));
    }
    #endregion
}
