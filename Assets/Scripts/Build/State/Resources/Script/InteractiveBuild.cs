using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveBuild : ActiveBuild
{
    protected List<CharacterController> characters;
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
            character.enabled = false;
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
                if(CharacterAction(characters[i])) LeaveBuild(characters[i]);
                else ++i;
            }
        }

        int countChar = characters.Count;
        
        if (countChar < ((InteractiveController) build).maxCharacter && ((InteractiveController) build).storable)
        {
            List<StorageBuild> repositories = new List<StorageBuild>();
            foreach (BuildController otherBuild in build._manager.listBuild)
            {
                if (otherBuild.type == BuildController.BuildType.Repository && otherBuild.Active())
                {
                    repositories.Add((StorageBuild)otherBuild.State);  
                }
            }

            if (repositories.Count > 0)
            {
                float incrValue = (1-(countChar / (float)((InteractiveController) build).maxCharacter)) / (float)repositories.Count;
                Debug.Log(incrValue);
                foreach (StorageBuild repository in repositories)
                {
                    foreach (InteractiveController.BonusResources resource in ((InteractiveController) build).resources)
                    {
                        switch (resource.resource)
                        {
                            case Resources.Energy:
                                repository.storeCoffee = Mathf.Min(repository.maxCoffee,repository.storeCoffee + Time.deltaTime * resource.value * incrValue);
                                break;
                            case Resources.Food:
                                repository.storeFood = Mathf.Min(repository.maxFood,repository.storeFood + Time.deltaTime * resource.value * incrValue);
                                break;
                        }
                    }
                }
                
            }
        }
            
    }

    protected virtual bool CharacterAction(CharacterController character)
    {
        bool end = true;

        float repartCharacter = ((((InteractiveController) build).storable) ? 1:(((InteractiveController) build).maxCharacter-characters.Count)) / (float)((InteractiveController) build).maxCharacter;
        Debug.Log("repart : " + repartCharacter);
        foreach (InteractiveController.BonusResources resource in ((InteractiveController)build).resources)
        {
            float incrValue = Time.deltaTime * resource.value * repartCharacter / build._manager.timeScale;
            Debug.Log("incr : " + incrValue);
            switch (resource.resource)
            {
                case Resources.Energy:
                    character.energy = Mathf.Min(1,character.energy + incrValue);
                    end &= character.energy >= 1;
                    break;
                case Resources.Food :
                    character.food = Mathf.Min(1,character.food + incrValue);
                    end &= character.food >= 1;
                    break;
                case Resources.Oxygen:
                    character.oxygen = Mathf.Min(1,character.oxygen + incrValue);
                    end &= character.oxygen >= 1;
                            
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return end;
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
        character.enabled = true;
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
