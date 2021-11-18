using UnityEngine;
using UnityEngine.AI;

public class Suspicious : IState
{
    NavMeshAgent _agent;
    Vector3 _target;
    float _rotationSpeed;
    float _timeLimit;
    float _timer;

    public Suspicious(NavMeshAgent agent, float rotationSpeed, float timeLimit)
    {
        _rotationSpeed = rotationSpeed;
        _agent = agent;
        _timeLimit = timeLimit;       
    }

    public void OnEnter()
    {       
        _timer = 0;
        _agent.isStopped = true;
        _target = Director.Instance.LastInterestingLocation;
    }

    public void OnExit()
    {
        _agent.isStopped = false;
    }

    public void Tick()
    {
        _timer += Time.deltaTime;
        Vector3 targetDirection = _target - _agent.transform.position;
        Vector3 newDirection = Vector3.RotateTowards(_agent.transform.forward, targetDirection, _rotationSpeed * Time.deltaTime, 0.0f);
        _agent.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public bool TimeElapsed() { return _timer > _timeLimit; }
}