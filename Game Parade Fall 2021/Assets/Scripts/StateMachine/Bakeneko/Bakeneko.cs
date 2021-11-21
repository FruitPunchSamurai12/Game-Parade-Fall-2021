using System;
using UnityEngine;

public class Bakeneko:MonoBehaviour
{
    [SerializeField] float _chaseMoveSpeed = 10f;
    [SerializeField] float _neutralMoveSpeed = 6f;
    [SerializeField] float _catchDistance = 1f;
    [SerializeField] float _reactionTime = .75f;
    [SerializeField] float _investigationTime = 10f;
    [SerializeField] float _investigationRadius = 7.5f;
    [SerializeField] float _predictionTime = 0.75f;
    [SerializeField] float _sightRange = 10f;
    [SerializeField] float _sightAngle = 45f;
    [SerializeField] float _lookRotationSpeed = 300f;
    [SerializeField] float _feelRadius = 5f;
    [SerializeField] LayerMask _obstaclesLayer;
    [SerializeField] Transform _eyes;

    [SerializeField] float _playfulnessRatio = 0.75f;
    [SerializeField] float _playfulness = 1;
    [SerializeField][Range(0.1f,0.9f)] float _playfulnessAmbushThreshold = 0.25f;


    public float NeutralSpeed => _neutralMoveSpeed;
    public float ChaseSpeed => _chaseMoveSpeed;
    public float CatchDistance => _catchDistance;
    public float ReactionTime => _reactionTime;
    public float InvestigationTime => _investigationTime;
    public float LookRotatioNSpeed => _lookRotationSpeed;
    public bool WantsToAmbust { get; private set; }
    public float InvestigationRadius => _investigationRadius;

    Transform _playerTranform;
    float _sightRangeSqr;
    float _feelRadiusSqr;
    bool _hasPlayerInSight = false;
    private void Awake()
    {
        _sightRangeSqr = _sightRange * _sightRange;
        _feelRadiusSqr = _feelRadius * _feelRadius;
    }

    private void Start()
    {
        Director.Instance.onBirdInPinchPoint += TryToAmbush;
    }


    public bool CanSee()
    {
        if (CanSeePlayer())
            return true;
        if (CanSeeMarks())
            return true;
        return false;
    }

    public bool CanSeeMarks()
    {
        foreach (var posBool in Director.Instance.MarksPositionsWithCatInvestigationBool)
        {
            if (posBool.Value == true)
                continue;
            if (CanSee(posBool.Key))
            {
                Director.Instance.MarksPositionsWithCatInvestigationBool[posBool.Key] = true;
                return true;
            }
        }
        return false;
    }

    public bool CanSeePlayer()
    {
        _playerTranform = GameManager.Instance.PlayerTransform;
        if (_playerTranform == null)
            return false;
        if(CanSee(_playerTranform.position))
        {
            _hasPlayerInSight = true;
            return true;
        }
        if(_hasPlayerInSight)
        {
            _hasPlayerInSight = false;
            Director.Instance.LostBirdLineOfSight(_predictionTime);
        }
        return false;
    }

    bool CanSee(Vector3 target)
    {
        Vector3 targetPosition = target;
        Vector3 targetDir = targetPosition.FlatVector() - transform.position.FlatVector();
        float angle = Vector3.Angle(targetDir.FlatVector(), _eyes.transform.forward.FlatVector());
        float distanceSqr = targetPosition.FlatVectorDistanceSquared(_eyes.transform.position);
        if (distanceSqr < _sightRangeSqr && angle < _sightAngle)
        {
            RaycastHit hit;
            float distance = Mathf.Sqrt(distanceSqr);
            Physics.Raycast(_eyes.position, targetDir.normalized, out hit, distance, _obstaclesLayer);
            if (hit.collider == null)
            {
                Director.Instance.CatSawSomething(targetPosition);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
            return false;
    }

    public bool CanFeel()
    {
        _playerTranform = GameManager.Instance.PlayerTransform;
        if (_playerTranform == null)
            return false;
        float distanceSqr = _playerTranform.position.FlatVectorDistanceSquared(transform.position);
        if(distanceSqr<_feelRadiusSqr)
        {
            Director.Instance.CatSawSomething(_playerTranform.position);
            return true;
        }
        return false;
    }

    void TryToAmbush()
    {
        if (_playfulness > _playfulnessAmbushThreshold)
            WantsToAmbust = true;
        else
            WantsToAmbust = false;
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Color color = new Color(0, 0, 1, 0.25f);
        UnityEditor.Handles.color = color;
        UnityEditor.Handles.DrawSolidArc(_eyes.transform.position, Vector3.up, Quaternion.Euler(0, -_sightAngle, 0) * transform.forward, _sightAngle * 2f, _sightRange);
        color = new Color(0, 1, 0, 0.25f);
        UnityEditor.Handles.color = color;
        UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.up, _feelRadius);
        color = new Color(1, 0, 0, 0.1f);
        UnityEditor.Handles.color = color;
        UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.up, _investigationRadius);
    }
#endif
}
