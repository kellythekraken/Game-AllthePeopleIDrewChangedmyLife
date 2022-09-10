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

        InputManager.Instance.interactAction.performed += ctx => { if(popupDisplayOn) DisplayWindow(false); };
        InputManager.Instance.wardrobeAction.performed += ctx => { OpenCloseWardrobe();};

        closeBtn.onClick.AddListener(() => OpenCloseWardrobe());
        wardrobeUI.SetActive(false);
    }

    bool wardrobeOpen = false;
    void OpenCloseWardrobe()
    {
        wardrobeOpen = !wardrobeOpen;

        wardrobeUI.SetActive(wardrobeOpen);
        if(wardrobeOpen)
        {
            GameManager.Instance.currMode = CurrentMode.Changing;
            GameManager.Instance.ToggleCameraMode(1);
            if(newItem)
            {
                newItem = false;
                newIndicator.enabled = newItem;
            }
        }
        else{
            GameManager.Instance.ToggleCameraMode(0);
            GameManager.Instance.BackToLastMode();
        }
    }

    public void DisplayReceivedItem(string npcName, GiftItem gift)
    {
        DisplayWindow(true);
        string text = string.Format("You received {0} from {1}!", gift.name, npcName);

        itemText.text = text;
        itemIcon.sprite = gift.icon;

        newIndicator.enabled = true;
        newItem = true;
        
        AddItemToWardrobe(gift);
    }
    bool popupDisplayOn;
    public void DisplayWindow(bool open)
    {
        if(!open) GameManager.Instance.currMode = CurrentMode.Nothing;
        popupDisplayOn = open;
        popupWindow.SetActive(open);
    }

    public void AddItemToWardrobe(GiftItem gift)
    {
        Transform parent = wardrobeSections.Find(x => x.name == gift.tag.ToString());
        GameObject obj = Instantiate(itemPrefab, parent);
        obj.GetComponent<Image>().sprite = gift.icon;
        obj.name = gift.name;

        WearableItem item = obj.GetComponent<WearableItem>();
        item.InitItem(gift);
    }
    void ClearChild(Transform parent)
    {
        foreach (Transform child in parent) Destroy(child.gameObject);
    }
}
