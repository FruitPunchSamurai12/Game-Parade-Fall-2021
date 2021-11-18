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
        var suspiciousHigh = new Suspicious(_agent, _bakeneko.LookRotatioNSpeed, _bakeneko.ReactionTime);
        var suspiciousLow = new Suspicious(_agent, _bakeneko.LookRotatioNSpeed, _bakeneko.ReactionTime);
        _stateMachine.AddTransition(patrol, suspiciousHigh, _bakeneko.CanSeePlayer);
        _stateMachine.AddTransition(patrol, suspiciousLow, _bakeneko.CanSeeMarks);
        _stateMachine.AddTransition(suspiciousHigh, investigate, suspiciousHigh.TimeElapsed);
        _stateMachine.AddTransition(suspiciousHigh, investigate, () => _bakeneko.CanSee() == false);
        _stateMachine.AddTransition(suspiciousLow, investigate, suspiciousLow.TimeElapsed);
        _stateMachine.AddTransition(suspiciousLow, suspiciousHigh, _bakeneko.CanSeePlayer);
        _stateMachine.AddTransition(investigate, chase, _bakeneko.CanSeePlayer);
        _stateMachine.AddTransition(investigate, suspiciousLow, _bakeneko.CanSeeMarks);
        _stateMachine.AddTransition(chase, investigate,()=>_bakeneko.CanSeePlayer()==false);
        _stateMachine.AddTransition(investigate, patrol, investigate.SearchedForTooLong);

        _stateMachine.SetState(patrol);
    }

    private void Update()
    {
        _stateMachine.Tick();
       
    }
}
