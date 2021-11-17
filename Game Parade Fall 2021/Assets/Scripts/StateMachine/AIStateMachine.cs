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
        var investigate = new Investigate(_agent, _bakeneko.ChaseSpeed, _bakeneko.InvestigationTime);
        var suspicious = new Suspicious(_agent, _bakeneko.LookRotatioNSpeed, _bakeneko.ReactionTime);
        _stateMachine.AddTransition(patrol, suspicious, _bakeneko.CanSee);
        _stateMachine.AddTransition(suspicious, investigate, suspicious.TimeElapsed);
        _stateMachine.AddTransition(suspicious, investigate, () => _bakeneko.CanSee() == false);
        _stateMachine.AddTransition(investigate, chase, _bakeneko.CanSee);
        _stateMachine.AddTransition(chase, investigate,()=>_bakeneko.CanSee()==false);
        _stateMachine.AddTransition(investigate, patrol, investigate.SearchedForTooLong);

        _stateMachine.SetState(patrol);
    }

    private void Update()
    {
        _stateMachine.Tick();
       
    }
}
