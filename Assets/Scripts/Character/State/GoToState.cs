using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToState : State
{
    private readonly Vector3 _target;
    public GoToState(CharacterController controller, Vector3 target) : base(controller)
    {
        this._target = target;
        controller.stateInfo = "GoTo";
        controller.IA.isStopped = false;
        controller.IA.SetDestination(target);
    }
    public override void Update()
    {
        controller.IA.isStopped = false;
        
        //controller.IA.SetDestination(_target);
        float dist = controller.IA.remainingDistance;
        if ((controller.transform.position - _target).magnitude <= controller.IA.stoppingDistance)
            Exit();


    }
    public override void Exit()
    {
        controller.state = new IdleState(controller);
        
    }
}
