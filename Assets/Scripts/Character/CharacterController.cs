using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    bool select;

    public enum Interact
    {
        INTERACT, CONSTRUCT, DESTRUCT
    }
    [HideInInspector]
    public GameManager manager;
    [HideInInspector]
    public NavMeshAgent IA;
    [HideInInspector]
    public State state;

    public bool Select { get => select; set => select = value; }

    [Header("Information")]
    public string name;
    public string stateInfo = "Iddle";
    [Header("Statistique")]
    [Range(0,1)]
    public float food = 1;
    [Range(0, 1)]
    public float oxygen = 1;
    [Range(0, 1)]
    public float energy = 1;
    public float beginTimer = 60;

    [Header("Material")]
    [SerializeField]
    Material selected;
    [SerializeField]
    Material unselect;

    // Start is called before the first frame update
    void Start()
    {
        manager = (GameManager)FindObjectOfType<GameManager>();
        IA = GetComponent<NavMeshAgent>();
        state = new IddleState(this);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        IA.speed = manager.characterSpeed;
        
        if (select) transform.GetChild(0).GetComponent<Renderer>().material = selected;
        else transform.GetChild(0).GetComponent<Renderer>().material = unselect;

        state.Update();

        if(beginTimer >= 0) beginTimer -= Time.deltaTime;
        else { 
            food = Mathf.Max(0,food - manager.foodRegress / manager.timeScale * Time.deltaTime);
            oxygen = Mathf.Max(0, oxygen - manager.oxygenRegress / manager.timeScale * Time.deltaTime);
            energy = Mathf.Max(0, energy - manager.energyRegress / manager.timeScale * Time.deltaTime);
        }
    }

    public void GoTo(Vector3 point)
    {
        state.GoTo(point);
    }
    public void GoToInteract(BuildController build)
    {
        if (Input.GetKey(KeyCode.LeftControl)) state.Destruct(build);
        else if (build.State == BuildController.StateBuild.ACTIF)
            state.Interact(build);
        else state.Construct(build);
    }
}
