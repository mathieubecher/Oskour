using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointer : MonoBehaviour
{
    Vector2 startSelectionDrag;
    public Material materialred;
    public GameObject[] characters;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if(Vector3.Angle(hit.normal,Vector3.up ) < 10)
                    Debug.DrawLine(hit.point,hit.point +hit.normal * 10, Color.red, 10);
                
            }
            startSelectionDrag = Input.mousePosition;

        }
        else if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.DrawLine(hit.point, hit.point + hit.normal * 10, Color.blue, Time.deltaTime);
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Rect selectionBox = new Rect(Mathf.Min(startSelectionDrag.x, Input.mousePosition.x), Mathf.Min(startSelectionDrag.y, Input.mousePosition.y),
            Mathf.Abs(startSelectionDrag.x - Input.mousePosition.x),
            Mathf.Abs(startSelectionDrag.y - Input.mousePosition.y));
            foreach(GameObject character in characters)
            {
                if (selectionBox.Contains(Camera.main.WorldToScreenPoint(character.transform.position)))
                {
                    character.GetComponent<Renderer>().material = materialred;
                }
            }
        }
    }
   
}
