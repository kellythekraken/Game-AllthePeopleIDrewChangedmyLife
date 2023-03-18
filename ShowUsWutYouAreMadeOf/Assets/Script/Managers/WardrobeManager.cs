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

    // available colors
    public static WardrobeManager Instance;
    [SerializeField] GameObject btnPrefab;
    [SerializeField] Transform accessoryParent;
    public List<WardrobeSection> WardrobeSections;
    Dictionary<string,WearableItem> defaultItemPair;
    List<WardrobeParent> wardrobeParents;
    List<GameObject> accessoryList; //reference to all the hidden accessory gameobjects
    List<Transform> wardrobeParentTransforms;    //transforms of all the wardrobe section parents
    List<string> outfitGifterList; //list the names of people who you're wearing their gifted item

    //each piece will have queer score, (and fashion score??)

    //new feature that triggers extra dialogue based on score/if you're wearing someone's gifted piece

    void OnDestroy()
    {
        //foreach(var i in WardrobeSections)i.renderer.sharedMesh = i.defaultMesh;
    }
   
    //called by wardrobebutton at start
    public void WardrobeInit()
    {
        Instance = this;
        accessoryList = new List<GameObject>();
        wardrobeParents = new List<WardrobeParent>();

        wardrobeParentTransforms = new List<Transform>();
        defaultItemPair = new Dictionary<string, WearableItem>();
        outfitGifterList = new List<string>();
        topSection = WardrobeSections.Find(t=>t.sectionName == ItemSection.Top);
        botSection = WardrobeSections.Find(t=>t.sectionName == ItemSection.Bottom);
        foreach(Transform i in transform)//get all children transform, that are wardrobe sections
        {
            var x = i.GetComponent<WardrobeParent>();
            if(x !=null) wardrobeParents.Add(x);

            wardrobeParentTransforms.Add(i);
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
            section.renderer = section.defaultRender;
            section.defaultMesh = section.defaultRender.sharedMesh;
            
            var parent = wardrobeParents.Find(x =>x.sectionName == section.sectionName);
            
            //create button for the default items
            for(var i = 0; i<section.defaultItems.Count;i++)
            {
                var btn = CreateItemBtn(section.defaultItems[i],false);
                if(parent!=null) parent.AddToList(btn.GetComponent<WearableItem>());
                if(i==0)//create the dict storing default items
                {
                    defaultItemPair.Add(section.sectionName.ToString(),btn.GetComponent<WearableItem>());
                }
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
        itemComponent.InitItem(item, isNew);
        itemComponent.gifter = gifterName;
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
    WardrobeSection topSection,botSection;
    void ChangeMesh(WardrobeSection section, Mesh meshToChange)
    {
        Mesh myMesh = section.renderer.sharedMesh;

        if(myMesh == meshToChange)  //if the item is clicked twice, set it back to default.
        {
            if(wearingDress) //dress and top logic
            {
                if(section.sectionName == ItemSection.Dress)   //attempt to take off dress
                {
                    wearingDress = false;

                    //button indicate default trousers 
                    SelectDefaultButton("Bottom",true);

                    if( topSection.renderer.sharedMesh ==null)
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
            //SelectDefaultButton(section.sectionName.ToString(),true);
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
                var topSection = WardrobeSections.Find(t=>t.sectionName == ItemSection.Top);
                if( topSection.renderer.sharedMesh ==null)
                {
                    topSection.renderer.sharedMesh = topSection.defaultMesh; 
                    SelectDefaultButton("Top",true);
                }
            }
            section.renderer.sharedMesh = meshToChange;
        }
    }
    void SelectDefaultButton(string parentName, bool select)
    {
        WearableItem defaultitem;
        defaultItemPair.TryGetValue(parentName,out defaultitem);
        Debug.Log("select" + parentName);
        defaultitem.WearItem(select);
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


