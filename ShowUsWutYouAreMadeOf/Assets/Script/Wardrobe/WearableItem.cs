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
    WardrobeParent parentWardrobeSection;
    bool newItem = true;
    internal bool isWearing;
    GiftItem item;
    Button btn;
    internal string gifter; 

    public void InitItem(GiftItem incomingItem, bool isNew)
    {
        btn = GetComponent<Button>();
        iconImage = GetComponent<Image>();
        btn.onClick.AddListener(WearBtnClicked);
        wManager = WardrobeManager.Instance;

        item = incomingItem;
        isWearing = false;      //=selectedIndicator.enabled = 
        newIndicator.SetActive(true);
        parentWardrobeSection = transform.parent.GetComponent<WardrobeParent>();
        if(!isNew) //default item shouldn't have new indicator
        {
            newItem = false; 
            newIndicator.SetActive(false);
        }
    }
    void WearBtnClicked()//for actual button response
    {
        if(isWearing)
        {
            //make default light up if you're clicking on the same one 
            Debug.Log("taking off " + name);
            parentWardrobeSection.SetToDefaultItem();
            return;
        }
        else { WearItem(true);}
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
            parentWardrobeSection.UnSelectPreviousItem(name);
        }
        
        //selectedIndicator.enabled = isWearing;  
        if(gifter!=null) WardrobeManager.Instance.UpdateGifterList(gifter,isWearing); 
    }
}
