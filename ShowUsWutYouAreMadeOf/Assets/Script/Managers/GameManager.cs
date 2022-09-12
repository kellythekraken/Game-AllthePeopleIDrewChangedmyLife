using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;

public enum CurrentMode { Nothing, Conversation, Sketching, Changing}

public class GameManager : MonoBehaviour
{
    //access current mode
    //controls the change of modes
    //saves all the important reference
    
    public static GameManager Instance;
    private CurrentMode _currMode;
    public CurrentMode currMode
    {
        get { return _currMode; }
        set
        {
            Debug.Log("change mode to " + value);
            OnModeChanged(value);
        }
    }
    private CurrentMode lastMode;
    public GameObject mainUI, settingsUI, sketchbookUI, dialogueUI, newItemWindow;
    public DialogueRunner dialogueRunner;
    [SerializeField] private NPCManager npcManager;
    [SerializeField] private GameObject pronounTag;
    [SerializeField] private Camera mainCamera, uiCamera, wardrobeCamera;
    private SketchingSystem sketchManager;
    internal WardrobeButton wardrobeBtn;
    internal InputManager inputManager;
    private TextMeshProUGUI pronounText;
    internal QueerNPC sketchSubject;
    internal bool sketchbookOpen;
    internal bool inConversation;
    private void Awake()
    {
        Instance = this;
        dialogueUI.SetActive(true);
        inputManager = FindObjectOfType<InputManager>();
        dialogueRunner.AddCommandHandler<bool>("sketch",OpenCloseSketchbook);
        //dialogueRunner.AddCommandHandler("gift", GiveItem);
        dialogueRunner.AddCommandHandler("pronoun", ShowPronoun);
        dialogueRunner.AddCommandHandler<string>("enter", npcManager.OnStage);
        dialogueRunner.AddCommandHandler<string>("leave", npcManager.OffStage);
        dialogueRunner.AddCommandHandler("randomEnter", npcManager.OnStageRandom);
        dialogueRunner.onDialogueComplete.AddListener(Complete);
    }
    void Complete()
    {
        Debug.Log("dialogue complete");
    }
    private void Start()
    {
        wardrobeBtn = WardrobeButton.Instance;
        sketchManager = sketchbookUI.GetComponent<SketchingSystem>();
        pronounText = pronounTag.GetComponentInChildren<TextMeshProUGUI>();

        OpenCloseSketchbook(false);
        pronounTag.SetActive(false);
        settingsUI.SetActive(false);

        LockCursor(true);
    }
    void OnModeChanged(CurrentMode mode)
    {
        lastMode = currMode;
        _currMode = mode;
        switch (mode)
        {
            case CurrentMode.Nothing:
                LockCursor(true);
                inputManager.EnableChatMoveBtn(true);
                inConversation = false;
                return;
            case CurrentMode.Conversation:
                LockCursor(true);
                inConversation = true;
                inputManager.EnableChatMoveBtn(false);
                return;
            case CurrentMode.Sketching:
                LockCursor(false);
                inConversation = false;
                inputManager.EnableChatMoveBtn(false);
                return;
            case CurrentMode.Changing:
                LockCursor(false);
                inConversation = false;
                inputManager.EnableChatMoveBtn(false);
                return;
        }
    }
    public void BackToLastMode()
    {
        currMode = lastMode;
    }

    bool inSetting = false;
    public void ToggleSettingScreen()
    {
        inSetting = !inSetting;
        settingsUI.SetActive(inSetting);
        LockCursor(!inSetting);
    }

    public void ContinueSketchChat()
    {
        //currMode = CurrentMode.Sketching;
        sketchSubject.StartSketchConversation();
    }
    public void OpenCloseSketchbook(bool open)
    {
        sketchbookOpen = open;
        sketchbookUI.SetActive(open);
        mainUI.SetActive(!open);

        if(open) sketchManager.PrepareToSketch(sketchSubject.queerID);
    }

    public void TriggerEndGameEvent()
    {
        Debug.Log("end game!");
    }

    public void ShowPronoun()
    {
        pronounTag.SetActive(true);
        pronounText.text = sketchSubject.queerID.pronouns;
    }
    public void HidePronoun()
    {
        pronounTag.SetActive(false);
    }

    public void LockCursor(bool lockCursor)
    {
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
    }

}
