using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    [SerializeField] GameObject continueBtn;
    internal InputAction continueAction, interactAction, lookAction, wardrobeAction, settingsAction;
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
        mainAcionMap = playerInput.actions.FindActionMap("Player");
        continueAction = mainAcionMap["ContinueDialogue"];
        interactAction = mainAcionMap["Interact"];
        moveAction = mainAcionMap["Move"];
        lookAction = mainAcionMap["Look"];
        wardrobeAction = mainAcionMap["Wardrobe"];
        settingsAction = mainAcionMap["Setting"];

        settingsAction.performed += SwitchSettingScreen; 
        wardrobeAction.performed += WardrobeButton.Instance.WardrobeAction;
        mainAcionMap.Enable();
        gm = GameManager.Instance;
    }
    void OnDisable()
    {
        settingsAction.performed -= SwitchSettingScreen; 
        wardrobeAction.performed -= WardrobeButton.Instance.WardrobeAction;
        mainAcionMap.Disable();
    }
    public void EnableAllInput(bool enable)   //when in start screen
    {
        if(enable) mainAcionMap.Enable();
        else mainAcionMap.Disable();
    }
    
    public void EnableWardrobeAction(bool enable)
    {
        if(enable) wardrobeAction.Enable();
        else {wardrobeAction.Disable();}
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
}
