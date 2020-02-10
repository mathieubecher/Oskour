using UnityEngine;


public class Construct : CameraState
{
    readonly BuildController _build;
    public Construct(CameraPointer controller, BuildController build) : base(controller)
    {
        this._build = build;
        Debug.Log("Construct");
    }
    public override void Click()
    {
        if(_build.colliders.Count == 0) { 
            _build.Construct();
            controller.state = new Pick(controller);
        }
        else
        {
            //TODO Feedback
        }


    }
    public override void Release()
    {

    }
    public override void Hover()
    { 
        var ray = controller.mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray: ray, hitInfo: out var hit, layerMask: LayerMask.GetMask("Default"),
            maxDistance: 1000)) return;
      
        if (Vector3.Angle(hit.normal, Vector3.up) < 10)
        {
           
            _build.transform.position = hit.point;
        }
    }
    public override void RightClick()
    {
        PickState();
    }
    public override void PickState()
    {
        Object.Destroy(_build.gameObject);
        controller.state = new Pick(controller);
    }
    public override void ConstructState(BuildController build)
    {
        Object.Destroy(_build.gameObject);
        controller.state = new Construct(controller, build);
    }
}

