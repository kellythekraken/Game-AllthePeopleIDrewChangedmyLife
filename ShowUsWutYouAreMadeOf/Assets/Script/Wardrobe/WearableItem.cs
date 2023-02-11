using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class WearableItem : MonoBehaviour
{
    //attached to the wardrobe item buttons. Store the infomation of each item and will respond to user clicking.

    //also as many accessories as possible

    [SerializeField] private GameObject newIndicator;
    [SerializeField] private Image selectedIndicator;
    
    Image iconImage;
    Transform parentWardrobeSection;
    bool newItem = true;
    bool isWearing;
    GiftItem item;
    Button btn;
    internal string gifter; 

    void Start()
    {
        btn = GetComponent<Button>();
        iconImage = GetComponent<Image>();
        btn.onClick.AddListener(WearItem);
    }
    public void InitItem(GiftItem incomingItem, bool isNew)
    {
        item = incomingItem;
        isWearing = selectedIndicator.enabled = false;
        newIndicator.SetActive(true);
        parentWardrobeSection = transform.parent;
        if(!isNew) //default item shouldn't have new indicator
        {
            newItem = false; 
            newIndicator.SetActive(false);
        }
    }
    void WearItem()
    {
        if(newItem) 
        {
            newItem = false;
            newIndicator.SetActive(false);
        }
        //put on/off the item
        isWearing = !isWearing;

        //logic to call wardrobemanager 
        iconImage.color = isWearing? Color.black : Color.white;

        //make the last wearing item of the same parenthood !ISWEARING and deselected!
        if(isWearing)
        {
            foreach(Transform i in parentWardrobeSection)
            {
                var component = i.GetComponent<WearableItem>();
                if(i.name != this.name && component.isWearing) 
                {
                    component.TakeOffItem();
                }
            }
        }
        //selectedIndicator.enabled = isWearing;  
        if(gifter!=null) WardrobeManager.Instance.UpdateGifterList(gifter,isWearing); 
    }

    void TakeOffItem()
    {
        isWearing = false; 
        iconImage.color = Color.white;
        if(gifter!=null) WardrobeManager.Instance.UpdateGifterList(gifter,false); 
        //component.selectedIndicator.enabled = false;
    }
}
