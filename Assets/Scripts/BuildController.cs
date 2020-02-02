using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildController : MonoBehaviour
{
    public enum StateBuild
    {
        PLACING, CONSTRUCT, ACTIF

    }
    [SerializeField] Material placing;
    [SerializeField] Material error;
    [SerializeField] Material basic;

    Renderer renderer;
    StateBuild state;

    [HideInInspector]
    public List<Collider> colliders;

    [Header("Description")]
    public string name;
    public string description;
    public int tier = 0;

    [SerializeField, Range(0, 1)]
    private float constructValue = 0;

    public float ConstructValue { get => constructValue; set{
        constructValue = value;
            if (value <= 0)
            {
                Destroy(this.gameObject);
                FindObjectOfType<GameManager>().resources += 1;
            }
            else if(value >= 1)
            {
                state = StateBuild.ACTIF;
            }
    } }

    public StateBuild State { get => state;}



    // Start is called before the first frame update
    void Start()
    {
        colliders = new List<Collider>();
        renderer = transform.GetChild(0).GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == StateBuild.PLACING)
        {
            if (colliders.Count > 0 && renderer.material != error) renderer.material = error;
            else if ( renderer.material != placing) renderer.material = placing;
        }
        else if(state == StateBuild.ACTIF && renderer.material != basic) renderer.material = basic;
    }

    private void OnTriggerEnter(Collider other)
    {   
        colliders.Add(other);
    }
    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
    }
    public void Construct()
    {
        transform.GetChild(0).GetComponent<NavMeshObstacle>().enabled = true;
        GetComponent<Collider>().isTrigger = false;
        state = StateBuild.CONSTRUCT;
        gameObject.layer = 10;
    }

    public virtual string ConditionToString()
    {
        return "1 poisson";
    }
    
}
