using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Characters Settings")]
    [SerializeField]
    private CharacterController[] characters;
    [Range(0, 100)]
    public float characterSpeed;
    private List<CharacterController> selected;

    [Header("Resources Timer")]
    public float foodRegress = 0.75f;
    public float oxygenRegress = 0.4f;
    public float energyRegress = 0.35f;
    public float timeScale = 20f;

    [Header("Build Settings")]
    [SerializeField]
    private BuildController[] build;

    public CharacterController[] Characters { get => characters; set => characters = value; }
    public List<CharacterController> Selected { get => selected; set => selected = value; }
    public BuildController[] Build { get => build; set => build = value; }

    CameraPointer pointer;


    // Start is called before the first frame update
    void Start()
    {
        Selected = new List<CharacterController>();
        pointer = Camera.main.GetComponent<CameraPointer>();
       
    }

    public void Select(CharacterController character)
    {
        character.Select = true;
        selected.Add(character);

    }
    public void ResetSelect()
    {
        foreach(CharacterController character in selected)
        {
            character.Select = false;
        }
        selected = new List<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G)) pointer.PlaceBuilding(build[0]);
    }
    public void PlaceBuilding(BuildController build)
    {
        pointer.PlaceBuilding(build);
    }
}
