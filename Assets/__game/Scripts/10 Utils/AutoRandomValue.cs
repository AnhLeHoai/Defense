using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class AutoRandomValue : StateMachineBehaviour
{
    
    public string valueParameter;
    public int minValue;
    public int maxValue;
    public int stopValue;

    [Space(10)] 
    public string transitionParameter;
    public float percentCompleteBeforeCallTrigger;

    private bool shouldStop = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
        AnimatorControllerPlayable controller)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex, controller);
        DOVirtual.DelayedCall(stateInfo.length * percentCompleteBeforeCallTrigger, () => SetTrigger(animator, stateInfo, layerIndex, controller));
        shouldStop = false;
    }

    private void SetTrigger(
        Animator animator, 
        AnimatorStateInfo stateInfo, 
        int layerIndex,
        AnimatorControllerPlayable controller)
    {
        base.OnStateExit(animator, stateInfo, layerIndex, controller);

        if (animator.GetInteger(valueParameter) == stopValue) return;
        if (shouldStop) return;
        
        int randomValue = Random.Range(minValue, maxValue + 1);
        animator.SetInteger(valueParameter, randomValue);

        if (!string.IsNullOrEmpty(transitionParameter))
            animator.SetTrigger(transitionParameter);
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        base.OnStateMachineExit(animator, stateMachinePathHash);
        shouldStop = true;
    }

    private void OnEnable()
    {
        shouldStop = false;
    }

    private void OnDisable()
    {
        shouldStop = true;
    }
}