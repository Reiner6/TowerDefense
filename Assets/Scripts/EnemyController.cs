/*
 * Enemy control: life and movement
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Variables
    public float speed = 1f;

    public float smooth = 1f;

    public int maxHealth = 5;
    public int currHealth = 5;

    Vector2 velocity;

    Vector3 beacon;

    Transform[] myWaypoints;

    int currentWaypoint = 0;
    #endregion
    #region MonoBehaviour

    private void OnEnable()
    {
        beacon = this.transform.position;
        currentWaypoint = 0;
        currHealth = maxHealth;
    }

    void Start()
    {
        currHealth = maxHealth;
        beacon = this.transform.position;

        myWaypoints = WaypointHandler.instance.myWaypoints;
    }


    void Update()
    {
        if (currentWaypoint >= myWaypoints.Length)
        {
            GameManager.instance.UpdateEnemies();
            this.gameObject.SetActive(false);
            return;
        }
        MoveBeacon();
        MoveEnemy();
    }
    #endregion

    #region Methods

    /// <summary>
    /// Moves the reference the enemy follows
    /// </summary>
    void MoveBeacon()
    {
        beacon = Vector2.MoveTowards(beacon, myWaypoints[currentWaypoint].position, speed * Time.deltaTime);
        if (Vector2.Distance(beacon, myWaypoints[currentWaypoint].position) <= 0.1f)
        {
            currentWaypoint++;
        }
    }
    /// <summary>
    /// Moves the enemy with a smoothing following the beacon
    /// </summary>
    void MoveEnemy()
    {
        this.transform.position = Vector2.SmoothDamp(this.transform.position, beacon, ref velocity, smooth, speed);

        Vector3 newDirection = (beacon - this.transform.position).normalized;


        Vector3 rotation = Quaternion.LookRotation(newDirection).eulerAngles;
        rotation.z = rotation.x;
        rotation.y = 0.0f;
        rotation.x = 0.0f;
        this.transform.eulerAngles = rotation;
    }

    /// <summary>
    /// Called by the bullet on collision to inflict damage
    /// </summary>
    /// <param name="damage"></param>
    public void UpdateHealth(int damage)
    {
        currHealth -= damage;

        if (currHealth <= 0)
        {
            this.gameObject.SetActive(false);
            currentWaypoint = 0;
            GameManager.instance.UpdateDeathEnemies();
        }
    }
    #endregion
}