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
    public GameObject pronounTag;
    [SerializeField] private NPCManager npcManager;
    [SerializeField] public GameObject playerObject;   //dont reference the actual player with important scripts!
    internal SketchingSystem sketchManager;
    internal WardrobeButton wardrobeBtn;
    internal SceneManager sceneManager;
    internal InputManager inputManager;
    internal AudioManager audioManager;
    internal TextMeshProUGUI pronounText;
    internal QueerNPC sketchSubject;
    internal bool sketchbookOpen;
    internal bool inConversation;
    internal Camera mainCam;
    private void Awake()
    {
        Instance = this;
        dialogueUI.SetActive(true);
        mainCam = Camera.main;
        inputManager = FindObjectOfType<InputManager>();
        wardrobeBtn = WardrobeButton.Instance;
        sketchManager = SketchingSystem.Instance;
        sceneManager = SceneManager.Instance;
        dialogueRunner.AddCommandHandler<bool>("sketch",OpenCloseSketchbook);
        //dialogueRunner.AddCommandHandler("gift", GiveItem);
        dialogueRunner.AddCommandHandler("pronoun", DiscoveredPronoun);
        dialogueRunner.AddCommandHandler<string>("enter", npcManager.OnStage);
        dialogueRunner.AddCommandHandler<string>("leave", npcManager.OffStage);
        dialogueRunner.AddCommandHandler("randomEnter", npcManager.OnStageRandom);
        dialogueRunner.AddCommandHandler("the_end", TriggerEndGameEvent);
        dialogueRunner.AddCommandHandler<bool>("option", InOptionView);
    }
    private void Start()
    {
        audioManager = AudioManager.Instance;
        currMode = CurrentMode.Nothing;
        pronounText = pronounTag.GetComponentInChildren<TextMeshProUGUI>();
        sketchbookUI.SetActive(false);
        pronounTag.SetActive(false);
        settingsUI.SetActive(false);
        DisplayControlInstruction();
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
                inputManager.EnableChatMoveBtn(true);
                inConversation = false;
                audioManager.SetGlobalParameter("Muffle",0f);
                return;
            case CurrentMode.Conversation:
                LockCursor(true);
                inConversation = true;
                inputManager.EnableChatMoveBtn(false);
                return;
            case CurrentMode.Sketching:
                inConversation = false;
                inputManager.EnableChatMoveBtn(false);
                StartCoroutine(WaitBeforeSketch());
                audioManager.SetGlobalParameter("Muffle",0.5f);
                return;
            case CurrentMode.Changing:
                LockCursor(false);
                inConversation = false;
                inputManager.EnableChatMoveBtn(false);
                audioManager.SetGlobalParameter("Muffle",1f);
                return;
            case CurrentMode.StartMenu:
                LockCursor(false);
                inConversation = false;
                inputManager.EnableAllInput(false);
                audioManager.SetGlobalParameter("Muffle",1f);
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
            sketchManager.PrepareToSketch(sketchSubject);
            StartCoroutine(OpenSketchbook());
        }
        else {
            sketchbookUI.SetActive(false);
            audioManager.PlayOneShot(FMODEvents.Instance.bookClose);
        }

        sketchbookOpen = open;
        wardrobeBtn.gameObject.SetActive(!open);
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
