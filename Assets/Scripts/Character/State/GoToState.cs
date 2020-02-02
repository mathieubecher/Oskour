using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToState : State
{
    private Vector3 target;
    public GoToState(CharacterController controller, Vector3 target) : base(controller)
    {
        this.target = target;
        controller.stateInfo = "GoTo";
        controller.IA.isStopped = false;
        controller.IA.SetDestination(target);
    }
    public override void Update()
    {
        controller.IA.isStopped = false;
        float dist = controller.IA.remainingDistance;
        if (dist < 1 || (controller.transform.position - target).magnitude <= controller.IA.stoppingDistance)
            Exit();


    }
    public override void Exit()
    {
        controller.state = new IddleState(controller);
        
    }
}
