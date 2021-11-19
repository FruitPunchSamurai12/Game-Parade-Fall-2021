using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimator : MonoBehaviour
{
    Animator _animator;
    AIStateMachine _catStateMachine;
    [SerializeField] string _patrolTrigger;
    [SerializeField] string _suspiciousTrigger;
    [SerializeField] string _investigateTrigger;
    [SerializeField] string _chaseTrigger;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _catStateMachine = GetComponent<AIStateMachine>();
        _catStateMachine.OnAIStateChanged += HandleCatStateChanged;
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
}