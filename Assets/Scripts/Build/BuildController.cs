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
    public StateBuild State
    {
        get => _state;
    }
    protected Animator _animator;
    [HideInInspector]
    public GameManager _manager;
    

    [ContextMenu("Prepare shader")]
    void ConstructShader()
    {
        
        if(TryGetComponent<MaterialsGestor>(out materials)) DestroyImmediate(materials);

        materials = gameObject.AddComponent<MaterialsGestor>();
    }

    
    [Header("Infos")]
    public new string name;
    public string description;
    public BuildType type;
    public int tier = 1;
    
    public double resourcesValue = 1;
    public float height = 100;
    
    [Space]
    public List<BuildType> requires;
    [HideInInspector]
    public MaterialsGestor materials;

    public static readonly int ProgressBuild = Animator.StringToHash("progress");


    // Start is called before the first frame update
    protected virtual void Awake()
    {
        
        _animator = gameObject.GetComponent<Animator>();
        _manager = FindObjectOfType<GameManager>();
        
        
    }

    public void ActivateBuild()
    {
        //build.transform.GetChild(0).GetComponent<NavMeshObstacle>().enabled = true;
        transform.GetChild(0).GetComponent<Collider>().enabled = true;
        Destroy(GetComponent<Rigidbody>());
        gameObject.layer = 10;
    }
    // Update is called once per frame
    private void Update()
    {
        
    }

    public void SetState(StateBuild state) { _state = state;}

    public bool Construct(float value) { return _state.Construct(value); }

    public virtual bool Interact(bool ctrl, CharacterController character)
    {
        if(ctrl) return Construct(-Time.deltaTime * _manager.constructSpeed / 10f);
        return _state.Type == StateBuild.StateList.Construct && Construct(Time.deltaTime * _manager.destroySpeed / 10f);
    }
    
    public bool Active()
    {
        return _state.Type == StateBuild.StateList.Active;
    }

    public float Progress()
    {
        return _animator.GetFloat(ProgressBuild);
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if(_state.Type == StateBuild.StateList.Placing)
            ((PlacingBuild)_state).AddCollider(other);
    }
    protected virtual void OnTriggerExit(Collider other)
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
        ConstructShader();
    }

    private void Clear()
    {
        BuildController[] controllers = GetComponents<BuildController>();
        foreach(BuildController controller in controllers)
            if (controller != this)
            {
                this.name = controller.name;
                this.description = controller.description;
                this.type = controller.type;
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
