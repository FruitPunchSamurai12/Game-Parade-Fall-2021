using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Waypoint waypointToTeleportCatAfterItCatchesTheBird;
    public event Action<Checkpoint> onCheckpointTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            onCheckpointTrigger?.Invoke(this);
        }
    }

}
