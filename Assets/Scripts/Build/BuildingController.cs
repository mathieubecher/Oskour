using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public enum BuildType
    {
        Farm, House, Repository, Hand, Oxygenator, Light, Coffee, Submarine, Infirmary, Turbine, Ruin
    }

    protected StateBuild _state;
    private GameManager _manager;

    [Header("Infos")]
    public new string name;
    public string description;
    public BuildType type;

    [HideInInspector]
    public float resourcesValue = 0;
    public readonly float resourcesScore = 0;
    
    [Space]
    public List<BuildType> requires;

    

    // Start is called before the first frame update
    private void Start()
    {
        _manager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void SetState(StateBuild state) { _state = state;}

    public void Construct(float value) { _state.Construct(value); }

    public virtual void Interact(bool ctrl, CharacterController character)
    {
        if(ctrl) Construct(-_manager.destroySpeed);
        else if(_state.Type == StateBuild.StateList.Construct) Construct(_manager.constructSpeed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(_state.Type == StateBuild.StateList.Placing)
            ((PlacingBuild)_state).colliders.Add(other);
    }
    private void OnTriggerExit(Collider other)
    {
        if (_state.Type == StateBuild.StateList.Placing)
            ((PlacingBuild)_state).colliders.Remove(other);
    }
    
    // INSPECTOR
    #region Inspector
    private void Reset()
    {
        Clear();
        ReOrder();
        AddAnimator();
    }

    private void Clear()
    {
        BuildingController[] controllers = GetComponents<BuildingController>();
        foreach(BuildingController controller in controllers) 
            if(controller != this) DestroyImmediate(controller);
    }

    private void ReOrder()
    {
        Component[] components = GetComponents<Component>();
        int i = 0;
        while (components[i] != this) ++i;
        while (i != 0)
        {
            if (i > 0)
            {
                UnityEditorInternal.ComponentUtility.MoveComponentUp(this);
                --i;
            }
            else
            {
                UnityEditorInternal.ComponentUtility.MoveComponentDown(this);
                ++i;
            }

        }
    }

    protected virtual void AddAnimator()
    {
        if (!GetAnimator(out Animator animator)) animator = gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = GetAnimatorController("BuildController");
    }
    protected bool GetAnimator(out Animator animator)
    {
        animator = GetComponent<Animator>();
        return animator != null;
    }

    protected UnityEditor.Animations.AnimatorController GetAnimatorController(string path)
    {
        return UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath("Assets/Resources/BuildState/"+path);
    }
    #endregion
}
