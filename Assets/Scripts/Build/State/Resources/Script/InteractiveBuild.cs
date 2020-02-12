using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveBuild : ActiveBuild
{
    private List<CharacterController> characters;
    public enum Resources
    {
        Energy, Oxygen, Food
    }
    
    public override bool Interact(CharacterController character)
    {
        if (!characters.Find(c => c == character))
        {
            characters.Add(character);
            character.gameObject.SetActive(false);
            build._manager.UnSelect(character);
        }
        else return false;
        return true;
    }
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator,stateInfo,layerIndex);
        type = StateList.Active;
        characters = new List<CharacterController>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int i = 0;
        while (i < characters.Count)
        {
            if (characters[i].Select)
            {
                LeaveBuild(characters[i]);
            }
            else
            {
                bool end = true;
                foreach (InteractiveController.BonusResources resource in ((InteractiveController)build).resources)
                {
                    switch (resource.resource)
                    {
                        case Resources.Energy:
                            end &= characters[i].energy >= 1;
                            break;
                        case Resources.Food :
                            end &= characters[i].food >= 1;
                            break;
                        case Resources.Oxygen:
                            end &= characters[i].oxygen >= 1;
                            
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                ++i;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator,stateInfo,layerIndex);
        while(characters.Count > 0)
        {
            LeaveBuild(characters[0]);
        }
    }

    private void LeaveBuild(CharacterController character)
    {
        character.gameObject.SetActive(true);
        character.state.Idle();
        characters.Remove(character);
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
}
