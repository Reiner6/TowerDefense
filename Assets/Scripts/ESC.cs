/*
 * Enums, Structs and Classes
 */
using System;
using UnityEngine;

/// <summary>
/// Turret type
/// </summary>
public enum ShootType
{
    automatic,
    shotgun,
    missile
}

/// <summary>
/// Turret information for its behaviour
/// </summary>
[Serializable]
public struct TurretData
{
    public Color turretColor;
    public float fireTime ;
    public float radius ;
    public float bulletSpeed ;

}