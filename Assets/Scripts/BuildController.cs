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
        FERME, DORTOIRE, ENTREPOT, BRAS, OXYGENATEUR, PHARE, KAFE, PIKOUZ, YELLOW, ELECTRICITE
    }
    protected enum Resources
    {
        ENERGY, OXYGEN, FOOD
    }
    [SerializeField] Material placing;
    [SerializeField] Material error;
    [SerializeField] Material basic;

    protected Renderer renderer;
    protected StateBuild state;

    //[HideInInspector]
    public List<Collider> colliders;

    [Header("Description")]
    public string name;
    public string description;
    public int tier = 0;
    public BuildType type;
    [SerializeField, Range(0, 1)]
    protected float constructValue = 0;

    [Header("Interactible")]
    public bool interactible = false;
    [SerializeField]
    protected Resources resourceType;
    public float bonusTime;
    public List<CharacterController> characters;
    public int maxCharacter = 5;
    public bool entreposable = false;

    [Header("Require")]
    public BuildType[] requires;

    public bool alreadyCreate = false;

    public float ConstructValue { get => constructValue; set{
        constructValue = value;
            if (value <= 0)
            {
                Destruct();
                manager.resources += 1;
            }
            else if (value >= 1)
            {
                state = StateBuild.ACTIF;
                manager.listBuild.Add(this);
            }
            else state = StateBuild.CONSTRUCT;
    } }

    public StateBuild State { get => state;}

    protected GameManager manager;
    [HideInInspector]
    public bool constructible;
    // Start is called before the first frame update
    void Start()
    {
        characters = new List<CharacterController>();
        manager =  FindObjectOfType<GameManager>();
        colliders = new List<Collider>();
        renderer = transform.GetChild(0).GetComponent<Renderer>();
        if (alreadyCreate)
        {
            Construct();
            ConstructValue = 1;
        }
    }

    void Constructible()
    {
        if (colliders.Count > 0)
        {
            constructible = false;
        }
        else
        {
            constructible = true;

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (state == StateBuild.PLACING) Constructible();
        if (state == StateBuild.PLACING || state == StateBuild.CONSTRUCT)
        {
            //if (!constructible && renderer.material != error) renderer.material = error;
            //else if ( renderer.material != placing) renderer.material = placing;
        }
        //else if(state == StateBuild.ACTIF && renderer.material != basic) renderer.material = basic;

        if(state == StateBuild.ACTIF)
        {
            float value = bonusTime * Time.deltaTime;
            int i = 0;
            while (i < characters.Count)
            {
                CharacterController character = characters[i];
                value -= bonusTime / ((float)maxCharacter * manager.timeScale) * Time.deltaTime;
                bool deletecharacter = false;
                if (value > 0)
                {
                    
                    if (resourceType == Resources.FOOD) deletecharacter = AddResourcesCharacter(character, character.food, out character.food, bonusTime / ((float)maxCharacter * manager.timeScale) * Time.deltaTime);
                    else if (resourceType == Resources.ENERGY) deletecharacter = AddResourcesCharacter(character, character.energy, out character.energy, bonusTime / ((float)maxCharacter * manager.timeScale) * Time.deltaTime);
                    else deletecharacter = AddResourcesCharacter(character, character.oxygen, out character.oxygen, bonusTime / ((float)maxCharacter * manager.timeScale) * Time.deltaTime);
                    
                }
                if (!deletecharacter) ++i;
            }
            if(value > 0 && entreposable)
            {
                i = 0;
                List<Entrepot> entrepots = new List<Entrepot>();
                while (i < manager.listBuild.Count)
                {
                    if(manager.listBuild[i].type == BuildType.ENTREPOT)
                    {
                        entrepots.Add((Entrepot)manager.listBuild[i]);
                    }
                }
                if(entrepots.Count > 0)
                {
                    value = value / entrepots.Count;
                    foreach (Entrepot entrepot in entrepots)
                    {
                        if(resourceType == Resources.FOOD)
                            entrepot.stockFood += value;
                        if (resourceType == Resources.ENERGY)
                            entrepot.stockCoffee += value;
                    }
                    foreach (Entrepot entrepot in entrepots) entrepot.stockFood += value;
                }
                
            }
        }
    }
    private bool AddResourcesCharacter(CharacterController character, float statEnter, out float stat, float value)
    {
        stat = Mathf.Min(1,statEnter + value);
        if (stat == 1)
        {
            characters.Remove(character);
            character.gameObject.SetActive(true);
            character.state.Iddle();
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if(state == StateBuild.PLACING)
            colliders.Add(other);

    }
    private void OnTriggerExit(Collider other)
    {
        if (state == StateBuild.PLACING)
            colliders.Remove(other);
    }
    public void Construct()
    {
        
        transform.GetChild(0).GetComponent<NavMeshObstacle>().enabled = true;
        transform.GetChild(0).GetComponent<Collider>().enabled = true;
        Destroy(GetComponent<Rigidbody>());
        state = StateBuild.CONSTRUCT;
        gameObject.layer = 10;
    }
    public void Destruct()
    {
        Debug.Log("Destroy");
        manager.listBuild.Remove(this);
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
            character.Select = false;
            character.gameObject.SetActive(false);
        }
        

    }
    
}
