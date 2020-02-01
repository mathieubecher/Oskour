using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private CharacterController[] characters;
    public CharacterController[] Characters { get => characters; set => characters = value; }

    private List<CharacterController> selected;
    public List<CharacterController> Selected { get => selected; set => selected = value; }

    

    // Start is called before the first frame update
    void Start()
    {
        Selected = new List<CharacterController>();
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
        
    }

}
