using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using TMPro;

public enum CurrentMode { Nothing, Conversation, Sketching}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private CurrentMode _currMode;
    public CurrentMode currMode
    {
        get { return _currMode; }
        set
        {
            Debug.Log("change mode to " + value);
            _currMode = value;
            OnModeChanged(value);
        }
    }

    public GameObject mainUI, settingsUI, wardrobeUI, sketchbookUI, dialogueUI, newItemWindow;
    [SerializeField] private GameObject pronounTag;

    private SketchingSystem sketchManager;
    private WardrobeButton wardrobeBtn;
    private DialogueRunner dialogueRunner;
    private TextMeshProUGUI pronounText;
    private InputManager inputManager;
    internal QueerNPC sketchSubject;
    internal bool sketchbookOpen;

    private void Awake()
    {
        Instance = this;

        dialogueUI.SetActive(true);
        dialogueRunner = dialogueUI.GetComponent<DialogueRunner>();
        dialogueRunner.AddCommandHandler<bool>("sketch",OpenCloseSketchbook);
        dialogueRunner.AddCommandHandler("gift", GiveItem);
        dialogueRunner.AddCommandHandler("pronoun", ShowPronoun);
    }

    private void Start()
    {
        wardrobeBtn = FindObjectOfType<WardrobeButton>();
        inputManager = FindObjectOfType<InputManager>();
        sketchManager = sketchbookUI.GetComponent<SketchingSystem>();
        pronounText = pronounTag.GetComponentInChildren<TextMeshProUGUI>();

        OpenCloseSketchbook(false);
        pronounTag.SetActive(false);
        settingsUI.SetActive(false);

        LockCursor(true);
    }
    void OnModeChanged(CurrentMode mode)
    {
        switch (mode)
        {
            case CurrentMode.Nothing:
                LockCursor(true);
                inputManager.EnableInteractBtn(true);
                return;
            case CurrentMode.Conversation:
                LockCursor(true);
                inputManager.EnableInteractBtn(false);
                return;
            case CurrentMode.Sketching:
                LockCursor(false);
                inputManager.EnableInteractBtn(false);
                return;
        }
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

    //should be called from the queer npc, to access item name and image
    public void GiveItem()
    {
        Queer queer = sketchSubject.queerID;
        wardrobeBtn.DisplayReceivedItem(queer.name, queer.items[0]);

        //for multiple items
/*        for (int i =0 ; i < queer.items.Length ; i++)
        {
            string text = string.Format("You received {0} from {1}!", queer.items[i].name, queer.npcName);
            Sprite image = queer.items[i].icon;
            Debug.Log(text);
            wardrobeBtn.DisplayReceivedItem(text,image);
        }*/
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
