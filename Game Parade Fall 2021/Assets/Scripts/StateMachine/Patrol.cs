using UnityEngine;
using UnityEngine.AI;

public class Patrol : IState
{
    NavMeshAgent _agent;
    float _moveSpeed;
    Vector3 _target;
    float _stoppingDistanceSqr;


    public Patrol(NavMeshAgent agent, float moveSpeed)
    {
        _agent = agent;
        _moveSpeed = moveSpeed;
        _agent.speed = _moveSpeed;
        _stoppingDistanceSqr = _agent.stoppingDistance* _agent.stoppingDistance;
    }

    public void OnEnter()
    {
        _target = WaypointManager.Instance.PickRandomPath();
        _agent.SetDestination(_target);
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        if(_agent.transform.position.FlatVectorDistanceSquared(_target)<_stoppingDistanceSqr)
        {
            _target = WaypointManager.Instance.PickRandomPath();
            _agent.SetDestination(_target);
        }
    }
}
