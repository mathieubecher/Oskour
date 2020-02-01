using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructState : State
{
    BuildController build;
    public ConstructState(CharacterController controller, BuildController build) : base(controller)
    {
        this.build = build;
    }
    public override void Update()
    {
        if (build.State == BuildController.StateBuild.CONSTRUCT)
            build.ConstructValue -= 0;
    }
    public override void Exit()
    {

    }
    public override void Destroy(BuildController build)
    {
        if (build != this.build) controller.state = new GoToInteract(controller, CharacterController.Interact.DESTRUCT, build);
        else controller.state = new DestroyState(controller, build);
    }
    public override void Create(BuildController build)
    {
        if (build != this.build) controller.state = new GoToInteract(controller, CharacterController.Interact.CONSTRUCT, build);
        
    }
    public override void Interact(BuildController build)
    {
        if (build != this.build) controller.state = new GoToInteract(controller, CharacterController.Interact.INTERACT, build);
        //else build.Interact(controller);
    }
}
