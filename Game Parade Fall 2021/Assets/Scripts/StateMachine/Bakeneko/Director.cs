using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Director : MonoBehaviour
{
    [SerializeField] Transform[] _waypoints;
    [SerializeField] WaypointManager waypointManager;

    CellTracker _catCellTracker;
    CellTracker _birdCellTracker;

    int _previousWaypointIndex=-1;

    public Vector3 LastInterestingLocation { get; private set; }
    public Dictionary<Vector3, bool> MarksPositionsWithCatInvestigationBool { get; private set; }//i have no clue what to name this

   

    public event Action onBirdInPinchPoint;

    public static Director Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            MarksPositionsWithCatInvestigationBool = new Dictionary<Vector3, bool>();
            Instance = this;        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if(_birdCellTracker!=null)
        {
            waypointManager.waypointEvaluation.IsWaypointAPinchPoint(_birdCellTracker.closestWaypointIndex);
            onBirdInPinchPoint?.Invoke();
        }
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

    public void NewMarkedPlaced(Vector3 position)
    {
        if(MarksPositionsWithCatInvestigationBool.ContainsKey(position))
        {
            MarksPositionsWithCatInvestigationBool[position] = false;
        }
        else
        {
            MarksPositionsWithCatInvestigationBool.Add(position, false);
        }
    }

    public Vector3 PickInvestigateTarget()
    {
        return _birdCellTracker.closestWaypoint.transform.position;
    }

    public void CatSawSomething(Vector3 location)
    {
        LastInterestingLocation = location;
    }

    public void SetCatAndBird(CellTracker cat,CellTracker bird)
    {
        _catCellTracker = cat;
        _birdCellTracker = bird;
        
    }


    public bool GetAmbushAndLookPoint(out Vector3 ambushPosition, out Vector3 lookAtPosition)
    {
        ambushPosition = new Vector3();
        lookAtPosition = new Vector3();
        if (_birdCellTracker == null || _catCellTracker == null) return false;
        var pp = GetAmbushPinchPoint();
        if (pp == null) return false;
        lookAtPosition = waypointManager.waypoints[pp.OutsideID].transform.position;
        float minDistance = float.MaxValue;
        foreach (var w in pp.ambushPointIDs)
        {
            Vector3 wPos = waypointManager.waypoints[w].transform.position;
            float distance = transform.position.FlatVectorDistanceSquared(wPos);
            if (distance < minDistance)
            {
                ambushPosition = wPos;
                minDistance = distance;
            }
        }
        return true;

    }

    PinchPoint GetAmbushPinchPoint()
    {
        if (_birdCellTracker == null) return null;
        var pp = waypointManager.waypointEvaluation.IsWaypointAPinchPoint(_birdCellTracker.closestWaypointIndex);
        return pp;
    }
}
