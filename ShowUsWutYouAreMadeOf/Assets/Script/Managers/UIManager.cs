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
    [SerializeField] TextMeshProUGUI instructionText;
    TextMeshProUGUI itemText, noticeText;
    bool popupOn;
    
    void Awake() => Instance = this;
    void Start()
    {
        itemText = receivedItemWindow.GetComponentInChildren<TextMeshProUGUI>();
        noticeText = noticeWindow.GetComponentInChildren<TextMeshProUGUI>();
        InputManager.Instance.interactAction.performed += ctx => { if(popupOn) DisplayItemWindow(false); };

        DisplayItemWindow(false);
        DisplayNoticeWindow(false);
        receivedItemWindow.SetActive(false);
        noticeWindow.SetActive(false);
        instructionText.enabled = false;
    }
        public void DisplayItem(string textToDisplay)
    {
        itemText.text = textToDisplay;
        DisplayItemWindow(true);
    }
    public void DisplayNotification(string textToDisplay)
    {
        noticeText.text = textToDisplay;
        DisplayNoticeWindow(true);
    }
    bool currentlyDisplayingInstruction = false;

    public void DisplayInstruction(string textToDisplay, float displayTime)
    {
        StartCoroutine(DisplayInstruction(displayTime));
        instructionText.text = textToDisplay;
    }

    IEnumerator DisplayInstruction(float timer)
    {
        instructionText.enabled = true;
        currentlyDisplayingInstruction = true;
        yield return new WaitForSeconds(timer);
        instructionText.enabled = false;
        currentlyDisplayingInstruction = false;
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
