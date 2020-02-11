using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructState : State
{
    private readonly BuildController _build;
    public ConstructState(CharacterController controller, BuildController build) : base(controller)
    {
        this._build = build;
        controller.stateInfo = "Construct";
        controller.IA.isStopped = true;
    }
    public override void Update()
    {
        if (_build == null || _build.Active() || !_build.Interact(false, controller))
        {
            Idle();
        }
        
    }
    public override void Exit()
    {

    }
    public override void Destruct(BuildController build)
    {
        if (build != this._build) controller.state = new GoToInteract(controller, CharacterController.Interact.DESTRUCT, build);
        else controller.state = new DestroyState(controller, build);
    }
    public override void Construct(BuildController build)
    {
        if (build != this._build) controller.state = new GoToInteract(controller, CharacterController.Interact.CONSTRUCT, build);
    }
    public override void Interact(BuildController build)
    {
        if (build != this._build) controller.state = new GoToInteract(controller, CharacterController.Interact.INTERACT, build);
        else build.Interact(false, controller);
    }
}
