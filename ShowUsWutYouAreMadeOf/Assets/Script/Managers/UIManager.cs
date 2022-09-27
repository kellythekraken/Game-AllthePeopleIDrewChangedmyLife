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
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private Button backToStartBtn;
    TextMeshProUGUI itemText, noticeText;
    bool popupOn;
    
    void Awake() => Instance = this;
    void Start()
    {
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

    public void ShowInstruction(string textToDisplay)
    {
        instructionText.text = textToDisplay;
        instructionText.enabled = true;
    }

    public void HideInstruction()
    {
        if(currentlyDisplayingInstruction) return;
        instructionText.enabled = false;
    }

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
    public void DisplayItemWindow(bool open)
    {
        if(!open) 
        {
            GameManager.Instance.currMode = CurrentMode.Nothing;
            InputManager.Instance.EnableChatMoveBtn(true);
        }
        else{ InputManager.Instance.EnableChatMoveBtn(false);}
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
        SceneManager m = FindObjectOfType<SceneManager>();
        m.ActivateStartMenu();
    }
}
