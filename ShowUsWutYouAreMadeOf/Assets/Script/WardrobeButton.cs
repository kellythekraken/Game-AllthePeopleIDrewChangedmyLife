using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class WardrobeButton : MonoBehaviour
{
    public GameObject popupWindow, wardrobeUI;
    public Button closeBtn;
    public Image newIndicator;
    [SerializeField] private Transform wardrobeParent;
    [SerializeField] private GameObject itemPrefab;

    List<Transform> wardrobeSections;
    Button openBtn, closePopup;
    TextMeshProUGUI itemText;
    Image itemIcon;

    bool newItem = false;
    private void Start()
    {
        DisplayWindow(false);

        newIndicator.enabled = newItem;
        openBtn = GetComponent<Button>();
        closePopup = popupWindow.GetComponentInChildren<Button>();
        itemText = popupWindow.GetComponentInChildren<TextMeshProUGUI>();
        itemIcon = popupWindow.transform.Find("Icon").GetComponent<Image>();

        wardrobeSections = new List<Transform>();

        foreach(Transform i in wardrobeParent)
        {
            wardrobeSections.Add(i);
            ClearChild(i);
        }

        openBtn.onClick.AddListener(() => OpenCloseWardrobe(true));
        closeBtn.onClick.AddListener(() => OpenCloseWardrobe(false));
        closePopup.onClick.AddListener(() => DisplayWindow(false));

        OpenCloseWardrobe(false);
    }

    void OpenCloseWardrobe(bool open)
    {
        wardrobeUI.SetActive(open);
        if(open && newItem)
        {
            newItem = false;
            newIndicator.enabled = newItem;
        }
    }

    public void DisplayReceivedItem(string npcName, GiftItem gift)
    {
        //here goes the logic for loading item images?

        DisplayWindow(true);
        string text = string.Format("You received {0} from {1}!", gift.name, npcName);

        itemText.text = text;
        itemIcon.sprite = gift.icon;

        newIndicator.enabled = true;
        newItem = true;
        
        AddItemToWardrobe(gift);
    }

    public void DisplayWindow(bool open)
    {
        popupWindow.SetActive(open);
    }

    public void AddItemToWardrobe(GiftItem gift)
    {
        Transform parent = wardrobeSections.Find(x => x.name == gift.tag.ToString());
        var item = Instantiate(itemPrefab, parent);
        item.GetComponent<Image>().sprite = gift.icon;
        item.name = gift.name;

        Button btn = item.GetComponent<Button>();
        btn.onClick.AddListener(() => WearableItemClicked(gift));

        item.transform.Find("newIndicator").GetComponent<Image>().enabled = true;
    }
    void WearableItemClicked(GiftItem item)
    {
        if (!item.clicked)
        {
            item.clicked = true;
        }
    }

    void ClearChild(Transform parent)
    {
        foreach (Transform child in parent) Destroy(child.gameObject);
    }
}
