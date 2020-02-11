using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public class PlacingBuild : StateBuild
{
    public List<Collider> colliders;
    public Material placing;
    public Material error;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator,stateInfo,layerIndex);
        type = StateList.Placing;
        build.materials.SetMaterial(placing);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    public void AddCollider(Collider collider)
    {
        if(colliders.Count == 0) build.materials.SetMaterial(error);
        colliders.Add(collider);
    }

    public void RemoveCollider(Collider collider)
    {
        if(colliders.Count == 1) build.materials.SetMaterial(placing);
        colliders.Remove(collider);
    }
    
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        build.ActivateBuild();
        
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
        if(colliders.Count <= 0) _animator.SetTrigger(ConstructTrigger);
        else
        {
            return false;
            //TODO Feedback
        }

        return true;
    }


}
