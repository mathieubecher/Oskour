using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveController : BuildController
{
    public bool storable;
    public List<BonusResources> resources;
    public int maxCharacter = 5;
    [Serializable]
    public struct BonusResources
    {
        public InteractiveBuild.Resources resource;
        [Range(0,10)]
        public float value;
    }

    
    public override bool Interact(bool ctrl, CharacterController character)
    {
        if(!ctrl && Active()) return _state.Interact(character);
        return base.Interact(ctrl, character);
        
    }
    
    #region Inspector

    protected override void AddAnimator()
    {
        if (!GetAnimator(out Animator animator)) animator = gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = GetAnimatorController("InteractiveController");
    }

    #endregion Inspector
}
