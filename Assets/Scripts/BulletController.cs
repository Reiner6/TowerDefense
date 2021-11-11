/*
 * Bullet control
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    #region Variables
    Rigidbody2D myRigidbody;

    GameObject myTarget;

    public float angularSpeed = 250f;
    public float linearSpeed = 6f;
    readonly int damage = 1;
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MissileBehaviour();
    }
    /*
     * Disables bullet when out of sight by camera
     */
    private void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyController>())
        {
            collision.GetComponent<EnemyController>().UpdateHealth(damage);
            this.gameObject.SetActive(false);
        }
    }

    #endregion

    #region Methods
    /// <summary>
    /// If a target is assigned the bullet becomes a homming projectile that follows its target until collision or the target is detroyed
    /// </summary>
    void MissileBehaviour()
    {
        if (myTarget != null)
        {
            Vector2 direction = myTarget.transform.position - this.transform.position;

            float rotateAmount = Vector3.Cross(direction.normalized, transform.up).z;

            myRigidbody.angularVelocity = -rotateAmount * angularSpeed;

            myRigidbody.velocity = transform.up * linearSpeed;

            if (!myTarget.activeInHierarchy)
                myTarget = null;
        }
    }
    /// <summary>
    /// Set direction and speed to the bullet
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="bulletSpeed"></param>

    public void Velocity(Vector2 direction, float bulletSpeed)
    {
        myRigidbody.velocity = direction * bulletSpeed;
    }
    /// <summary>
    /// Starts using the missile behaviour if there is an assigned target
    /// </summary>
    /// <param name="_target"></param>
    /// <param name="bulletSpeed"></param>
    public void Missile(GameObject _target, float bulletSpeed)
    {
        myTarget = _target;
        linearSpeed = bulletSpeed;
    }

    #endregion

    #region GetSet

    public Rigidbody2D MyRigidbody { get { return myRigidbody; } }
    #endregion
}