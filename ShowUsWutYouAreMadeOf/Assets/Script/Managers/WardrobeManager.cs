using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public class WardrobeSection
{
    public ItemSection sectionName;
    public SkinnedMeshRenderer defaultRender;
    internal SkinnedMeshRenderer renderer;   //current renderer to replace the materials, maybe a list if there're more renderer?
    internal Mesh defaultMesh;
    public List<GiftItem> defaultItems;
    int _materialIndex;
    int _subObjectIndex;
}

public class WardrobeManager : MonoBehaviour
{
    //only manage things within the wardrobe!
    //store all of the found items

    public static WardrobeManager Instance;
    [SerializeField] GameObject btnPrefab;
    [SerializeField] Transform accessoryParent;
    public List<WardrobeSection> WardrobeSections;
    public WardrobeSection AccessorySections;
    List<WardrobeParent> wardrobeParents;
    List<GameObject> accessoryList; //reference to all the hidden accessory gameobjects
    List<Transform> wardrobeParentTransforms;    //transforms of all the wardrobe section parents
    List<string> outfitGifterList; //list the names of people who you're wearing their gifted item

    //each piece will have queer score, (and fashion score??)

    //new feature that triggers extra dialogue based on score/if you're wearing someone's gifted piece

    //called by wardrobebutton at start
    public void WardrobeInit()
    {
        Instance = this;
        accessoryList = new List<GameObject>();
        wardrobeParents = new List<WardrobeParent>();
        
        topSection = WardrobeSections.Find(t=>t.sectionName == ItemSection.Top);
        wardrobeParentTransforms = new List<Transform>();
        outfitGifterList = new List<string>();

        foreach(Transform i in transform)//get all children transform, that are wardrobe sections
        {
            var p = i.GetComponent<WardrobeParent>();
            wardrobeParents.Add(p);

            wardrobeParentTransforms.Add(i);
            foreach (Transform child in i.transform) Destroy(child.gameObject);
        }
        InitDefaultItems();

        //load default item to wardrobeparent list
        foreach(var i in wardrobeParents) i.AddDefaultItemsToList();
    }

    //loop through the default list set in the editor, and load them into the wardrobe
    void InitDefaultItems()
    {
        foreach(Transform i in accessoryParent)
        {
            GameObject obj = i.gameObject;
            accessoryList.Add(obj);
            obj.SetActive(false);
        }
        //create the list of accessory gameobject reference
        for(var x=0; x < AccessorySections.defaultItems.Count; x++)
        {
            var btn = CreateItemBtn(AccessorySections.defaultItems[x],false);
        }

        //for the rest of the wardrobe section
        foreach(WardrobeSection section in WardrobeSections)
        {
            section.renderer = section.defaultRender;
            section.defaultMesh = section.defaultRender.sharedMesh;

            //create button for the default items
            for(var i = 0; i < section.defaultItems.Count;i++)
            {
                var btn = CreateItemBtn(section.defaultItems[i],false);
            }
        }
    }
    public void AddGiftToWardrobe(GiftItem item, string gifterName)
    {
        CreateItemBtn(item, true, gifterName);  //would this become a problem? without var = createitembtn
    }

    //take care of mesh/gameobject/ type of wearable item
    GameObject CreateItemBtn(GiftItem item, bool isNew, string gifterName = null)
    {
        Transform parent = wardrobeParentTransforms.Find(x => x.name == item.section.ToString());

        GameObject obj = Instantiate(btnPrefab,parent);
        Button btn = obj.GetComponent<Button>();

        WardrobeSection section = WardrobeSections.Find(x => x.sectionName == item.section);
        obj.GetComponent<Image>().sprite = item.icon;
        obj.name = item.name;

        //wearable property
        switch(item.type)
        {
            case ItemType.Mesh:
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
        itemComponent.InitItem(item, isNew,item.section);
        itemComponent.gifter = gifterName;
        if(isNew)
        {
            var targetParent = wardrobeParents.Find(t=>t.sectionName == item.section);
            targetParent.AddToList(itemComponent);
        }
        return obj;
    }
    public void UpdateGifterList(string gifterName, bool addItem = true)
    {
        if(addItem) 
        {outfitGifterList.Add(gifterName);}
        else if(outfitGifterList.Contains(gifterName)) 
        {outfitGifterList.Remove(gifterName);}
    }

    public bool IsWearingGiftedItem(string gifterName)
    {
        return outfitGifterList.Contains(gifterName);
    }

#region ItemAppearance
    bool wearingDress = false;
    WardrobeSection topSection; //to check if wearing anything on the top
    void ChangeMesh(WardrobeSection section, Mesh meshToChange)
    {
        Mesh myMesh = section.renderer.sharedMesh;

        if(myMesh == meshToChange)  //if the item is clicked twice, set it back to default.
        {
            if(wearingDress) //dress, top and bottom logic
            {
                wardrobeParents.Find(t=>t.name == "Top").wearingDress = true;

                if(section.sectionName == ItemSection.Dress)   //attempt to take off dress
                {
                    wearingDress = false;
                    SelectDefaultButton("Bottom",true);
                    if(topSection.renderer.sharedMesh == null)
                    {
                        topSection.renderer.sharedMesh = topSection.defaultMesh;
                        SelectDefaultButton("Top",true);
                    }
                }
                else if(section.sectionName == ItemSection.Top) //attempt to take of top when dress
                {
                    topSection.renderer.sharedMesh = null;
                    return;
                }
            }
            section.renderer.sharedMesh = section.defaultMesh;
        }
        else
        {
            if(section.sectionName == ItemSection.Dress)
            {   
                wearingDress = true;
                SelectDefaultButton("Bottom",false);
            }
            else if (section.sectionName == ItemSection.Bottom) //put top on if change to trouser
            {
                //deselect dress?
                wearingDress = false;
                SelectDefaultButton("Dress",false);
                wardrobeParents.Find(t=>t.name == "Top").wearingDress = false;

                if( topSection.renderer.sharedMesh ==null)
                {
                    topSection.renderer.sharedMesh = topSection.defaultMesh; 
                    SelectDefaultButton("Top",true);
                }
            }
            section.renderer.sharedMesh = meshToChange;
        }
    }
    void SelectDefaultButton(string sectionName, bool select)
    {
        var p = wardrobeParents.Find(t=>t.name == sectionName);
        if(p!=null)
        {
            if(select) p.SetToDefaultItem();
            else { p.UnselectAllItems();}
        }
    }

    void ChangeMaterial()
    {
        //change material color of all renderer
    }

    void DisplayGameobject(string objName)
    {
        //turn on/off the gameobject
        var obj = accessoryList.Find(x => x.name == objName);
        obj.SetActive(!obj.activeSelf);
    }
#endregion
}


