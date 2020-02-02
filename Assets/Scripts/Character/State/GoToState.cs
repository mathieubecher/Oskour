using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToState : State
{
    public GoToState(CharacterController controller, Vector3 target) : base(controller)
    {
        controller.IA.SetDestination(target);
    }
    public override void Update()
    {
        float dist = controller.IA.remainingDistance;
        if (dist != Mathf.Infinity && controller.IA.pathStatus == NavMeshPathStatus.PathComplete && controller.IA.remainingDistance == 0)
            Exit();


    }
    public override void Exit()
    {
        controller.state = new IddleState(controller);
    }
}
