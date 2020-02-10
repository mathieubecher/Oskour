using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using UnityEngine.Serialization;

public class BuildController : MonoBehaviour
{
    public enum StateBuild
    {
        PLACING, CONSTRUCT, ACTIF

    }
    public enum BuildType
    {
        FERME, DORTOIRE, ENTREPOT, BRAS, OXYGENATEUR, PHARE, KAFE, PIKOUZ, YELLOW, ELECTRICITE, RUINE
    }
    protected enum Resources
    {
        ENERGY, OXYGEN, FOOD
    }
    [SerializeField] private Material placing;
    [SerializeField] private Material error;
    [SerializeField] private Material basic;

    private new Renderer renderer;
    private StateBuild state;

    //[HideInInspector]
    public List<Collider> colliders;

    [Header("Description")]
    public new string name;
    public string description;
    public int tier;
    public BuildType type;
    [SerializeField, Range(0, 1)]
    protected float constructValue = 0;

    [Header("Interactive")]
    public bool interactive = false;
    [SerializeField]
    protected Resources resourceType;
    public float bonusTime;
    public List<CharacterController> characters;
    public int maxCharacter = 5;
    public bool storable = false;

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

    public StateBuild State => state;

    protected GameManager manager;
    [HideInInspector]
    public bool constructible;
    // Start is called before the first frame update
    private void Start()
    {
        characters = new List<CharacterController>();
        manager =  FindObjectOfType<GameManager>();
        colliders = new List<Collider>();
        renderer = transform.GetChild(0).GetComponent<Renderer>();
        if (!alreadyCreate) return;
        Construct();
        ConstructValue = 1;
    }

    private void Constructable()
    {
        constructible = colliders.Count <= 0;
    }
    // Update is called once per frame
    private void Update()
    {
        if (state == StateBuild.PLACING) Constructable();
        
    }
    protected virtual void ActiveComportment()
    {
        float value = bonusTime * Time.deltaTime;
        int i = 0;
        while (i < characters.Count)
        {
            CharacterController character = characters[i];
            value -= bonusTime / (maxCharacter * manager.timeScale) * Time.deltaTime;
            bool deleteCharacter = false;
            if (value > 0)
            {
                switch (resourceType)
                {
                    case Resources.FOOD:
                        deleteCharacter = AddResourcesCharacter(character, character.food, out character.food, bonusTime / (maxCharacter * manager.timeScale) * Time.deltaTime);
                        break;
                    case Resources.ENERGY:
                        deleteCharacter = AddResourcesCharacter(character, character.energy, out character.energy, bonusTime / (maxCharacter * manager.timeScale) * Time.deltaTime);
                        break;
                    case Resources.OXYGEN:
                        deleteCharacter = AddResourcesCharacter(character, character.oxygen, out character.oxygen, bonusTime / (maxCharacter * manager.timeScale) * Time.deltaTime);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            if (!deleteCharacter) ++i;
        }

        if (!(value > 0) || !storable) return;
        i = 0;
        List<Entrepot> entrepots = new List<Entrepot>();
        while (i < manager.listBuild.Count)
        {
            if (manager.listBuild[i].type == BuildType.ENTREPOT)
            {
                entrepots.Add((Entrepot)manager.listBuild[i]);
            }
        }

        if (entrepots.Count <= 0) return;
        value /= entrepots.Count;
        foreach (Entrepot entrepot in entrepots)
        {
            switch (resourceType)
            {
                case Resources.FOOD:
                    entrepot.stockFood += value;
                    break;
                case Resources.ENERGY:
                    entrepot.stockCoffee += value;
                    break;
                case Resources.OXYGEN:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        foreach (Entrepot entrepot in entrepots) entrepot.stockFood += value;
    }
    protected bool AddResourcesCharacter(CharacterController character, float statEnter, out float stat, float value)
    {
        stat = Mathf.Min(1,statEnter + value);
        if (!(stat >= 1)) return false;
        characters.Remove(character);
        character.gameObject.SetActive(true);
        character.state.Iddle();
        return true;
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

    private void Destruct()
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
        if (!interactive || characters.Count >= maxCharacter) return;
        characters.Add(character);
        character.Select = false;
        character.gameObject.SetActive(false);


    }
    
}
