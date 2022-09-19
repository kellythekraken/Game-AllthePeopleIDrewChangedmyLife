using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public class WardrobeSection
{
    public string DisplayName;
    public ItemSection sectionName;
    public SkinnedMeshRenderer renderer;   //renderer to replace the materials, maybe a list if there're more renderer?
    public Mesh defaultMesh;
    public List<GiftItem> defaultItems;
    int _materialIndex;
    int _subObjectIndex;
}

public class WardrobeManager : MonoBehaviour
{
    //only manage things within the wardrobe!
    //store all of the found items

    // available colors
    public static WardrobeManager Instance;
    [SerializeField] GameObject btnPrefab;
    [SerializeField] Transform accessoryParent;
    public List<WardrobeSection> WardrobeSections;
    List<GameObject> accessoryList; //reference to all the hidden accessory gameobjects
    List<Transform> wardrobeSectionList;    //transforms of all the wardrobe section parents

    void OnDestroy()
    {
        foreach(var i in WardrobeSections)i.renderer.sharedMesh = i.defaultMesh;
    }

    //called by wardrobebutton at start
    public void WardrobeInit()
    {
        Instance = this;
        accessoryList = new List<GameObject>();
        wardrobeSectionList = new List<Transform>();
        foreach(Transform i in transform)
        {
            wardrobeSectionList.Add(i);
            foreach (Transform child in i) Destroy(child.gameObject);//clear child
        }
        InitDefaultItems();
    }
    //loop through the default list set in the editor, and load them into the wardrobe
    void InitDefaultItems()
    {
        //create the list of accessory gameobject reference
        foreach(Transform i in accessoryParent)
        {
            GameObject obj = i.gameObject;
            accessoryList.Add(obj);
            obj.SetActive(false);
        }

        foreach(WardrobeSection section in WardrobeSections)
        {
            //create button for the default items
            foreach(GiftItem i in section.defaultItems)
            {
                CreateItemBtn(i,false);
            }
        }
    }
    public void AddItemToWardrobe(GiftItem item)
    {
        Debug.Log("adding item" + item.name + " to " + item.section);
        CreateItemBtn(item);
    }

    //take care of mesh/gameobject/ type of wearable item
    void CreateItemBtn(GiftItem item, bool isNew = true)
    {
        Transform parent = wardrobeSectionList.Find(x => x.name == item.section.ToString());

        GameObject obj = Instantiate(btnPrefab,parent);
        Button btn = obj.GetComponent<Button>();

        WardrobeSection section = WardrobeSections.Find(x => x.sectionName == item.section);
        obj.GetComponent<Image>().sprite = item.icon;
        obj.name = item.name;

        //wearable property
        switch(item.type)
        {
            case ItemType.Mesh:
            //if(section.renderer == null) Debug.LogWarning("Warning! " + section.DisplayName + " does not have a renderer but has meshes");
            btn.onClick.AddListener( () => ChangeMesh(section, item.mesh));
            break;

            case ItemType.Material:
            btn.onClick.AddListener(ChangeMaterial);

            break;
            case ItemType.Gameobject:
            btn.onClick.AddListener(() => DisplayGameobject(item.name));
            break;
        }

        WearableItem itemComponent = obj.GetComponent<WearableItem>();
        itemComponent.InitItem(item, isNew);
    }

    void ChangeMesh(WardrobeSection section, Mesh meshToChange)
    {
        Mesh myMesh = section.renderer.sharedMesh;

        if(myMesh == meshToChange)  //if the item is clicked twice, set it back to default.
        {
            Debug.Log("set" + section.renderer + "mesh to default");
            section.renderer.sharedMesh = section.defaultMesh;
        }
        else
        {
            section.renderer.sharedMesh = meshToChange;
        }
    }

    void ChangeMaterial()
    {
        //change material of all renderer
    }

    void DisplayGameobject(string objName)
    {
        //turn on/off the gameobject
        var obj = accessoryList.Find(x => x.name == objName);
        obj.SetActive(!obj.activeSelf);
    }
}


