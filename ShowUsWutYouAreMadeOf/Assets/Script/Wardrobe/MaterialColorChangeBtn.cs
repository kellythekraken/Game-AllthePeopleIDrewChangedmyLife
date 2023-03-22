using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MaterialColorChangeBtn : MonoBehaviour
{
    //get the material name based on the type
    //onclick change to this color

    enum MaterialType {Hair, Skin, Eye}
    [SerializeField] MaterialType materialType;
    [SerializeField] Material newMaterial;
    Transform player;
    SkinnedMeshRenderer targetRenderer;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ChangeMaterialColor);
        player = GameManager.Instance.playerObject.transform;
        FindMaterial();
    }

    void FindMaterial()
    {
        switch(materialType)
        {
            case MaterialType.Hair:
                targetRenderer = player.Find("hair").GetComponent<SkinnedMeshRenderer>();
                return;
            case MaterialType.Skin:
                targetRenderer = player.Find("body").GetComponent<SkinnedMeshRenderer>();
                return;
            case MaterialType.Eye:
                targetRenderer = player.Find("eye").GetComponent<SkinnedMeshRenderer>();
                return;
        }
    }

    void ChangeMaterialColor()
    {
        switch(materialType)
        {
            case MaterialType.Hair:
                targetRenderer.sharedMaterial = newMaterial;
                return;
            case MaterialType.Skin:
                targetRenderer.sharedMaterial = newMaterial;
                return;
            case MaterialType.Eye:
                targetRenderer.materials[1].CopyPropertiesFromMaterial(newMaterial); 
                return;
        }
    }
}
