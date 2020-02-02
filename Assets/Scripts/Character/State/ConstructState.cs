using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructState : State
{
    BuildController build;
    public ConstructState(CharacterController controller, BuildController build) : base(controller)
    {
        this.build = build;
        controller.stateInfo = "Construct";
        controller.IA.isStopped = true;
    }
    public override void Update()
    {
        if (build != null && build.State == BuildController.StateBuild.CONSTRUCT)
            build.ConstructValue += Time.deltaTime * controller.manager.constructSpeed / controller.manager.timeScale;
        else Iddle();
    }
    public override void Exit()
    {

    }
    public override void Destruct(BuildController build)
    {
        if (build != this.build) controller.state = new GoToInteract(controller, CharacterController.Interact.DESTRUCT, build);
        else controller.state = new DestroyState(controller, build);
    }
    public override void Construct(BuildController build)
    {
        if (build != this.build) controller.state = new GoToInteract(controller, CharacterController.Interact.CONSTRUCT, build);
    }
    public override void Interact(BuildController build)
    {
        if (build != this.build) controller.state = new GoToInteract(controller, CharacterController.Interact.INTERACT, build);
        //else build.Interact(controller);
    }
}
