﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBuild : StateMachineBehaviour
{
    public enum StateList
    {
        Construct, Active, Placing
    }
    protected static readonly int Progress = Animator.StringToHash("progress");
    protected static readonly int ConstructTrigger = Animator.StringToHash("Construct");
    
    protected Animator _animator;
    protected BuildController build;
    
    protected StateList type;
    public StateList Type { get=>type; }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _animator = animator;
        build = animator.GetComponent<BuildController>();
        build.SetState(this);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that sets up animation IK (inverse kinematics)
    }

    public virtual bool Interact(CharacterController character)
    {
        return false;
    }
    public virtual bool Construct(float value)
    {
        _animator.SetTrigger(ConstructTrigger);
        return true;
    }
}
