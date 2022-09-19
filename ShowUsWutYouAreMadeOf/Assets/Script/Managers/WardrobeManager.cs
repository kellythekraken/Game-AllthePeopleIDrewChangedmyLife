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
    [SerializeField] GameObject btnPrefab;
    public List<WardrobeSection> WardrobeSections;
    public WardrobeSection CurrentWardrobeSection { get; private set; }
    public static WardrobeManager Instance;

    [SerializeField] Transform wardrobeParent;
    [SerializeField] Transform accessoryParent;
    List<GameObject> accessoryList;
    List<Transform> wardrobeSectionList;
    List<Mesh> currentMeshInWear;

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
        foreach(Transform i in wardrobeParent.transform)
        {
            wardrobeSectionList.Add(i);
            foreach (Transform child in i) Destroy(child.gameObject);//clear child
        }
        InitDefaultList();
    }
    //loop through the default list set in the editor, and load them into the wardrobe
    void InitDefaultList()
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
            //add the button to the local parent

            string sectionName = section.DisplayName;
            //button for the meshes
            foreach(GiftItem i in section.defaultItems)
            {
                CreateItemBtn(i);
            }
        }
    }
    
    public void AddItemToWardrobe(GiftItem item)
    {
        Debug.Log("adding item" + item.name + " to " + item.section);
        //var btn = CreateItemBtn(i);

        //instantiate and bind to a button
        //add to default list
        //should share some same functions with the initdefaultlist

        //if it's a gameobject, bind it to the existing gameobject from the list, by name
        
    }

    void CreateItemBtn(GiftItem item)
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
            btn.onClick.AddListener(() => DisplayGameobject(item.itemName));
            break;
        }

        WearableItem itemComponent = obj.GetComponent<WearableItem>();
        itemComponent.InitItem(item);
    }

    void ChangeMesh(WardrobeSection section, Mesh meshToChange)
    {
        Mesh myMesh = section.renderer.sharedMesh;
        if(myMesh == meshToChange)
        {
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


