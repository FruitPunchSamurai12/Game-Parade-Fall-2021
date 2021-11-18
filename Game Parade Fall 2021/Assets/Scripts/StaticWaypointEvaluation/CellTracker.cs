using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTracker : MonoBehaviour
{
    public WaypointManager waypointManager {get; private set;}
    public int cellIndex {get; private set;}
    public int closestWaypointIndex{get; private set;}
    public Waypoint closestWaypoint => waypointManager.waypoints[closestWaypointIndex];

    public float updateTime = 0.5f;
    float updateTimer = 0;
    private void Start()
    {
        waypointManager = FindObjectOfType<WaypointManager>();
        FindCurrentCell();
    }

    public void FindCurrentCell()
    {
        foreach (var cell in SpatialPartition.Instance.cells)
        {
            if (cell.cellBounds.Contains(transform.position))
            {
                cellIndex = cell.index;
                break;
            }
        }
        closestWaypointIndex = FindClosestWaypoint().ID;
    }

    public void Teleport(Vector3 position)
    {
        transform.position = position;
        cellIndex = SpatialPartition.Instance.GetCellIndexFromPosition(position);
        closestWaypointIndex = FindClosestWaypoint().ID;
    }

    private void Update()
    {
        updateTimer += Time.deltaTime;
        if (updateTimer > updateTime)
        {
            if (!SpatialPartition.Instance.cells[cellIndex].cellBounds.Contains(transform.position))
            {
                for (int i = 0; i < SpatialPartition.Instance.cells[cellIndex].neighboors.Count; i++)
                {
                    if (SpatialPartition.Instance.cells[SpatialPartition.Instance.cells[cellIndex].neighboors[i]].cellBounds.Contains(transform.position))
                    {
                        cellIndex = SpatialPartition.Instance.cells[cellIndex].neighboors[i];
                        break;
                    }
                }
            }
            closestWaypointIndex = FindClosestWaypoint().ID;
            updateTimer = 0;
        }
    }

    public Waypoint FindClosestWaypoint()
    {
        float minMagnitudeSqr = float.MaxValue;
        List<Waypoint> waypoints = waypointManager.waypoints;
        Waypoint closest = null;
        foreach (int wpIndex in FindAllCloseWaypoints())
        {
            float magnitudeSqr = Vector3.SqrMagnitude(waypoints[wpIndex].transform.position - transform.position);
            if(magnitudeSqr< minMagnitudeSqr)
            {
                minMagnitudeSqr = magnitudeSqr;
                closest = waypoints[wpIndex];
            }
        }
        return closest;
    }


    public List<int> FindAllCloseWaypoints()
    {
        return SpatialPartition.Instance.GetCellAndNeighboorWaypoints(cellIndex);
    }
}
