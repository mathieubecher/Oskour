using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointer : MonoBehaviour
{
    Vector2 startSelectionDrag;
    GameManager manager;
    Camera mainCamera;
    StatePointer state;
    bool select = false;
    enum StatePointer
    {
        PICK,
        CONSTRUCT
    }
    // Start is called before the first frame update
    void Start()
    {
        manager = (GameManager)FindObjectOfType<GameManager>();
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == StatePointer.PICK) Pick();
        else Construct();

    }

    void Pick()
    {
        // PICK
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject.layer == 9)
            {
                if (!Input.GetKey(KeyCode.LeftShift)) manager.ResetSelect();
                manager.Select(hit.collider.transform.parent.GetComponent<CharacterController>());
                select = false;
            }

            else
            {
                startSelectionDrag = Input.mousePosition;
                select = true;
            }
            

        }
        else if (Input.GetMouseButton(0) && select)
        {
            SelectCharacter(!Input.GetKey(KeyCode.LeftShift));
        }
    }

    void SelectCharacter(bool resetSelect = true)
    {
        if (resetSelect) manager.ResetSelect();

        Rect selectionBox = new Rect(Mathf.Min(startSelectionDrag.x, Input.mousePosition.x), Mathf.Min(startSelectionDrag.y, Input.mousePosition.y),
           Mathf.Abs(startSelectionDrag.x - Input.mousePosition.x),
           Mathf.Abs(startSelectionDrag.y - Input.mousePosition.y));
        foreach (CharacterController character in manager.Characters)
        {
            if (selectionBox.Contains(mainCamera.WorldToScreenPoint(character.transform.position)))
            {
                manager.Select(character);
            }
        }
    }
    void Construct()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.layer == 0 && Vector3.Angle(hit.normal, Vector3.up) < 10)
            {
                // CONSTRUCT
                Debug.DrawLine(hit.point, hit.point + hit.normal * 10, Color.red, 10);
            }

        }
    }
}
