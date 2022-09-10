using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class WearableItem : MonoBehaviour
{
    [SerializeField] private GameObject newIndicator;
    [SerializeField] private Image selectedIndicator;
    bool newItem; 
    bool isWearing;
    GiftItem item;
    Button btn;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(WearItem);
    }
    public void InitItem(GiftItem incomingItem)
    {
        item = incomingItem;
        newItem = false;
        newIndicator.SetActive(true);
    }

    void WearItem()
    {
        if(!newItem) 
        {
            newItem = true;
            newIndicator.SetActive(false);
        }
        //put on/off the item
        isWearing = !isWearing;
        selectedIndicator.enabled = isWearing;
        
    }
}
