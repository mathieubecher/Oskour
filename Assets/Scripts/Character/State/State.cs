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
    public virtual void Collide(BuildController build) { }
    public virtual void Destruct(BuildController build) { controller.state = new GoToInteract(controller, CharacterController.Interact.DESTRUCT, build); Debug.Log("Destruct"); }
    public virtual void Construct(BuildController build) { controller.state = new GoToInteract(controller, CharacterController.Interact.CONSTRUCT, build); Debug.Log("Construct"); }
    public virtual void Iddle() { controller.state = new IddleState(controller); }
    public virtual void GoTo(Vector3 target) { controller.state = new GoToState(controller,target); }
    public virtual void Interact(BuildController build) { controller.state = new GoToInteract(controller, CharacterController.Interact.INTERACT, build); Debug.Log("Interact"); }

}
