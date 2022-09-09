using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;


public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    [SerializeField] GameObject continueBtn;

    internal InputAction interactAction, dialogueAction;

    PlayerInput playerInput;
    InputActionMap mainAcionMap;
    LineView lineView;

    void Awake()
    {
        Instance = this;
        playerInput = GetComponent<PlayerInput>();
        lineView = FindObjectOfType<LineView>();

        mainAcionMap = playerInput.actions.FindActionMap("Player");

        playerInput.onActionTriggered += OnFire;

        //dialogueAction = mainAcionMap.FindAction("Dialogue");
        //interactAction.performed += ctx => Interact();
        interactAction = mainAcionMap["Interact"];
        dialogueAction = mainAcionMap["Dialogue"];
        dialogueAction.performed += ctx => { if(continueBtn.activeSelf) TriggerNextConversation();};

        
        mainAcionMap["Setting"].performed += ctx => SwitchSettingScreen(); 
    }

    void OnEnable()
    {
        mainAcionMap.Enable();
    }
    void OnDisable()
    {
        mainAcionMap.Disable();
    }
/*
    private void Update()
    {
        if (interactAction.triggered) Interact(); // alternative method
    }*/
    
    public void EnableInteractBtn(bool enable)
    {
        if (enable) interactAction.Enable();
        else interactAction.Disable();
    }
    public void EnableDialogueBtn(bool enable)
    {
        if (enable) dialogueAction.Enable();
        else dialogueAction.Disable();
    }
    void SwitchSettingScreen()
    {
        GameManager.Instance.ToggleSettingScreen();
    }
    void OnFire(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx);
    }
    void TriggerNextConversation()
    {
        lineView.OnContinueClicked();
    }
}
