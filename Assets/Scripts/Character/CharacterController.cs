using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    bool select;
    [SerializeField]
    Material selected;
    [SerializeField]
    Material unselect;

    GameManager manager;
    NavMeshAgent IA;
    public bool Select { get => select; set => select = value; }

    // Start is called before the first frame update
    void Start()
    {
        manager = (GameManager)FindObjectOfType<GameManager>();
        IA = GetComponent<NavMeshAgent>();

        
    }

    // Update is called once per frame
    void Update()
    {
        IA.speed = manager.characterSpeed;
        
        if (select) transform.GetChild(0).GetComponent<Renderer>().material = selected;
        else transform.GetChild(0).GetComponent<Renderer>().material = unselect;
    }

    public void GoTo(Vector3 point)
    {
        IA.destination = point;
    }
}
