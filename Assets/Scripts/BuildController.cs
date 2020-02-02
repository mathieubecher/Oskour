using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
public class BuildController : MonoBehaviour
{
    public enum StateBuild
    {
        PLACING, CONSTRUCT, ACTIF

    }
    public enum BuildType
    {
        FERME, DORTOIRE, ENTREPOT, BRAS, OXYGENATEUR, PHARE, KAFE, PIKOUZ, YELLOW 
    }
    enum Resources
    {
        ENERGY, OXYGEN, FOOD
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
    public BuildType type;
    [SerializeField, Range(0, 1)]
    private float constructValue = 0;

    [Header("Interactible")]
    public bool interactible = false;
    public List<CharacterController> characters;
    public int maxCharacter = 0;
    [SerializeField]
    private Resources resourceType;
    [Range(0, 2)]
    public float bonusTime;

    [Header("Require")]
    public BuildType[] requires;

    

    public float ConstructValue { get => constructValue; set{
        constructValue = value;
            if (value <= 0)
            {
                Destruct();
                FindObjectOfType<GameManager>().resources += 1;
            }
            else if (value >= 1)
            {
                state = StateBuild.ACTIF;
            }
            else state = StateBuild.CONSTRUCT;
    } }

    public StateBuild State { get => state;}

    private GameManager manager;
    [HideInInspector]
    public bool constructible;
    // Start is called before the first frame update
    void Start()
    {
        manager =  FindObjectOfType<GameManager>();
        colliders = new List<Collider>();
        renderer = transform.GetChild(0).GetComponent<Renderer>();
    }

    void Constructible()
    {
        if (colliders.Count > 0)
        {
            constructible = false;
        }
        else
        {
            List<BuildType> requirestolist = new List<BuildType>();
            for (int i = 0; i < requires.Length; ++i) requirestolist.Add(requires[i]);
            foreach(BuildController build in manager.listBuild)
            {
                if((transform.position - build.transform.position).magnitude <= manager.rangeBuildEffect && requirestolist.Exists(x =>x == build.type))
                {
                    requirestolist.Remove(build.type);
                }
            }
            constructible = requirestolist.Count == 0;

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (state == StateBuild.PLACING) Constructible();
        if (state == StateBuild.PLACING || state == StateBuild.CONSTRUCT)
        {
            if (!constructible && renderer.material != error) renderer.material = error;
            else if ( renderer.material != placing) renderer.material = placing;
        }
        else if(state == StateBuild.ACTIF && renderer.material != basic) renderer.material = basic;

        if(state == StateBuild.ACTIF)
        {
            float value = bonusTime;
            foreach(CharacterController character in characters)
            {
                value -= bonusTime / (float)maxCharacter;
                if (value > 0)
                {
                    if(resourceType == Resources.FOOD) AddResourcesCharacter(character, character.food, out character.food, bonusTime / (float)maxCharacter); 
                    else if(resourceType == Resources.ENERGY) AddResourcesCharacter(character, character.energy, out character.energy, bonusTime / (float)maxCharacter);
                    else AddResourcesCharacter(character, character.oxygen, out character.oxygen, bonusTime / (float)maxCharacter);
                }
            }
        }
    }
    private void AddResourcesCharacter(CharacterController character, float statEnter, out float stat, float value)
    {
        stat = Mathf.Min(1,statEnter + value);
        if (stat == 1)
        {
            characters.Remove(character);
            character.enabled = true;
            character.state.Iddle();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if(state == StateBuild.PLACING)
            colliders.Add(other);
        else if(other.gameObject.layer == 9)
        {
            
            other.transform.parent.GetComponent<CharacterController>().state.Collide(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (state == StateBuild.PLACING)
            colliders.Remove(other);
    }
    public void Construct()
    {
        transform.GetChild(0).GetComponent<NavMeshObstacle>().enabled = true;
        Destroy(GetComponent<Rigidbody>());
        state = StateBuild.CONSTRUCT;
        gameObject.layer = 10;
    }
    public void Destruct()
    {

        Destroy(this.gameObject);
    }

    public virtual string ConditionToString()
    {
        return "1 poisson";
    }

    public virtual void Interact(CharacterController character)
    {
        if(interactible && characters.Count < maxCharacter)
        {
            characters.Add(character);
            manager.Selected.Remove(character);
            character.enabled = false;
        }
        

    }
    
}
