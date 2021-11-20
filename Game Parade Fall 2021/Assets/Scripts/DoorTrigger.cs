using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] int _frontAreaID;
    [SerializeField] PatrolRoute _frontAreaCatPatrolRoute = new PatrolRoute();
    [SerializeField] int _backAreaID;
    [SerializeField] PatrolRoute _backAreaCatPatrolRoute = new PatrolRoute();

    BoxCollider _col;

    private void OnValidate()
    {
        _col = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("passed");
            Vector3 playerDir = transform.position - other.transform.position;
            float dot = Vector3.Dot(transform.forward, playerDir);
            if (dot > 0)
                Director.Instance.PlayerChangedArea(_backAreaID,_backAreaCatPatrolRoute);
            else
                Director.Instance.PlayerChangedArea(_frontAreaID,_frontAreaCatPatrolRoute);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.DrawWireCube(transform.position, transform.rotation*_col.size);
        if (_frontAreaCatPatrolRoute._routeWaypoints.Count > 0)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, _frontAreaCatPatrolRoute._routeWaypoints[0].transform.position);
            Gizmos.color = Color.green;
            _frontAreaCatPatrolRoute.DebugDraw();
        }
        if (_backAreaCatPatrolRoute._routeWaypoints.Count>0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _backAreaCatPatrolRoute._routeWaypoints[0].transform.position);
            Gizmos.color = Color.green;
            _backAreaCatPatrolRoute.DebugDraw();
        }
    }
#endif
}
