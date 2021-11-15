using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [SerializeField] Transform[] _waypoints;

    int _previousWaypointIndex=-1;

    public static WaypointManager Instance { get; private set; }

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
}
