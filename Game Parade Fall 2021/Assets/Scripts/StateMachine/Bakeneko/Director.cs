using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class Director : MonoBehaviour
{ 
    GameObject _cat;
    GameObject _bird;
    PlayerMovement _birdMovement;

    [SerializeField] float _teleportCooldown = 10f;
    [SerializeField] float _catCloseToBirdTimeThreshold = 7.5f;

    public float _teleportCooldownTimer = 1000;
    public float _catCloseToBirdTimer = 1000;
    bool _catInRange = false;

    int _currentCatAreaID;
    int _currentBirdAreaID;

    public Vector3 LastInterestingLocation { get; private set; }
    public Vector3 BirdPosition => _bird.transform.position;
    public Dictionary<Vector3, bool> MarksPositionsWithCatInvestigationBool { get; private set; }//i have no clue what to name this

    public event Action onBirdInPinchPoint;//not used
    public event Action onBirdSwitchedAreas;

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

    private void Start()
    {
        CameraEffects.onCatWithinRange += HandleCatCloseToBird;
        CameraEffects.onCatOutOfRange += HandleCatAwayFromBird;
    }

    public void HandleCatCloseToBird()
    {
        _catCloseToBirdTimer = 0;
        _catInRange = true;
    }

    public void HandleCatAwayFromBird()
    {
        _catInRange = false;
    }

    private void OnDestroy()
    {
        CameraEffects.onCatWithinRange -= HandleCatCloseToBird;
        CameraEffects.onCatOutOfRange -= HandleCatAwayFromBird;
    }


    private void Update()
    {
        if(!_catInRange)
        {
            _catCloseToBirdTimer += Time.deltaTime;
        }
        _teleportCooldownTimer += Time.deltaTime;
    }

    public PatrolRoute PickRandomRoute()
    {
        return WaypointManager.Instance.GetRandomRoute(_currentBirdAreaID);
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
       return WaypointManager.Instance.GetInvestigateWaypoint(_currentBirdAreaID,_bird.transform.position);
    }

    public void CatSawSomething(Vector3 location)
    {
        LastInterestingLocation = location;
    }

    public void LostBirdLineOfSight(float predictionTime)
    {
        LastInterestingLocation = _bird.transform.position + _birdMovement.Direction * _birdMovement.Speed * predictionTime;
    }

    public void PlayerReset(int birdAreaID,int catAreaID)
    {
        _currentBirdAreaID = birdAreaID;
        _currentCatAreaID = catAreaID;
        _teleportCooldownTimer = 0;
        _catCloseToBirdTimer = 0;
        _cat.GetComponent<AIStateMachine>().ResetStateMachine(WaypointManager.Instance.GetRandomRoute(_currentCatAreaID));
    }

    public void PlayerChangedArea(int newAreaID, PatrolRoute catPatrolRoute)
    {
        _currentBirdAreaID = newAreaID;
        if (_teleportCooldownTimer >= _teleportCooldown && _catCloseToBirdTimer >= _catCloseToBirdTimeThreshold)
        {
            _teleportCooldownTimer = 0;
            _catCloseToBirdTimer = 0;
            _currentCatAreaID = newAreaID;
            _cat.GetComponent<AIStateMachine>().ResetStateMachine(catPatrolRoute);
        }
    }


    public void SetCatAndBird(GameObject cat, GameObject bird)
    {
        _cat = cat;
        _bird = bird;
        _birdMovement = _bird.GetComponent<PlayerMovement>();
    }


    /*
        public bool GetAmbushAndLookPoint(out Vector3 ambushPosition, out Vector3 lookAtPosition)
        {
            ambushPosition = new Vector3();
            lookAtPosition = new Vector3();
            if (_bird == null || _cat == null) return false;
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
            return null;
            if (_bird == null) return null;
            var pp = waypointManager.waypointEvaluation.IsWaypointAPinchPoint(_bird.closestWaypointIndex);
            return pp;
        }*/
}

