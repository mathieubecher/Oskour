using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MaterialsGestor : MonoBehaviour
{
    public struct MaterialGestor
    {
        public Renderer Renderer;
        public Material[] BaseMaterial;
        
        public MaterialGestor(Renderer renderer, Material[] baseMaterial)
        {
            this.Renderer = renderer;
            this.BaseMaterial = baseMaterial;
            
        }
    }
    
    private List<MaterialGestor> _materials;
    private BuildController _build;
    private List<Light> _lights;

    private static readonly int Texture = Shader.PropertyToID("_texture");
    private static readonly int Color = Shader.PropertyToID("_color");
    private static readonly int Progress = Shader.PropertyToID("_progress");

    private void Awake()
    {
        _materials = new List<MaterialGestor>();
        _lights = new List<Light>();
        LoadMaterialGestor(this.gameObject);
        _build = GetComponent<BuildController>();
        Debug.Log("load Material");

    }

    private void LoadMaterialGestor(GameObject child)
    {
        if (child.TryGetComponent<Renderer>(out Renderer childRenderer))
        {
            _materials.Add(new MaterialGestor(childRenderer, childRenderer.materials));
        }
        if(child.TryGetComponent<Light>(out Light childLight)) _lights.Add(childLight);
        for (int i = 0; i < child.transform.childCount; ++i)
        {
            LoadMaterialGestor(child.transform.GetChild(i).gameObject);
            
        }
    }

    private void Update()
    {
        
    }

    public void SetProgress(float progress)
    {
        
        foreach (MaterialGestor gestor in _materials)
        {
            foreach (Material mat in gestor.BaseMaterial)
            {
                mat.SetFloat("_progress",progress *  _build.height + transform.position.y);
            }
        }
    }
    public void SetExit(float progress)
    {
        foreach (MaterialGestor gestor in _materials)
        {
            foreach (Material mat in gestor.BaseMaterial)
            {
                mat.SetFloat("_planProgress",progress);
            }
        }
    }

    public void SetLight(bool active)
    {
        foreach (Light light in _lights)
        {
            light.enabled = true;
        }
    }    
    
    #region Inspector
#if UNITY_EDITOR
    private Material construct;

    private void Reset()
    {
        construct = AssetDatabase.LoadAssetAtPath<Material>("Assets/Resources/Building/BuildModel/placing.mat");
        RecursiveMaterials(gameObject);
    }

    private void RecursiveMaterials(GameObject child)
    {
        
        if (child.TryGetComponent<Renderer>(out Renderer childRenderer))
        {
            bool defaultLit = false;
            int j = 0;
            while (j < childRenderer.sharedMaterials.Length && !defaultLit)
            {
                string shaderName = childRenderer.sharedMaterials[j].shader.name;
                defaultLit |= shaderName == "HDRP/Lit" || shaderName == "Shader Graphs/Lit" ;
                ++j;
            }
            if(defaultLit){
                Material[] mats = new Material[childRenderer.sharedMaterials.Length-1];
                for (int i = 0; i < mats.Length; ++i)
                {
                    if (childRenderer.sharedMaterials[i].shader.name == "HDRP/Lit"){
                        Material mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/material/Construct/"+ childRenderer.sharedMaterials[i].name +"_Lit.mat");
                        if (mat == null)
                        {
                            mat = new Material(Shader.Find("Shader Graphs/Lit"));
                            AssetDatabase.CreateAsset(mat, "Assets/material/Construct/"+ childRenderer.sharedMaterials[i].name +"_Lit.mat");
                            mat.SetTexture(Texture,childRenderer.sharedMaterials[i].mainTexture);
                            mat.SetColor(Color,childRenderer.sharedMaterials[i].color);
                            mat.SetFloat(Progress,100);

                        }
                        
                        mats[i] = mat;
                    }
                    else
                    {
                        mats[i] = childRenderer.sharedMaterials[i];
                    }
                }
                childRenderer.sharedMaterials = mats;
            }
            //_materials.Add(new MaterialGestor(childRenderer, childRenderer.sharedMaterials));
        }
        
        //if(child.TryGetComponent<Light>(out Light childLight)) _lights.Add(childLight);
        for (int i = 0; i < child.transform.childCount; ++i)
        {
            RecursiveMaterials(child.transform.GetChild(i).gameObject);
            
        }
    }
    #endif
    #endregion
}
