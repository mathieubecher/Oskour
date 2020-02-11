using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MaterialsGestor : MonoBehaviour
{
    public struct MaterialGestor
    {
        public Renderer Renderer;
        public Material BaseMaterial;

        public MaterialGestor(Renderer renderer, Material baseMaterial)
        {
            this.Renderer = renderer;
            this.BaseMaterial = baseMaterial;
        }
    }

    private List<MaterialGestor> _materials;
    private void Awake()
    {
        _materials = new List<MaterialGestor>();
        RecursiveMaterials(gameObject);
    }

    private void RecursiveMaterials(GameObject child)
    {
        if(child.TryGetComponent<Renderer>(out Renderer childRenderer)) _materials.Add(new MaterialGestor(childRenderer, childRenderer.material));
        for (int i = 0; i < child.transform.childCount; ++i)
        {
            RecursiveMaterials(child.transform.GetChild(i).gameObject);
        }
    }

    public void ResetMaterial()
    {
        foreach (MaterialGestor materialGestor in _materials)
        {
            materialGestor.Renderer.material = materialGestor.BaseMaterial;
        }
    }

    public void SetMaterial(Material material)
    {
        foreach (MaterialGestor materialGestor in _materials)
        {
            materialGestor.Renderer.material = material;
        }
    }

    private void Update()
    {
        
    }
}
