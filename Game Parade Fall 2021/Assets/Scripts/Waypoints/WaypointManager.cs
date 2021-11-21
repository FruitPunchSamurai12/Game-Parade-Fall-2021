using UnityEngine;

public class WaypointManager:MonoBehaviour
{
    [SerializeField] MazeArea[] _mazeAreas = new MazeArea[9];


    public static WaypointManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public PatrolRoute GetRandomRoute(int catAreaID)
    {
        if (catAreaID < 0 || catAreaID >= _mazeAreas.Length) return null;
        var routes = _mazeAreas[catAreaID]._areaPatrolRoutes;
        return routes[Random.Range(0, routes.Count)];
    }

    public Vector3 GetInvestigateWaypoint(int playerAreaID,Vector3 playerPos)
    {
        if (playerAreaID < 0 || playerAreaID >= _mazeAreas.Length) return Vector3.zero;
        var waypoints = _mazeAreas[playerAreaID]._investigateWaypoints;
        float minDistance = float.MaxValue;
        Vector3 investigatePos = new Vector3();
        foreach (var w in waypoints)
        {
            float distance = w.transform.position.FlatVectorDistanceSquared(playerPos);
            if(distance<minDistance)
            {
                minDistance = distance;
                investigatePos = w.transform.position;
            }
        }
        return investigatePos;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        foreach (var area in _mazeAreas)
        {
            area.DebugDraw();

        }
    }
#endif
}