using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    [SerializeField] Transform[] _waypoints;

    CellTracker _catCellTracker;
    CellTracker _birdCellTracker;

    int _previousWaypointIndex=-1;

    public static Director Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public Vector3 PickRandomPath()
    {
        int random = Random.Range(0, _waypoints.Length);
        if(random==_previousWaypointIndex)
        {
            random++;
            if (random >= _waypoints.Length)
                random = 0;
        }

        _previousWaypointIndex = random;
        return _waypoints[random].position;

    }

    public Vector3 PickInvestigateTarget()
    {
        return _birdCellTracker.closestWaypoint.transform.position;
    }

    public void SetCatAndBird(CellTracker cat,CellTracker bird)
    {
        _catCellTracker = cat;
        _birdCellTracker = bird;
    }
}
