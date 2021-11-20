using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] Waypoint _frontAreaCatTeleportWaypoint;
    [SerializeField] Waypoint _backAreaCatTeleportWaypoint;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Vector3 playerDir = transform.position - other.transform.position;
            float dot = Vector3.Dot(transform.forward, playerDir);
            if (dot > 0)
                Director.Instance.PlayerChangedArea(_backAreaCatTeleportWaypoint);
            else
                Director.Instance.PlayerChangedArea(_frontAreaCatTeleportWaypoint);
        }
    }
}
