using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildController : MonoBehaviour
{
    enum State
    {
        PLACING, CONSTRUCT, ACTIF

    }
    [SerializeField] Material placing;
    [SerializeField] Material error;
    [SerializeField] Material basic;
    
    Renderer renderer;
    State state;
    List<Collider> colliders;

    public int tier = 0;
    public string name;
    public string description;

    // Start is called before the first frame update
    void Start()
    {
        colliders = new List<Collider>();
        renderer = transform.GetChild(0).GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.PLACING)
        {
            if (colliders.Count > 0 && renderer.material != error) renderer.material = error;
            else if ( renderer.material != placing) renderer.material = placing;
        }
        else if(renderer.material != basic) renderer.material = basic;
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
        state = State.CONSTRUCT;
        gameObject.layer = 10;
    }
    public virtual string ConditionToString()
    {
        return "1 poisson";
    }
}
