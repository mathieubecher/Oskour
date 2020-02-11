using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToInteract : GoToState
{
    private readonly BuildController _build;
    private readonly CharacterController.Interact _state;
    public GoToInteract(CharacterController controller, CharacterController.Interact state, BuildController build) : base(controller,build.transform.position)
    {
        controller.stateInfo = "GoToInteract";
        this._state = state;
        this._build = build;
    }
    public override void Update()
    {
        base.Update();
        Transform transform = controller.transform;
        Vector3 position = transform.position;
        Quaternion localRotation = transform.localRotation;
        
        Ray ray = new Ray(position,(localRotation * Vector3.forward).normalized * 10);
        if (!Physics.Raycast(ray, out RaycastHit hit, 10, LayerMask.GetMask("Building"))) return;
        if(hit.collider.transform.parent.gameObject.GetComponent<BuildController>() == _build)
        {
            Exit();
        }
    }
    public override void Exit()
    {
        switch (_state)
        {
            case CharacterController.Interact.CONSTRUCT:
                controller.state = new ConstructState(controller, _build);
                break;
            case CharacterController.Interact.DESTRUCT:
                controller.state = new DestroyState(controller, _build);
                break;
            default:
                _build.Interact(false, controller);
                break;
        }
    }
    public override void Collide(BuildController build) {
        if (build == this._build)
        {
            Exit();
        }
    }
}
