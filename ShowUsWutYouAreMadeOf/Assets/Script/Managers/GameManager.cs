using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;
using UnityEngine.Events;

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
    public GameObject settingsUI, sketchbookUI, dialogueUI, newItemWindow;
    public DialogueRunner dialogueRunner;
    internal UnityEvent BodypartSelectEvent = new UnityEvent();
    [SerializeField] private NPCManager npcManager;
    [SerializeField] private GameObject pronounTag;
    [SerializeField] private GameObject playerObject;
    private SketchingSystem sketchManager;
    internal WardrobeButton wardrobeBtn;
    internal InputManager inputManager;
    private TextMeshProUGUI pronounText;
    internal QueerNPC sketchSubject;
    internal bool sketchbookOpen;
    internal bool inConversation;
    private Camera mainCam;
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
    }
    private void Start()
    {
        wardrobeBtn = WardrobeButton.Instance;
        sketchManager = SketchingSystem.Instance;
        pronounText = pronounTag.GetComponentInChildren<TextMeshProUGUI>();
        mainCam = Camera.main;
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

    #region SKETCHING PHASE
    public void ContinueSketchChat()
    {
        sketchSubject.StartSketchConversation();
    }
    public void OpenCloseSketchbook(bool open)
    {
        ShowPlayer(!open);
        if(open) 
        {
            sketchManager.PrepareToSketch(sketchSubject.queerID);
            StartCoroutine(OpenSketchbook());
        }
        else sketchbookUI.SetActive(false);

        sketchbookOpen = open;
        wardrobeBtn.gameObject.SetActive(!open);
    }
    IEnumerator OpenSketchbook()
    {
        yield return new WaitForSeconds(.8f);  //delay shortly before opening sketchbook
        sketchbookUI.SetActive(true);
    }
#endregion

    #region CONVERSATION PHASE
    public void ShowPronoun()
    {
        pronounTag.SetActive(true);
        pronounText.text = sketchSubject.queerID.pronouns;
    }
    public void HidePronoun()
    {
        pronounTag.SetActive(false);
    }
#endregion

    public void ShowPlayer(bool show)
    {
        playerObject.SetActive(show);
    }
    bool inSetting = false;
    public void ToggleSettingScreen()
    {
        inSetting = !inSetting;
        settingsUI.SetActive(inSetting);
        if(inSetting) currMode = CurrentMode.Changing;
        else BackToLastMode();
    }
    public void TriggerEndGameEvent()
    {
        Debug.Log("end game!");
    }
    public void LockCursor(bool lockCursor)
    {
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
    }

}
