using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WardrobeButton : MonoBehaviour
{
    public GameObject popupWindow, wardrobeUI;
    public Button closeBtn;
    public Image newIndicator;
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

    public void DisplayReceivedItem(string text, Sprite image)
    {
        //here goes the logic for loading item images?

        DisplayWindow(true);
        itemText.text = text;
        itemIcon.sprite = image;

        newIndicator.enabled = true;
        newItem = true;
    }

    public void DisplayWindow(bool open)
    {
        popupWindow.SetActive(open);
    }
}
