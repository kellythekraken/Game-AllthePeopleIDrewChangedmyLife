using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerInput playerInput;
    InputActionMap mainAcionMap;

    public InputAction interactAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        mainAcionMap = playerInput.actions.FindActionMap("Player");

        playerInput.onActionTriggered += OnFire;

        interactAction = mainAcionMap.FindAction("Interact");
        //interactAction.performed += ctx => Interact();

        //mainAcionMap["Interact"].performed += ctx => Interact(); 
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

    void OnFire(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx);
    }
    void Interact()
    {
        Debug.Log("interact!");
    }
}
