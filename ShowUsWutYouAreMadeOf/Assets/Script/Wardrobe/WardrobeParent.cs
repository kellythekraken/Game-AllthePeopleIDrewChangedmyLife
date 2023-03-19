using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardrobeParent : MonoBehaviour
{
    //init with list of item buttons, control the set to default and unselect all item function
    internal ItemSection sectionName;
    public List<WearableItem> items;

    void InitWardrobeParent()
    {
        sectionName = (ItemSection)System.Enum.Parse(typeof(ItemSection),this.name);
    }
    public void AddDefaultItemsToList()
    {
        InitWardrobeParent();
        foreach(Transform c in this.transform)
        {
            var btn = c.GetComponent<WearableItem>();
            if(btn!= null) AddToList(btn);
        }
        if(name != "Dress" && name != "Accessory")
        {items[0].WearItem(true);}
    }
    public void AddToList(WearableItem item)
    {
        if(items == null) items = new List<WearableItem>();;
        items.Add(item);
    }
    public void SetToDefaultItem()
    {
        if(sectionName == ItemSection.Dress)
        {
            UnselectAllItems(); return;
        }
        for(var i = 1; i<items.Count;i++)
        {
            items[i].WearItem(false);
        }
        items[0].WearItem(true);
    }

    public void UnSelectPreviousItem(string newSelectedItem)
    {
        foreach(var i in items)
        {
            var component = i.GetComponent<WearableItem>();
            if(i.name != newSelectedItem && component.isWearing) 
            {
                component.WearItem(false);
            }
        }
    }
    public void UnselectAllItems()
    {
        foreach(var i in items)
        {
            i.WearItem(false);
        }
    }
}
