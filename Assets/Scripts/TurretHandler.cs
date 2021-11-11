/*
 * Turret handler manages the spawning of new turrets
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHandler : MonoBehaviour
{
    #region Variables
    public GameObject turretPrefab;
    List<GameObject> turretsList = new List<GameObject>();
    bool building = false;
    ShootType myShootType;

    public LayerMask ignoreMask;

    public TurretData automaticData;
    public TurretData shotgunData;
    public TurretData missileData;

    public GameObject turretReference;

    Vector3 turretReferencePosition;
    #endregion
    #region MonoBehaviour
    private void Start()
    {
        GameManager.instance.turretHandler = this;
        turretReference.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.instance.GameOver)
            return;
        if (building)
        {
            // Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            turretReferencePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            turretReferencePosition.z = 0;
            turretReference.transform.position = turretReferencePosition;

            if (Input.GetMouseButtonDown(0))
            {
                SpawnTower();

            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                building = false;

                turretReference.SetActive(false);
            }
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Checks if a turret can be spawned on the selecte positon and alignst it in the grid, as well as check if there is an exisitng turret in that position.
    /// </summary>
    void SpawnTower()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (worldPosition.y > Mathf.RoundToInt(worldPosition.y))
        {
            worldPosition.y = Mathf.RoundToInt(worldPosition.y) + 0.5f;
        }
        else if (worldPosition.y < Mathf.RoundToInt(worldPosition.y))
        {
            worldPosition.y = Mathf.RoundToInt(worldPosition.y) - 0.5f;
        }

        if (worldPosition.x > Mathf.RoundToInt(worldPosition.x))
        {
            worldPosition.x = Mathf.RoundToInt(worldPosition.x) + 0.5f;
        }
        else if (worldPosition.x < Mathf.RoundToInt(worldPosition.x))
        {
            worldPosition.x = Mathf.RoundToInt(worldPosition.x) - 0.5f;
        }

        worldPosition.z = 0;

        for (int i = 0; i < turretsList.Count; i++)
        {
            if (turretsList[i].transform.position == worldPosition)
                return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit2D check = Physics2D.Raycast(ray.origin, ray.direction, 100, ignoreMask);

        if (check.collider != null)
        {
            return;
        }

        GameObject go = Instantiate(turretPrefab, worldPosition, Quaternion.identity, this.transform);

        TurretController temp = go.GetComponent<TurretController>();
        temp.myType = myShootType;

        switch (temp.myType)
        {
            case ShootType.automatic:
                temp.myTurretData = automaticData;
                temp.body.color = automaticData.turretColor;
                break;
            case ShootType.shotgun:
                temp.myTurretData = shotgunData;
                temp.body.color = shotgunData.turretColor;
                break;
            case ShootType.missile:
                temp.myTurretData = missileData;
                temp.body.color = missileData.turretColor;
                break;
            default:
                break;
        }

        turretsList.Add(go);

        building = false;

        turretReference.SetActive(false);
        GameManager.instance.AvailableTurrets--;
    }

    /// <summary>
    /// Select the type of turret to build
    /// 1 = automatic
    /// 2 = shotgun
    /// 3 = missile
    /// </summary>
    /// <param name="i"></param>
    public void SelectTurret(int i)
    {
        if (building)
            return;

        building = true;

        myShootType = (ShootType)i;
        switch (myShootType)
        {
            case ShootType.automatic:
                turretReference.transform.localScale = Vector3.one * automaticData.radius;
                break;
            case ShootType.shotgun:
                turretReference.transform.localScale = Vector3.one * shotgunData.radius;
                break;
            case ShootType.missile:
                turretReference.transform.localScale = Vector3.one * missileData.radius;
                break;
            default:
                break;
        }

        turretReference.SetActive(true);
    }
    #endregion

    #region GetSet

    public bool IsBuilding { get { return building; } }
    #endregion


}