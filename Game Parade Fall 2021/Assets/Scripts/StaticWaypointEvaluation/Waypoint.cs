using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour,IComparable<Waypoint>
{
    public int ID;
    public int[] linksToOtherWaypoints;
    public int exposure;
    private void OnValidate()
    {
        name = "W" + ID;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, 1);
        UnityEditor.Handles.Label(transform.position, ID.ToString());
    }
#endif
    public int CompareTo(Waypoint wp)
    {
        return ID.CompareTo(wp.ID);
    }
}