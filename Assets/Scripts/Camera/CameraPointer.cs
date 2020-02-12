using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraPointer : MonoBehaviour
{

    [HideInInspector]
    public GameManager manager;
    [HideInInspector]
    public UnityEngine.Camera mainCamera;
    [HideInInspector]
    public CameraState state;

    private BuildController _toBuild;
    private Vector2 _startSelectionDrag;

    // Start is called before the first frame update
    private void Start()
    {
        state = new CameraPick(this);
        manager = (GameManager)FindObjectOfType<GameManager>();
        mainCamera = GetComponent<UnityEngine.Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) state.Click();
        else if (Input.GetMouseButtonUp(0)) state.Release();
        else if (Input.GetMouseButtonDown(1)) state.RightClick();
        else state.Hover();
        if(Input.GetKeyDown(KeyCode.G)) PlaceBuilding(manager.Build[4]);

    }

    public void PlaceBuilding(BuildController build)
    {
        if (manager.resources > 0)
        {
            List<BuildController.BuildType> requiresToList = build.requires.ToList();

            foreach (BuildController presentBuild in manager.listBuild)
            {
                if(presentBuild.Active()) requiresToList.Remove(presentBuild.type);
            }

            if (requiresToList.Count == 0)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                Vector3 pos;
                if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit, layerMask: LayerMask.GetMask("Default"),
                    maxDistance: 1000)) pos = hit.point;
                else pos = Vector3.zero;
                BuildController toBuild = Instantiate(build, pos, Quaternion.identity);
                state.ConstructState(toBuild);
            }
            else
            {
                //TODO Feedback build require
            }
        }
        else
        {
            //TODO Feedback no resources
        }
    }

    public void DestroyBuilding()
    {
        
    }
}

