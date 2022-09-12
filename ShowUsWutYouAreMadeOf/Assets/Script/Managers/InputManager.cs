using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;


public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    [SerializeField] GameObject continueBtn;

    internal InputAction interactAction, chatAction, lookAction, wardrobeAction;
    InputAction moveAction;
    PlayerInput playerInput;
    InputActionMap mainAcionMap;

    void Awake()
    {
        Instance = this;
        playerInput = FindObjectOfType<PlayerInput>();

        mainAcionMap = playerInput.actions.FindActionMap("Player");

        playerInput.onActionTriggered += OnFire;

        //dialogueAction = mainAcionMap.FindAction("Dialogue");
        //interactAction.performed += ctx => Interact();
        interactAction = mainAcionMap["Interact"];
        chatAction = mainAcionMap["Chat"];
        moveAction = mainAcionMap["Move"];
        lookAction = mainAcionMap["Look"];
        wardrobeAction = mainAcionMap["Wardrobe"];
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
    public void EnableChatMoveBtn(bool enable)
    {
        if (enable) 
        {
            chatAction.Enable();
            moveAction.Enable();
            lookAction.Enable();
        }
        else 
        {
            chatAction.Disable();
            moveAction.Disable();
            lookAction.Disable();
        }
    }

    public void LockInputOnDialogueStart()
    {
        StartCoroutine(LockInputCoroutine());
    }
    IEnumerator LockInputCoroutine()    //lock the input on dialogue start, to avoid instant skip of first line
    {
        mainAcionMap.Disable();
        yield return new WaitForSeconds(.7f);
        mainAcionMap.Enable();
    }
    void SwitchSettingScreen()
    {
        GameManager.Instance.ToggleSettingScreen();
    }
    void OnFire(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx);
    }
}
