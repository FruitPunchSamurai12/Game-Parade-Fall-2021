using System;
using UnityEngine;
using UnityEngine.AI;


///NOT USED

public class Ambush : IState
{
    NavMeshAgent _agent;
    float _moveSpeed;
    float _rotationSpeed;  
    float _stopDistance;
    Vector3 _ambushPosition;
    Vector3 _lookAtPosition;

    public event Action onFailToAmbush;
    public Ambush(NavMeshAgent agent, float moveSpeed, float rotationSpeed, float stopDistance)
    {
        _agent = agent;
        _moveSpeed = moveSpeed;
        _rotationSpeed = rotationSpeed;
        _stopDistance = stopDistance * stopDistance;
    }

    public void OnEnter()
    {
        _agent.speed = _moveSpeed;
       // if(Director.Instance.GetAmbushAndLookPoint(out _ambushPosition,out _lookAtPosition))
       // {
       //     _agent.SetDestination(_ambushPosition);
       // }
       // else
       //     onFailToAmbush?.Invoke();
        
    }

    public void OnExit()
    {
        _agent.isStopped = false;
    }

    public void Tick()
    {
        if (_agent.transform.position.FlatVectorDistanceSquared(_ambushPosition) < _stopDistance)
        {
            _agent.isStopped = true;
            Vector3 targetDirection = _lookAtPosition - _agent.transform.position;
            Vector3 newDirection = Vector3.RotateTowards(_agent.transform.forward, targetDirection, _rotationSpeed * Time.deltaTime, 0.0f);
            _agent.transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}
