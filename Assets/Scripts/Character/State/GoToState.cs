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
        controller.IA.SetDestination(target);
        controller.stateInfo = "GoTo";
        controller.IA.isStopped = false;
    }
    public override void Update()
    {
        float dist = controller.IA.remainingDistance;
        Debug.Log(dist);
        if (dist < 1 || (controller.transform.position - target).magnitude <= controller.IA.stoppingDistance)
            Exit();


    }
    public override void Exit()
    {
        controller.state = new IddleState(controller);
        
    }
}
