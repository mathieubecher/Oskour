using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

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
    private List<Light> _lights;
    private void Awake()
    {
        _materials = new List<MaterialGestor>();
        _lights = new List<Light>();
        RecursiveMaterials(gameObject);
    }

    private void RecursiveMaterials(GameObject child)
    {
        if(child.TryGetComponent<Renderer>(out Renderer childRenderer)) _materials.Add(new MaterialGestor(childRenderer, childRenderer.materials));
        if(child.TryGetComponent<Light>(out Light childLight)) _lights.Add(childLight);
        for (int i = 0; i < child.transform.childCount; ++i)
        {
            RecursiveMaterials(child.transform.GetChild(i).gameObject);
        }
    }

    public void ResetMaterial()
    {
        foreach (MaterialGestor materialGestor in _materials)
        {
            materialGestor.Renderer.materials = materialGestor.BaseMaterial;
            materialGestor.Renderer.shadowCastingMode = ShadowCastingMode.On;
        }

        foreach (Light lightGestor in _lights) lightGestor.enabled = true;
    }

    public void SetMaterial(Material material)
    {
        foreach (MaterialGestor materialGestor in _materials)
        {
            materialGestor.Renderer.material = material;
            materialGestor.Renderer.shadowCastingMode = ShadowCastingMode.Off;
        }
        foreach (Light lightGestor in _lights) lightGestor.enabled = false;
    }

    private void Update()
    {
        
    }
}
