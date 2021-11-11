using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointHandler : MonoBehaviour
{
    public static WaypointHandler instance;

    public Transform[] myWaypoints;
    private void Awake()
    {
        instance = this;
    }
    
}
