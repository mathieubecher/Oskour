using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructBuild : StateBuild
{
    
    public Material construct;
    public AnimationCurve exitcurve;
    private bool exit = false;
    public float exitSpeed = 3;
    private float exitProgress = 1;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator,stateInfo,layerIndex);
        type = StateList.Construct;
        
        if(_animator.GetFloat(Progress)  >= 1)_animator.SetFloat(Progress, 1);
        else if(_animator.GetFloat(Progress)  <= 0) _animator.SetFloat(Progress, 0);
        
                
        //build.materials.ResetMaterial();
        build.materials.SetProgress(animator.GetFloat(Progress));
    }
    
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (exit)
        {
            exitProgress -= Time.deltaTime * exitSpeed;
            if(exitProgress < 0){
                build._manager.listBuild.Remove(build);
                Destroy(_animator.gameObject);
            }
            else
            {
                build.materials.SetExit(exitcurve.Evaluate(exitProgress));
                build.materials.SetProgress((animator.GetFloat(Progress)- 1 + exitProgress));
            }
        }
        else
        {
            build.materials.SetProgress(animator.GetFloat(Progress));
        }
    }
    

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    public override bool Construct(float value)
    {
        if (exit) return true;
        if (value > 0 && build._manager.GetResources() < value) value = build._manager.GetResources();
        
        _animator.SetFloat(Progress, _animator.GetFloat(Progress) + value);
        
        if(_animator.GetFloat(Progress)  >= 1)_animator.SetFloat(Progress, 1);
        else if(_animator.GetFloat(Progress)  <= 0) _animator.SetFloat(Progress, 0);

        
        if (_animator.GetFloat(Progress) >= 1)
        {
            _animator.SetFloat(Progress, 1);
            _animator.SetTrigger(ConstructTrigger);   
        }

        build._manager.GetResources();
        if (_animator.GetFloat(Progress) > 0) return true;

        exit = true;

        return true;
    }
}
