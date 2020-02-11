using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructBuild : StateBuild
{
    
    public Material construct;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator,stateInfo,layerIndex);
        type = StateList.Construct;
        if(_animator.GetFloat(Progress) >= 1) _animator.SetFloat(Progress, 0.99f);
        else if(_animator.GetFloat(Progress) <= 0) _animator.SetFloat(Progress, 0.01f);
        build.materials.SetMaterial(construct);
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetFloat(Progress) <= 0)
        {
            build._manager.listBuild.Remove(build);
            Destroy(animator.gameObject);
        }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
    public override bool Construct(float value)
    {
        _animator.SetFloat(Progress, _animator.GetFloat(Progress) + value);
        if(_animator.GetFloat(Progress)>=1 ) type = StateList.Active;
        return true;
    }
}
