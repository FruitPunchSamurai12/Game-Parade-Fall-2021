using UnityEngine;
using UnityEngine.AI;
using System;

public class AIStateMachine : MonoBehaviour
{
    StateMachine _stateMachine;
    NavMeshAgent _agent;
    Bakeneko _bakeneko;

    Patrol _patrol;

    public Type CurrentStateType => _stateMachine.CurrentState.GetType();
    public event Action<IState> OnAIStateChanged;

    private void Awake()
    {
        _stateMachine = new StateMachine();
        _stateMachine.OnStateChanged += state => OnAIStateChanged?.Invoke(state);
        _agent = GetComponent<NavMeshAgent>();
        _bakeneko = GetComponent<Bakeneko>();
        _patrol = new Patrol(_agent,_bakeneko.NeutralSpeed);
        var chase = new Chase(_agent, _bakeneko.ChaseSpeed, _bakeneko.CatchDistance);
        var investigate = new Investigate(_agent, _bakeneko.ChaseSpeed, _bakeneko.InvestigationTime,_bakeneko.InvestigationRadius);
        var suspiciousHigh = new Suspicious(_agent, _bakeneko.LookRotatioNSpeed, _bakeneko.ReactionTime);
        var suspiciousLow = new Suspicious(_agent, _bakeneko.LookRotatioNSpeed, _bakeneko.ReactionTime);
        _stateMachine.AddTransition(_patrol, suspiciousHigh, _bakeneko.CanSeePlayer);
        _stateMachine.AddTransition(_patrol, suspiciousLow, _bakeneko.CanFeel);
        _stateMachine.AddTransition(_patrol, suspiciousLow, _bakeneko.CanSeeMarks);
        _stateMachine.AddTransition(suspiciousHigh, investigate, suspiciousHigh.TimeElapsed);
        _stateMachine.AddTransition(suspiciousHigh, investigate, () => _bakeneko.CanSee() == false && _bakeneko.CanFeel() == false);
        _stateMachine.AddTransition(suspiciousLow, investigate, suspiciousLow.TimeElapsed);
        _stateMachine.AddTransition(suspiciousLow, suspiciousHigh, _bakeneko.CanSeePlayer);
        _stateMachine.AddTransition(suspiciousLow, suspiciousHigh, _bakeneko.CanFeel);
        _stateMachine.AddTransition(investigate, chase, _bakeneko.CanFeel);
        _stateMachine.AddTransition(investigate, chase, _bakeneko.CanSeePlayer);
        _stateMachine.AddTransition(investigate, suspiciousLow, _bakeneko.CanSeeMarks);
        _stateMachine.AddTransition(chase, investigate,()=>_bakeneko.CanSeePlayer()==false && _bakeneko.CanFeel()==false);
        _stateMachine.AddTransition(investigate, _patrol, investigate.SearchedForTooLong);

        _stateMachine.SetState(_patrol);
    }

 

    public void ResetStateMachine(PatrolRoute patrolRoute)
    {
        _agent.enabled = false;
        transform.position = patrolRoute._routeWaypoints[0].transform.position;
        _agent.enabled = true;
        _stateMachine.SetState(_patrol);
        _patrol.AssignNewRoute(patrolRoute);
    }


    private void Update()
    {
        _stateMachine.Tick();
        string CatState = CurrentStateType.ToString();
        switch (CatState)
        {
            case "Patrol":
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("CatState", 0f);
                break;
            case "Suspicious":
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("CatState", 1f);
                break;
            case "Investigate":
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("CatState", 2f);
                break;
            case "Chase":
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("CatState", 3f);
                break;
            default:
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("CatState", 0f);
                break;
        }

    }
}
