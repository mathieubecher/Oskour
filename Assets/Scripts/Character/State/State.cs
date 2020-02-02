using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected CharacterController controller;
    public State(CharacterController controller)
    {
        this.controller = controller;
    }
    public abstract void Update();
    public abstract void Exit();
    public virtual void Destroy(BuildController build) { controller.state = new GoToInteract(controller, CharacterController.Interact.DESTRUCT, build); }
    public virtual void Create(BuildController build) { controller.state = new GoToInteract(controller, CharacterController.Interact.CONSTRUCT, build); }
    public virtual void Iddle() { controller.state = new IddleState(controller); }
    public virtual void Interact(BuildController build) { controller.state = new GoToInteract(controller, CharacterController.Interact.INTERACT, build); }

}
