using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerInput playerInput;

    InputAction interactAction;
    InputActionAsset savedInputAsset;

    InputActionMap gameplayActions;

    void Awake()
    {
        savedInputAsset = playerInput.actions;
        playerInput.onActionTriggered += OnFire;
        gameplayActions = savedInputAsset.FindActionMap("Player");

        interactAction = savedInputAsset.FindAction("Interact");

        interactAction.performed += ctx => Interact();

        //gameplayActions["fire"].performed += ctx=> NormalMethod();
    }

    void OnEnable()
    {
        gameplayActions.Enable();
        //interactAction.Enable();
    }
    void OnDisable()
    {
        gameplayActions.Disable();
    }
/*
    private void Update()
    {
        if (interactAction.triggered) Interact(); // alternative method
    }*/

    void OnFire(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx + " action is pressed");
    }
    void Interact()
    {
        Debug.Log("interact!");
    }
}
