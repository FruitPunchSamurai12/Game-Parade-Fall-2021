using UnityEngine;
using UnityEngine.AI;

public class Patrol : IState
{
    NavMeshAgent _agent;
    float _moveSpeed;
    PatrolRoute _currentRoute;
    int _currentRouteIndex;
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
        _agent.speed = _moveSpeed;
        AssignRandomRoute();
    }

    private void AssignRandomRoute()
    {
        AssignNewRoute(Director.Instance.PickRandomRoute());
    }

    public void AssignNewRoute(PatrolRoute route)
    {
        _currentRoute = route;
        _currentRouteIndex = 0;
        _target = _currentRoute._routeWaypoints[0].transform.position;
        _agent.SetDestination(_target);
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        if (_agent.transform.position.FlatVectorDistanceSquared(_target) < _stoppingDistanceSqr)
        {
            _currentRouteIndex++;
            if (_currentRoute._routeWaypoints.Count >= _currentRouteIndex)
            {
                AssignRandomRoute();
            }
            else
            {
                _target = _currentRoute._routeWaypoints[_currentRouteIndex].transform.position;
                _agent.SetDestination(_target);
            }
        }
    }
}
