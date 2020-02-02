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
        
    }


    public override void Exit()
    {

    }
    public override void Iddle() {}
}
