using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;


public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    [SerializeField] GameObject continueBtn;

    internal InputAction interactAction;
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
        moveAction = mainAcionMap["Move"];
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
    
    public void EnablePlayerInput(bool enable)
    {
        if(enable) 
        {
        }
    }
    public void EnableInteractBtn(bool enable)
    {
        if (enable) 
        {
            interactAction.Enable();
            //moveAction.Enable();
        }
        else 
        {
            interactAction.Disable();
            //moveAction.Disable();
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
