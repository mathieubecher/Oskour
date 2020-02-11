using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BuildController : MonoBehaviour
{
    public enum BuildType
    {
        Farm, House, Repository, Hand, Oxygenator, Light, Coffee, Submarine, Infirmary, Turbine, Ruin
    }

    protected StateBuild _state;
    [HideInInspector]
    public GameManager _manager;

    [Header("Infos")]
    public new string name;
    public string description;
    public BuildType type;
    public int tier = 1;

    [HideInInspector]
    public float resourcesValue = 0;
    public readonly float resourcesScore = 0;
    
    [Space]
    public List<BuildType> requires;
    [HideInInspector]
    public MaterialsGestor materials;

   

    // Start is called before the first frame update
    private void Start()
    {
        materials = gameObject.AddComponent<MaterialsGestor>();
        _manager = FindObjectOfType<GameManager>();
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void SetState(StateBuild state) { _state = state;}

    public bool Construct(float value) { return _state.Construct(value); }

    public virtual bool Interact(bool ctrl, CharacterController character)
    {
        if(ctrl) return Construct(-_manager.destroySpeed / _manager.timeScale * Time.deltaTime);
        if(_state.Type == StateBuild.StateList.Construct) return Construct(_manager.constructSpeed / _manager.timeScale * Time.deltaTime);
        return false;
    }
    
    public bool Active()
    {
        return _state.Type == StateBuild.StateList.Active;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(_state.Type == StateBuild.StateList.Placing)
            ((PlacingBuild)_state).AddCollider(other);
    }
    private void OnTriggerExit(Collider other)
    {
        if (_state.Type == StateBuild.StateList.Placing)
            ((PlacingBuild)_state).RemoveCollider(other);
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
        BuildController[] controllers = GetComponents<BuildController>();
        foreach(BuildController controller in controllers)
            if (controller != this)
            {
                this.name = controller.name;
                this.description = controller.description;
                this.tier = controller.tier;
                this.requires = controller.requires;
                DestroyImmediate(controller);
            }
    }

    private void ReOrder()
    {
        Component[] components = GetComponents<Component>();
        int i = 0;
        while (components[i] != this) ++i;
        while (i != 1)
        {
            if (i > 1)
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

    protected RuntimeAnimatorController GetAnimatorController(string path)
    {
        if(Resources.Load<UnityEditor.Animations.AnimatorController>(path) != null)
            return  (AnimatorController)Resources.Load<UnityEditor.Animations.AnimatorController>(path);
        
        return Resources.Load<RuntimeAnimatorController>(path);
    }
    #endregion
}
