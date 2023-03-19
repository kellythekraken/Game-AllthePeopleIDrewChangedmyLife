using System.Collections;
using UnityEngine;
using Yarn.Unity;
using TMPro;

public enum CurrentMode { Nothing, Conversation, Sketching, Changing, StartMenu}

public class GameManager : MonoBehaviour
{
    //access current mode
    //controls the change of modes
    //saves all the important reference
    public static GameManager Instance;
    private CurrentMode _currMode;
    internal CurrentMode currMode
    {
        get { return _currMode; }
        set
        {
            Debug.Log("change mode to " + value);
            OnModeChanged(value);
        }
    }
    private CurrentMode lastMode;

    [Header("UI")]
    public GameObject pronounTag;
    public GameObject settingsUI, sketchbookUI, dialogueUI, newItemWindow;
    public DialogueRunner dialogueRunner;
    internal InMemoryVariableStorage variableStorage;

    [Header("Player")]
    [SerializeField] public GameObject playerObject;   //player visibility
    [SerializeField] public GameObject player;   //the one with important controls and scripts

    [Header("Settings")]
    public bool startIndoor;    //determine where you start as 
    internal WardrobeButton wardrobeBtn;
    internal SceneManager sceneManager;
    internal InputManager inputManager;
    internal AudioManager audioManager;
    internal NPCManager npcManager;
    internal SketchingSystem sketchManager;
    internal InteractIndicator interactIndicator;
    internal TextMeshProUGUI pronounText;
    internal QueerNPC sketchSubject;
    internal bool sketchbookOpen;
    internal Camera mainCam;
    private void Awake()
    {
        Instance = this;
        mainCam = Camera.main;
        inputManager = FindObjectOfType<InputManager>();
        wardrobeBtn = WardrobeButton.Instance;
        sceneManager = SceneManager.Instance;
        npcManager = NPCManager.Instance;
        sketchManager = sketchbookUI.GetComponent<SketchingSystem>();
    }

    void ShowSketchInstruction()
    {
        UIManager.Instance.DisplayInstruction("Select an area on the subject to focus, and pick a color to sketch.", 4f);
    }
    private void Start()
    {
        dialogueUI.SetActive(true);
        sketchManager.InitSketchbook();
        audioManager = AudioManager.Instance;
        interactIndicator = InteractIndicator.Instance;
        variableStorage = dialogueRunner.GetComponent<InMemoryVariableStorage>();
        currMode = CurrentMode.Nothing;
        pronounText = pronounTag.GetComponentInChildren<TextMeshProUGUI>();
        sketchbookUI.SetActive(false);
        pronounTag.SetActive(false);
        settingsUI.SetActive(false);
        DisplayControlInstruction();

        dialogueRunner.AddCommandHandler<bool>("sketch",OpenCloseSketchbook);
        //dialogueRunner.AddCommandHandler("gift", GiveItem);
        dialogueRunner.AddCommandHandler("pronoun", DiscoveredPronoun);
        dialogueRunner.AddCommandHandler("startsketch", ShowSketchInstruction);
        dialogueRunner.AddCommandHandler("the_end", TriggerEndGameEvent);
        dialogueRunner.AddCommandHandler<bool>("option", InOptionView);
    }   
    void OnDisable()=> currMode = CurrentMode.StartMenu;
    void OnModeChanged(CurrentMode mode)
    {
        lastMode = currMode;
        _currMode = mode;
        switch (mode)
        {
            case CurrentMode.Nothing:
                LockCursor(true);
                if(DoorInteraction.indoor) EnableWardrobeAction(true);
                inputManager.EnableChatMoveBtn(true);
                audioManager.SetMuffleParameter(0f);
                return;
            case CurrentMode.Conversation:
                EnableWardrobeAction(false);
                interactIndicator.DisplayIndicator(false);
                LockCursor(true);
                inputManager.EnableChatMoveBtn(false);
                return;
            case CurrentMode.Sketching:
                inputManager.EnableChatMoveBtn(false);
                StartCoroutine(WaitBeforeSketch());
                audioManager.SetMuffleParameter(0.5f);
                return;
            case CurrentMode.Changing:
                LockCursor(false);
                inputManager.EnableChatMoveBtn(false);
                audioManager.SetMuffleParameter(1f);
                return;
            case CurrentMode.StartMenu:
                LockCursor(false);
                inputManager.EnableAllInput(false);
                audioManager.SetMuffleParameter(1f);
                return;
        }
    }
    public void BackToLastMode()
    {
        currMode = lastMode;
    }

    public bool CanFreelyInteract()
    {
        return currMode == CurrentMode.Nothing;
    }
    public bool InConversation()
    {
        return currMode == CurrentMode.Conversation;
    }
    #region SKETCHING PHASE
    public void ContinueSketchChat()
    {
        sketchSubject.StartSketchConversation();
        float count;
        variableStorage.TryGetValue("$StrokeCount", out count);
    }
    // yarn command 'sketch'
    public void OpenCloseSketchbook(bool open)
    {
        ShowPlayer(!open);
        if(open) 
        {
            sketchManager.PrepareToSketch(sketchSubject);
            StartCoroutine(OpenSketchbook());
        }
        else {
            sketchbookUI.SetActive(false);
            audioManager.PlayOneShot(FMODEvents.Instance.bookClose);
        }

        sketchbookOpen = open;
        EnableWardrobeAction(!open);
    }
    IEnumerator OpenSketchbook()
    {
        yield return new WaitForSeconds(.8f);  //delay shortly before opening sketchbook
        sketchbookUI.SetActive(true);
        audioManager.PlayOneShot(FMODEvents.Instance.bookOpen);
    }
#endregion

#region CONVERSATION PHASE
    public void DiscoveredPronoun()
    {
        sketchSubject.pronounKnown = true;
        sketchSubject.ChangePronounTag();
    }
    public void ShowPronoun() => pronounTag.SetActive(true);
    public void HidePronoun() => pronounTag.SetActive(false);
    
#endregion

#region WARDROBE 

    public void EnableWardrobeAction(bool enable)
    {
        wardrobeBtn.gameObject.SetActive(enable);
    }
#endregion
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
    }
    //command:the_end
    public void TriggerEndGameEvent()
    {
        //show ending screen or start screen
        sceneManager.StartOver();
    }
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
    void InOptionView(bool inView)
    {
        LockCursor(!inView);
        inputManager.EnableChatMoveBtn(!inView);
    }
    public void LockCursor(bool lockCursor)
    {
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
    }
    IEnumerator WaitBeforeSketch(float seconds = .5f)
    {
        yield return new WaitForSeconds(seconds);
        LockCursor(false);
    }
    void DisplayControlInstruction()
    {
        UIManager.Instance.DisplayInstruction("Use WASD/arrow keys to move, \n Mouse to look around.", 4f);
    }
}
