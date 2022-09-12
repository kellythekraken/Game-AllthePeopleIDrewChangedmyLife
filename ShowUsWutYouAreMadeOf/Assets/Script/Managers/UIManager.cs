using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //controls the popup ui and main interface
    public static UIManager Instance;

    public GameObject receivedItemWindow, noticeWindow;
    TextMeshProUGUI itemText, noticeText;
    Image itemIcon;
    bool popupOn;
    
    void Awake() => Instance = this;
    void Start()
    {
        itemText = receivedItemWindow.GetComponentInChildren<TextMeshProUGUI>();
        itemIcon = receivedItemWindow.transform.Find("Icon").GetComponent<Image>();
        noticeText = noticeWindow.GetComponentInChildren<TextMeshProUGUI>();

        InputManager.Instance.interactAction.performed += ctx => { if(popupOn) DisplayItemWindow(false); };


        DisplayItemWindow(false);
        DisplayNoticeWindow(false);
        receivedItemWindow.SetActive(false);
    }
        public void DisplayItem(string textToDisplay, Sprite icon)
    {
        itemText.text = textToDisplay;
        itemIcon.sprite = icon;
        DisplayItemWindow(true);
    }
    public void DisplayNotification(string textToDisplay)
    {
        Debug.Log("display notification");
        noticeText.text = textToDisplay;
        DisplayNoticeWindow(true);
    }

    public void DisplayItemWindow(bool open)
    {
        if(!open) GameManager.Instance.currMode = CurrentMode.Nothing;
        popupOn = open;
        receivedItemWindow.SetActive(open);
    }
    

    void DisplayNoticeWindow(bool open)
    {
        noticeWindow.SetActive(open);
        GameManager.Instance.LockCursor(!open);
    }
}
