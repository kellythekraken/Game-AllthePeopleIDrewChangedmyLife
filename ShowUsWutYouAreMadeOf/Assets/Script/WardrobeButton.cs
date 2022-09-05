using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WardrobeButton : MonoBehaviour
{
    public GameObject popupWindow, wardrobeUI;
    public Button closeBtn;
    public Image newIndicator;
    Button openBtn, closePopup;

    bool newItem = false;
    private void Start()
    {
        DisplayWindow(false);

        newIndicator.enabled = newItem;
        openBtn = GetComponent<Button>();
        closePopup = popupWindow.GetComponentInChildren<Button>();

        openBtn.onClick.AddListener(() => OpenCloseWardrobe(true));
        closeBtn.onClick.AddListener(() => OpenCloseWardrobe(false));
        closePopup.onClick.AddListener(() => DisplayWindow(false));

        OpenCloseWardrobe(false);
    }

    void OpenCloseWardrobe(bool open)
    {
        wardrobeUI.SetActive(open);
        Debug.Log("set wardrobe" + open);
        if(open && newItem)
        {
            newItem = false;
            newIndicator.enabled = newItem;
        }
    }

    public void ReiceivedItem(string text)
    {
        //here goes the logic for loading item images?

        DisplayWindow(true);
        popupWindow.GetComponentInChildren<TextMeshProUGUI>().text = text;
        newIndicator.enabled = true;
        newItem = true;
    }

    public void DisplayWindow(bool open)
    {
        popupWindow.SetActive(open);
    }
}
