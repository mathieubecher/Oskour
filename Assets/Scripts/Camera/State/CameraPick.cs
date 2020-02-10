using UnityEngine;

public class CameraPick : CameraState
{
    private Vector3 _startSelectionDrag;
    private bool _select;
    public CameraPick(CameraPointer controller) : base(controller)
    {
        
    }
    public override void Click()
    {
        Ray ray = controller.mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray: ray,hitInfo: out RaycastHit hit, layerMask: LayerMask.GetMask("Character"), maxDistance: 1000))
        {
            if (!Input.GetKey(KeyCode.LeftShift)) controller.manager.ResetSelect();
            Debug.Log(hit.collider.gameObject.layer);
            controller.manager.Select(hit.collider.transform.parent.GetComponent<CharacterController>());
            _select = false;
        }
        else
        {
            _startSelectionDrag = Input.mousePosition;
            _select = true;
        }
    }
    public override void Release()
    {
        _select = false;
    }
    public override void Hover()
    {
        if (_select)
            SelectCharacter(!Input.GetKey(KeyCode.LeftShift));
    }
    public override void RightClick()
    {
        Ray ray = controller.mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit)) return;
        switch (hit.collider.gameObject.layer)
        {
            case 0:
            {
                Debug.DrawLine(hit.point, hit.point + hit.normal * 10, Color.red, 10);
                foreach (CharacterController character in controller.manager.Selected)
                {
                    character.GoTo(hit.point);
                }

                break;
            }
            case 10:
            {
                var i = 0;
                while (i < controller.manager.Selected.Count)
                {
                    CharacterController character = controller.manager.Selected[i];
                    character.GoToInteract(hit.collider.gameObject.GetComponent<BuildController>());
                    if (character.Select) ++i;
                    else controller.manager.Selected.Remove(character);
                }

                break;
            }
            default : break;
        }
    }

    private void SelectCharacter(bool resetSelect = true)
    {
        if (resetSelect) controller.manager.ResetSelect();

        Rect selectionBox = new Rect(Mathf.Min(_startSelectionDrag.x, Input.mousePosition.x), Mathf.Min(_startSelectionDrag.y, Input.mousePosition.y),
            Mathf.Abs(_startSelectionDrag.x - Input.mousePosition.x),
            Mathf.Abs(_startSelectionDrag.y - Input.mousePosition.y));
        foreach (CharacterController character in controller.manager.Characters)
        {
            if (selectionBox.Contains(controller.mainCamera.WorldToScreenPoint(character.transform.position)))
            {
                controller.manager.Select(character);
            }
        }
    }
}

public class RayCastHit
{
}
