using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuinController : BuildController
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        ActivateBuild();
        GetComponent<Animator>().SetFloat(ProgressBuild, 1);
        _manager.listBuild.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
    }
    protected override void OnTriggerExit(Collider other)
    {
    }
    
    #region Inspector

    protected override void AddAnimator()
    {
        if (!GetAnimator(out Animator animator)) animator = gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = GetAnimatorController("RuinController");
    }
    
    #endregion Inspector
}
