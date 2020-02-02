using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IddleState : State
{
   public IddleState(CharacterController controller) : base(controller)
    {
        controller.stateInfo = "Iddle";
        controller.IA.isStopped = true;
    }
    public override void Update()
    {
        //controller.GetComponent<Rigidbody>().velocity = Vector3.zero;   
    }


    public override void Exit()
    {

    }
    public override void Iddle() {}
}
