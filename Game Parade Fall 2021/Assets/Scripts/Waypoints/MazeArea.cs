using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class MazeArea
{
    public int ID;

    public List<PatrolRoute> _areaPatrolRoutes = new List<PatrolRoute>();
    public List<Waypoint> _investigateWaypoints = new List<Waypoint>();


#if UNITY_EDITOR
    public void DebugDraw()
    {
        foreach (var pr in _areaPatrolRoutes)
        {
            Gizmos.color = Color.green;
            pr.DebugDraw();
        }
        foreach (var wp in _investigateWaypoints)
        {
            Handles.color = Color.magenta;
            Handles.DrawSolidDisc(wp.transform.position, Vector3.up, 1);
        }
    }
#endif

}