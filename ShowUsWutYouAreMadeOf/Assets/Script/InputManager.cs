using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;


public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    [SerializeField] GameObject continueBtn;

    internal InputAction interactAction, chatAction, wardrobeAction;
    InputAction moveAction;

    PlayerInput playerInput;
    InputActionMap mainAcionMap;

    void Awake()
    {
        Instance = this;
        playerInput = GetComponent<PlayerInput>();

        mainAcionMap = playerInput.actions.FindActionMap("Player");

        playerInput.onActionTriggered += OnFire;

        //dialogueAction = mainAcionMap.FindAction("Dialogue");
        //interactAction.performed += ctx => Interact();
        interactAction = mainAcionMap["Interact"];
        chatAction = mainAcionMap["Chat"];
        moveAction = mainAcionMap["Move"];
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
/*
    private void Update()
    {
        if (interactAction.triggered) Interact(); // alternative method
    }*/
    public void EnableChatMoveBtn(bool enable)
    {
        if (enable) 
        {
            chatAction.Enable();
            moveAction.Enable();
        }
        else 
        {
            chatAction.Disable();
            moveAction.Disable();
        }
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
