using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardrobeParent : MonoBehaviour
{
    internal ItemSection sectionName;
    public List<WearableItem> items;

    public void AddToList(WearableItem item)
    {
        if(items == null) items = new List<WearableItem>();;
        items.Add(item);
    }
    public void SetToDefaultItem()
    {
        transform.GetChild(0).GetComponent<WearableItem>().WearItem(true);
    }
    public void UnselectAllItems()
    {
        foreach(var i in items)
        {
            i.WearItem(false);
        }
    }
}
