using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveController : BuildingController
{
    
    public List<BonusResources> resources;
    public struct BonusResources
    {
        public ActiveBuild.Resources resource;
        [Range(0,10)]
        public float value;
    }


    public override void Interact(bool ctrl, CharacterController character)
    {
        base.Interact(ctrl, character);
        if(!ctrl && _state.Type == StateBuild.StateList.Active) _state.Interact(character);
    }
    
    #region Inspector

    protected override void AddAnimator()
    {
        if (!GetAnimator(out Animator animator)) animator = gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = GetAnimatorController("InteractiveController");
    }

    #endregion Inspector
}
