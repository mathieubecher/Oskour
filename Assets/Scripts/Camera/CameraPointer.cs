﻿using System.Collections.Generic;
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
        state = new Pick(this);
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
        var requiresToList = build.requires.ToList();

        foreach (var presentBuild in manager.listBuild)
        {
            requiresToList.Remove(presentBuild.type);
        }
        if(requiresToList.Count == 0)
        {
            var toBuild = Instantiate(build, Vector3.zero, Quaternion.identity);
            state.ConstructState(toBuild);
        }
        else
        {
            //TODO Feedback
        }
    }

    public void DestroyBuilding()
    {
        
    }
}
