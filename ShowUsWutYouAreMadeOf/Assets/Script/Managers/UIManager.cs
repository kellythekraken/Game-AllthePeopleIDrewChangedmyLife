using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    //controls the popup ui and main interface
    public static UIManager Instance;
    public GameObject receivedItemWindow, noticeWindow;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private Button backToStartBtn;
    TextMeshProUGUI itemText, noticeText;
    bool popupOn;

    internal UnityEvent BackToStartEvent;
    
    void Awake() => Instance = this;
    void Start()
    {
        BackToStartEvent = new UnityEvent();
        itemText = receivedItemWindow.GetComponentInChildren<TextMeshProUGUI>();
        noticeText = noticeWindow.GetComponentInChildren<TextMeshProUGUI>();
        backToStartBtn.onClick.AddListener(BackToStartBtnClicked);
        receivedItemWindow.SetActive(false);
        noticeWindow.SetActive(false);
    }
    void OnEnable() => InputManager.Instance.interactAction.performed += InteractAction;
    void OnDisable()=> InputManager.Instance.interactAction.performed -= InteractAction;

    public void InteractAction(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        if(popupOn) 
        {
            DisplayItemWindow(false);
            DisplayInstruction("Press TAB to change outfit", 3f);
        }
    }

    public void DisplayNotification(string textToDisplay)
    {
        noticeText.text = textToDisplay;
        DisplayNoticeWindow(true);
    }
    bool currentlyDisplayingInstruction = false;

    public void ShowInteractInstruction()
    {
        if(currentlyDisplayingInstruction) {InteractIndicator.Instance.DisplayIndicator(false); return;}
        instructionText.text = "E / Left Mouse to interact";    //maybe change this to (e) after the text display??
        instructionText.enabled = true;
    }
    public void HideInstruction()
    {
        if(currentlyDisplayingInstruction) return;
        instructionText.enabled = false;
    }
    public void DisplayInstruction(string textToDisplay)
    {
        instructionText.text = textToDisplay;
        instructionText.enabled = true;
    }

    //add the dialogue into a queue, and priority?? or override the show hid instruction
    //bool instructionRunning = false;
    public void DisplayInstruction(string textToDisplay, float displayTime)
    {
        instructionText.text = textToDisplay;
        StartCoroutine(DisplayInstruction(displayTime));
    }

    IEnumerator DisplayInstruction(float timer)
    {
        instructionText.enabled = true;
        currentlyDisplayingInstruction = true;
        yield return new WaitForSeconds(timer);
        instructionText.enabled = false;
        currentlyDisplayingInstruction = false;
    }
    QueerNPC itemGifter;
    public void DisplayItem(QueerNPC npc)
    {
        itemText.text = npc.queerID.giftLine;
        itemGifter = npc;
        DisplayItemWindow(true);
    }
    public void DisplayItemWindow(bool open)
    {
        if(!open) 
        {
            HideInstruction();
            GameManager.Instance.currMode = CurrentMode.Nothing;
            GameManager.Instance.EnableWardrobeAction(true);
            itemGifter.alreadyGifted = true;
        }
        else 
        {
            DisplayInstruction("E/Click to continue");
            InputManager.Instance.EnableInteractBtn(true);
            GameManager.Instance.EnableWardrobeAction(false);
        }
    
        popupOn = open;
        receivedItemWindow.SetActive(open);
    }

    void DisplayNoticeWindow(bool open)
    {
        noticeWindow.SetActive(open);
        GameManager.Instance.LockCursor(!open);
    }

    void BackToStartBtnClicked()
    {
        GameManager.Instance.ToggleSettingScreen();
        BackToStartEvent.Invoke();
    }
}
