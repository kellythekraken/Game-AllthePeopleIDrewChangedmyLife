using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class WearableItem : MonoBehaviour
{
    //attached to the wardrobe item buttons. Store the infomation of each item and will respond to user clicking.

    //could only have one hairstyle, but outfit you can put on as many as possible? 
    //also as many accessories as possible

    [SerializeField] private GameObject newIndicator;
    [SerializeField] private Image selectedIndicator;
    
    Image iconImage;
    Transform parentWardrobeSection;
    bool newItem = true;
    bool isWearing;
    GiftItem item;
    Button btn;

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
        
        //make all other wearable item of the same parenthood !ISWEARING and deselected!

        foreach(Transform i in parentWardrobeSection)
        {
            if(i.name != this.name) 
            {
                var component = i.GetComponent<WearableItem>();
                component.isWearing = false; 
                component.iconImage.color = Color.white;
                //component.selectedIndicator.enabled = false;
            }
        }
        iconImage.color = isWearing? Color.black : Color.white;

        //selectedIndicator.enabled = isWearing;  
    }

}
