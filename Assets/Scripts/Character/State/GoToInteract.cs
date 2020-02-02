using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToInteract : GoToState
{
    BuildController build;
    CharacterController.Interact state;
    public GoToInteract(CharacterController controller, CharacterController.Interact state, BuildController build) : base(controller,build.transform.position)
    {
        controller.stateInfo = "GoToInteract";
        this.state = state;
        this.build = build;
    }
    public override void Update()
    {
        base.Update();
    }
    public override void Exit()
    {
        if (state == CharacterController.Interact.CONSTRUCT) controller.state = new ConstructState(controller, build);
        else /* if(state == CharacterController.Interact.DESTRUCT)*/ controller.state = new DestroyState(controller, build);
        //else build.Interact(controller);
    }
    public override void Collide(BuildController build) {
        if (build == this.build)
        {
            Exit();
        }
    }
}
