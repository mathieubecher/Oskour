using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    bool select;
    [SerializeField]
    Material selected;
    [SerializeField]
    Material unselect;
    public bool Select { get => select; set => select = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (select) transform.GetChild(0).GetComponent<Renderer>().material = selected;
        else transform.GetChild(0).GetComponent<Renderer>().material = unselect;
    }
}
