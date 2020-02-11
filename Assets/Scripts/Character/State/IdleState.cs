using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
   public IdleState(CharacterController controller) : base(controller)
    {
        controller.stateInfo = "Idle";
        controller.IA.isStopped = true;
    }
    public override void Update()
    {
        //controller.GetComponent<Rigidbody>().velocity = Vector3.zero;   
    }


    public override void Exit()
    {

    }
    public override void Idle() {}
}
