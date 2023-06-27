using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    [SerializeField] GameObject continueBtn;
    internal InputAction continueAction, interactAction, lookAction, wardrobeAction, settingsAction, quickRestartAction;
    InputAction moveAction;
    [SerializeField] PlayerInput playerInput;
    InputActionMap mainAcionMap;
    GameManager gm;
    
    void Awake()
    {
        Instance = this;
    }
    void OnEnable()
    {
        gm = GameManager.Instance;
        mainAcionMap = gm.demoBuild? playerInput.actions.FindActionMap("DemoMain") : playerInput.actions.FindActionMap("Player") ;

        continueAction = mainAcionMap["ContinueDialogue"];  
        interactAction = mainAcionMap["Interact"];
        moveAction = mainAcionMap["Move"];
        lookAction = mainAcionMap["Look"];
        wardrobeAction = mainAcionMap["Wardrobe"];
        if(!gm.demoBuild) 
        {
            settingsAction = mainAcionMap["Setting"]; 
            settingsAction.performed += SwitchSettingScreen;
        }
        else
        {
            quickRestartAction = playerInput.actions.FindActionMap("DemoKey") ["Restart"];
            quickRestartAction.performed += RestartDemo;
        }
        wardrobeAction.performed += gm.wardrobeBtn.WardrobeAction; 
        mainAcionMap.Enable();
    }
    void OnDisable()
    {
        if(!gm.demoBuild) settingsAction.performed -= SwitchSettingScreen; 
        else { quickRestartAction.performed -= RestartDemo; }
        
        wardrobeAction.performed -= gm.wardrobeBtn.WardrobeAction;
        mainAcionMap.Disable();
    }
    public void EnableAllInput(bool enable)   //when in start screen
    {
        if(enable) mainAcionMap.Enable();
        else {mainAcionMap.Disable();}
    }
    public void EnableChatMoveBtn(bool enable)
    {
        if (enable) 
        {
            interactAction.Enable();
            lookAction.Enable();
            moveAction.Enable();
        }
        else 
        {
            interactAction.Disable();
            lookAction.Disable();
            moveAction.Disable();
        }
    }

    public void EnableInteractBtn(bool enable)
    {
        if (enable) 
            interactAction.Enable();
        else 
            interactAction.Disable();
    }
    public void LockInputOnDialogueStart() 
    {
        StartCoroutine(LockInputCoroutine());
    }
    IEnumerator LockInputCoroutine()    //lock the input on dialogue start, to avoid instant skip of first line
    {
        continueAction.Disable();
        yield return new WaitForSeconds(.7f);
        continueAction.Enable();
    }
    void SwitchSettingScreen(InputAction.CallbackContext ctx)
    {
        gm.ToggleSettingScreen();
    }

    //ESC to restart the demo for exhibition build
    void RestartDemo(InputAction.CallbackContext ctx)
    {
        SceneManager.Instance.ActivateStartMenu();
    }
}
