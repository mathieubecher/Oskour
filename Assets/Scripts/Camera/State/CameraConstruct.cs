using UnityEngine;


public class CameraConstruct : CameraState
{
    private readonly BuildController _build;
    public CameraConstruct(CameraPointer controller, BuildController build) : base(controller)
    {
        this._build = build;
    }
    public override void Click()
    {
        if(_build.Construct(0)) {
            controller.state = new CameraPick(controller);
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
        Ray ray = controller.mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit, layerMask: LayerMask.GetMask("Default"),
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
        controller.state = new CameraPick(controller);
    }
    public override void ConstructState(BuildController build)
    {
        Object.Destroy(_build.gameObject);
        controller.state = new CameraConstruct(controller, build);
    }
}

