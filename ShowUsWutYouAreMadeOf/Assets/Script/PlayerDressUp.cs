using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDressUp : MonoBehaviour
{
    //keep track of the item that player is wearing

    //current 

    //store all of the found items

    //method to add item to the wardrobe list
    
    // available colors
    public List<Customization> Customizations;
    int _currentCustomizationIndex;
    public Customization CurrentCustomization { get; private set; }

    void Awake()
    {
        foreach (var customization in Customizations)
        {
            customization.UpdateRenderers();
            customization.UpdateSubObjects();
        }
    }

    void Update()
    {
        SelectCustomizationWithUpDownArrows();

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CurrentCustomization.NextMaterial();
            CurrentCustomization.NextSubObject();
        }
    }

    void SelectCustomizationWithUpDownArrows()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            _currentCustomizationIndex++;
        if (Input.GetKeyDown(KeyCode.UpArrow))
            _currentCustomizationIndex--;
        if (_currentCustomizationIndex < 0)
            _currentCustomizationIndex = Customizations.Count - 1;
        if (_currentCustomizationIndex >= Customizations.Count)
            _currentCustomizationIndex = 0;
        CurrentCustomization = Customizations[_currentCustomizationIndex];
    }
}

[Serializable]
public class Customization
{
    public string DisplayName;
    
    public List<Renderer> Renderers;    //mesh and submesh
    public List<Material> Materials;    //material, texture, color
    public List<GameObject> SubObjects;
    int _materialIndex;
    int _subObjectIndex;

    public void NextMaterial()  //only change material for all the renderer mesh
    {
        _materialIndex++;
        if (_materialIndex >= Materials.Count)
            _materialIndex = 0;

        UpdateRenderers();
    }

    public void NextSubObject() //change the gameobject, e.g. hat, weapons. Have them as child of the character
    {
        _subObjectIndex++;
        if (_subObjectIndex >= SubObjects.Count)
            _subObjectIndex = 0;

        UpdateSubObjects();
    }

    public void UpdateSubObjects()  
    {
        for (var i = 0; i < SubObjects.Count; i++)
            if (SubObjects[i])
                SubObjects[i].SetActive(i == _subObjectIndex);
    }

    public void UpdateRenderers()
    {
        foreach (var renderer in Renderers)
            if (renderer)
                renderer.material = Materials[_materialIndex];
    }
}
