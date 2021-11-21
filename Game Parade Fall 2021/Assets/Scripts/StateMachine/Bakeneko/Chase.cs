using UnityEngine;
using UnityEngine.AI;

public class Chase : IState
{
    NavMeshAgent _agent;
    float _moveSpeed;
    Transform _target;
    float _catchDistanceSqr;


    public Chase(NavMeshAgent agent,float moveSpeed,float catchDistance)
    {
        _target = GameManager.Instance.PlayerTransform;
        _agent = agent;
        _moveSpeed = moveSpeed;
        _catchDistanceSqr = catchDistance * catchDistance;
    }

    public void OnEnter()
    {
        _agent.isStopped = false;
        _agent.speed = _moveSpeed;
        if (_target != null)
            _agent.SetDestination(_target.position);
        else
            Debug.LogError("Target is null");
    }

    public void OnExit()
    {

    }

    public void Tick()
    {
        if (_target != null)
        {
            _agent.SetDestination(_target.position);
            if (_agent.transform.position.FlatVectorDistanceSquared(_target.position) < _catchDistanceSqr)
            {
                GameManager.Instance.BirdGotCaught();
            }
        }
        else
            Debug.LogError("Target is null");
    }
}
