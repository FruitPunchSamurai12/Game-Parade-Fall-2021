using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class CatAnimator : MonoBehaviour
{
    Animator _animator;
    NavMeshAgent _agent;
    AIStateMachine _catStateMachine;
    [SerializeField] string _patrolTrigger;
    [SerializeField] string _suspiciousTrigger;
    [SerializeField] string _investigateTrigger;
    [SerializeField] string _chaseTrigger;

    public static event Action onStep;
    public static event Action catHiss;
    public static event Action catSniff;


    CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        _agent = GetComponentInParent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _catStateMachine = GetComponentInParent<AIStateMachine>();
        _catStateMachine.OnAIStateChanged += HandleCatStateChanged;

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnDestroy()
    {
        _catStateMachine.OnAIStateChanged -= HandleCatStateChanged;
    }

    private void HandleCatStateChanged(IState state)
    {
        string CatState = state.GetType().ToString();
        Debug.Log(CatState);
        switch (CatState)
        {
            case "Patrol":
                EnterPatrol();
                return;
            case "Suspicious":
                EnterSuspicious();
                return;
            case "Investigate":
                EnterInvestigate();
                return;
            case "Chase":
                EnterChase();
                return;
        }
    }

    public void EnterPatrol()
    {
        SetTrigger(_patrolTrigger);
    }

    public void EnterSuspicious()
    {
        SetTrigger(_suspiciousTrigger);
    }
    public void EnterInvestigate()
    {
        SetTrigger(_investigateTrigger);
    }
    public void EnterChase()
    {
        SetTrigger(_chaseTrigger);
    }

    public void SetTrigger(string trigger)
    {
        _animator.SetTrigger(trigger);
    }

    public void AnimationStartMove()
    {
        _agent.isStopped=false;
    }

    public void AnimationStopMove()
    {
        _agent.isStopped = true;
    }

    public void Footstep()
    {
        impulseSource.GenerateImpulse();
        onStep.Invoke();
    }

    public void CatHiss()
    {
        impulseSource.GenerateImpulse();
        catHiss.Invoke();
    }

    public void CatSniff()
    {
        impulseSource.GenerateImpulse();
        catSniff.Invoke();
    }
}
