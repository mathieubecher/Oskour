﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyState : IddleState
{
    BuildController build;
    public DestroyState(CharacterController controller, BuildController build) : base(controller)
    {
        this.build = build;
        controller.stateInfo = "Destruct";
        
        controller.IA.isStopped = true;
    }
    public override void Update()
    {
        if (build != null && (build.State == BuildController.StateBuild.CONSTRUCT || build.State == BuildController.StateBuild.ACTIF))
            build.ConstructValue -= Time.deltaTime * controller.manager.destroySpeed / controller.manager.timeScale;
        else Iddle();
    }
    public override void Exit()
    {

    }
    public override void Destruct(BuildController build) {
        if (build != this.build) controller.state = new GoToInteract(controller, CharacterController.Interact.DESTRUCT, build);
    }
    public override void Construct(BuildController build) {
        if (build != this.build) controller.state = new GoToInteract(controller, CharacterController.Interact.CONSTRUCT, build);
        else controller.state = new ConstructState(controller, build);
    }
    public override void Interact(BuildController build)
    {
        if (build != this.build) controller.state = new GoToInteract(controller, CharacterController.Interact.INTERACT, build);
        //else build.Interact(controller);
    }
}