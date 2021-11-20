using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatrolRoute
{
    public List<Waypoint> _routeWaypoints = new List<Waypoint>();

#if UNITY_EDITOR
    public void DebugDraw()
    {
        for (int i = 0; i < _routeWaypoints.Count-1; i++)
        {

            Gizmos.DrawLine(_routeWaypoints[i].transform.position, _routeWaypoints[i + 1].transform.position);
        }
    }
#endif
}
