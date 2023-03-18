using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class WearableItem : MonoBehaviour
{
    //attached to the wardrobe item buttons. Store the infomation of each item and will respond to user clicking.
    //UI surface only changes, for function go to wardrobemanager

    [SerializeField] private GameObject newIndicator;
    //[SerializeField] private Image selectedIndicator;
    WardrobeManager wManager;
    internal ItemSection sectionType;    //assigned by wardrobe manager
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
        btn.onClick.AddListener(WearBtnClicked);
        wManager = WardrobeManager.Instance;
    }
    public void InitItem(GiftItem incomingItem, bool isNew)
    {
        item = incomingItem;
        isWearing = false;      //=selectedIndicator.enabled = 
        newIndicator.SetActive(true);
        parentWardrobeSection = transform.parent;
        if(!isNew) //default item shouldn't have new indicator
        {
            newItem = false; 
            newIndicator.SetActive(false);
        }
    }

    void WearBtnClicked()
    {
        isWearing = !isWearing;
        WearItem(isWearing);
    }
    public void WearItem(bool wear)
    {
        if(newItem) 
        {
            newItem = false;
            newIndicator.SetActive(false);
        }
        //put on/off the item
        isWearing = wear;

        //logic to call wardrobemanager 
        iconImage.color = wear? Color.black : Color.white;

        //make the last wearing item of the same parenthood !ISWEARING and deselected!
        
        if(isWearing)
        {
            foreach(Transform i in parentWardrobeSection)
            {
                var component = i.GetComponent<WearableItem>();
                if(i.name != this.name && component.isWearing) 
                {
                    component.WearItem(false);
                }
            }
        }
        
        //selectedIndicator.enabled = isWearing;  
        if(gifter!=null) WardrobeManager.Instance.UpdateGifterList(gifter,isWearing); 
    }
}
