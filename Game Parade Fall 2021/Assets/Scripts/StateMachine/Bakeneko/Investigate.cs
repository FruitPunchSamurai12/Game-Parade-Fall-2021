using UnityEngine;
using UnityEngine.AI;

public class Investigate : IState
{
    NavMeshAgent _agent;
    float _moveSpeed;
    Vector3 _target;
    float _stoppingDistanceSqr;
    float _timeLimit;
    float _timer;
    float _wanderRadius;
    bool _gotInvestigatePoint;

    public Investigate(NavMeshAgent agent, float moveSpeed,float timeLimit,float wanderRadius)
    {
        _agent = agent;
        _wanderRadius = wanderRadius;
        _moveSpeed = moveSpeed;
        _agent.speed = _moveSpeed;
        _timeLimit = timeLimit;
        _stoppingDistanceSqr = _agent.stoppingDistance * _agent.stoppingDistance;
    }

    public void OnEnter()
    {
        _agent.speed = _moveSpeed;
        _target = Director.Instance.LastInterestingLocation;
        _timer = 0;
        _agent.SetDestination(_target);
        _gotInvestigatePoint = false;
    }

    public void OnExit()
    {
        _gotInvestigatePoint = false;
    }

    public void Tick()
    {
        _timer += Time.deltaTime;
        if (_agent.transform.position.FlatVectorDistanceSquared(_target) < _stoppingDistanceSqr)
        {
            if (!_gotInvestigatePoint)
            {
                _target = Director.Instance.PickInvestigateTarget();
                _agent.SetDestination(_target);
                _gotInvestigatePoint = true;
            }
            else
            {
                if (!TrySetWanderTarget(Director.Instance.BirdPosition))
                {
                    _gotInvestigatePoint = false;
                }
            }
        }
    }

    public bool SearchedForTooLong() { return _timer > _timeLimit; }


    private bool TrySetWanderTarget(Vector3 origin)
    {
        bool validDestination = false;
        int attempts = 100;
        var destination = origin;
        while (!validDestination && attempts > 0)
        {
            var random = Random.insideUnitCircle;
            var direction = new Vector3(random.x, 0, random.y);
            destination = origin + direction.normalized * Random.Range(_wanderRadius/2f, _wanderRadius);
            validDestination = SamplePosition(destination);          
            attempts--;
        }
        if (validDestination)
        {
            _target = destination;
            _agent.SetDestination(_target);
        }
        return validDestination;
    }

    bool SamplePosition(Vector3 position)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(position, out hit, _agent.height * 2, NavMesh.AllAreas); //NavMesh.GetAreaFromName("Walkable"));
    }
}
