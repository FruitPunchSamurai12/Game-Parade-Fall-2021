using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    [SerializeField] Transform[] _waypoints;

    CellTracker _catCellTracker;
    CellTracker _birdCellTracker;

    int _previousWaypointIndex=-1;

    public Vector3 LastInterestingLocation { get; private set; }
    public Dictionary<Vector3, bool> MarksPositionsWithCatInvestigationBool { get; private set; }//i have no clue what to name this

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
}
