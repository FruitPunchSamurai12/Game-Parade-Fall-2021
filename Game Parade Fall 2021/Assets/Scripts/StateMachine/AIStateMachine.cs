using UnityEngine;
using UnityEngine.AI;
using System;

public class AIStateMachine : MonoBehaviour
{
    StateMachine _stateMachine;
    NavMeshAgent _agent;
    Bakeneko _bakeneko;
    public Type CurrentStateType => _stateMachine.CurrentState.GetType();
    public event Action<IState> OnAIStateChanged;

    private void Awake()
    {
        _stateMachine = new StateMachine();
        _stateMachine.OnStateChanged += state => OnAIStateChanged?.Invoke(state);
        _agent = GetComponent<NavMeshAgent>();
        _bakeneko = GetComponent<Bakeneko>();
        var patrol = new Patrol(_agent,_bakeneko.NeutralSpeed);
        var chase = new Chase(_agent, _bakeneko.ChaseSpeed, _bakeneko.CatchDistance);
        _stateMachine.AddTransition(patrol, chase, _bakeneko.CanSee);

        _stateMachine.SetState(patrol);
    }

    private void Update()
    {
        _stateMachine.Tick();
       
    }
}
