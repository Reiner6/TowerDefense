/*
 * Turret behaviour controller
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    #region Variables
    public ShootType myType;

    List<Transform> enemyList = new List<Transform>();

    public float speed = 1f;

    public GameObject bullet;

    List<BulletController> myBullets = new List<BulletController>();

    public TurretData myTurretData;
    public GameObject radius;
    public SpriteRenderer body;

    CircleCollider2D CircleCollider2D;

    float timer;

    float angleTarget;
    Vector3 vectorToTarget;
    #endregion

    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }
    
    void Update()
    {
        if (enemyList.Count == 0)
        {
            return;
        }

        Rotate(enemyList[0].position);
        Shoot();

        if (!enemyList[0].gameObject.activeInHierarchy)
            enemyList.Remove(enemyList[0]);
    }

    /*
     * Updates the enemies that enters the radius of the turret
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
            if (!enemyList.Contains(other.transform))
                enemyList.Add(other.transform);
    }

    /*
     * Updates the enemies that exits the radius of the turret
     */
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
            if (enemyList.Contains(other.transform))
                enemyList.Remove(other.transform);
    }
    #endregion

    #region Methods
    /// <summary>
    ///  Start setup of the turret
    /// </summary>
    void Setup()
    {
        CircleCollider2D = GetComponent<CircleCollider2D>();

        CircleCollider2D.radius = myTurretData.radius;

        radius.gameObject.SetActive(true);
        radius.transform.localScale = Vector3.one * CircleCollider2D.radius * 2f;
    }

    /// <summary>
    ///  Rotates the turret to face its target
    /// </summary>
    void Rotate(Vector3 target)
    {

        vectorToTarget = target - transform.position;
        angleTarget = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angleTarget, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, Time.deltaTime * speed);

    }

    /// <summary>
    ///  Calls to fire depending on the type of turret
    /// </summary>
    void Shoot()
    {
        timer += Time.deltaTime;
        switch (myType)
        {
            case ShootType.automatic:
                AutomaticFire();
                break;
            case ShootType.shotgun:
                ShotgunFire();
                break;
            case ShootType.missile:
                MissileFire();
                break;
            default:
                break;
        }
    }

    /// <summary>
    ///  Checks if there is available bullets to use or it has to create new ones
    /// </summary>
    bool CheckForAmmo(int value)
    {
        if (myBullets.Count < 0)
            return false;

        for (int i = 0; i < myBullets.Count; i++)
        {
            if (!myBullets[i].gameObject.activeInHierarchy)
            {
                value--;
                if (value == 0)
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    ///  Shoots a single bullet everytime is called
    /// </summary>
    void AutomaticFire()
    {
        if (timer >= myTurretData.fireTime)
        {
            timer = 0;
            if (!CheckForAmmo(1))
            {
                GameObject go = Instantiate(bullet, this.transform.position, Quaternion.identity);

                BulletController tempBullet = go.GetComponent<BulletController>();
                tempBullet.Velocity(vectorToTarget, myTurretData.bulletSpeed);

                myBullets.Add(tempBullet);
            }
            else
            {
                for (int i = 0; i < myBullets.Count; i++)
                {
                    if (!myBullets[i].gameObject.activeInHierarchy)
                    {
                        myBullets[i].gameObject.SetActive(true);
                        myBullets[i].transform.position = this.transform.position;
                        myBullets[i].Velocity(vectorToTarget, myTurretData.bulletSpeed);
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    ///  Shoots a 3 bullets everytime is called
    /// </summary>
    void ShotgunFire()
    {
        if (timer >= myTurretData.fireTime)
        {
            timer = 0;
            if (!CheckForAmmo(3))
            {

                for (float i = -1; i < 2; i++)
                {
                    GameObject go = Instantiate(bullet, this.transform.position, Quaternion.identity);

                    BulletController tempBullet = go.GetComponent<BulletController>();

                    tempBullet.Velocity(Quaternion.Euler(0, 0, (i * 10)) * vectorToTarget, myTurretData.bulletSpeed);

                    myBullets.Add(tempBullet);
                }

            }
            else
            {
                float temp = -1;
                for (int i = 0; i < myBullets.Count; i++)
                {
                    if (!myBullets[i].gameObject.activeInHierarchy)
                    {
                        myBullets[i].gameObject.SetActive(true);
                        myBullets[i].transform.position = this.transform.position;


                        myBullets[i].Velocity(Quaternion.Euler(0, 0, (temp * 10)) * vectorToTarget, myTurretData.bulletSpeed);
                        temp++;
                        if (temp > 1)
                        {
                            return;
                        }
                    }
                }
            }
        }
    }
    
    /// <summary>
    ///  Shoots a homming bullet everytime is called
    /// </summary>
    void MissileFire()
    {
        if (timer >= myTurretData.fireTime)
        {
            Vector3 myForward = this.transform.eulerAngles;
            myForward.z -= 90f;
            timer = 0;
            if (!CheckForAmmo(1))
            {
                GameObject go = Instantiate(bullet, this.transform.position, Quaternion.Euler(myForward));

                BulletController tempBullet = go.GetComponent<BulletController>();
                tempBullet.Missile(enemyList[0].gameObject, myTurretData.bulletSpeed);

                myBullets.Add(tempBullet);
            }
            else
            {
                for (int i = 0; i < myBullets.Count; i++)
                {
                    if (!myBullets[i].gameObject.activeInHierarchy)
                    {
                        myBullets[i].gameObject.SetActive(true);
                        myBullets[i].transform.position = this.transform.position;
                        myBullets[i].transform.eulerAngles = myForward;
                        myBullets[i].Missile(enemyList[0].gameObject, myTurretData.bulletSpeed);
                        return;
                    }
                }
            }

        }
    }
    #endregion
}