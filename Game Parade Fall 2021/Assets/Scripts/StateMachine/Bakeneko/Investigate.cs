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

    public Investigate(NavMeshAgent agent, float moveSpeed,float timeLimit)
    {
        _agent = agent;
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
    }

    public void OnExit()
    {

    }

    public void Tick()
    {
        _timer += Time.deltaTime;
        if (_agent.transform.position.FlatVectorDistanceSquared(_target) < _stoppingDistanceSqr)
        {
            _target = Director.Instance.PickInvestigateTarget();
            _agent.SetDestination(_target);
        }
    }

    public bool SearchedForTooLong() { return _timer > _timeLimit; }
}
