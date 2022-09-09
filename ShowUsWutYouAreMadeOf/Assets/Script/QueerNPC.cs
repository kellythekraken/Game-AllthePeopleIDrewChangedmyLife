using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.InputSystem;

public class QueerNPC : MonoBehaviour
{
    public Queer queerID;
    
    private GameManager gm;
    private DialogueRunner dialogueRunner;
    private bool interactable = true;

    private bool inConversation = false;
    private float defaultIndicatorIntensity;

    private DialogueRange range;

    InputManager inputManager;
    Transform player;

    public void Start()
    {
        gm = GameManager.Instance.GetComponent<GameManager>();
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        dialogueRunner.onDialogueComplete.AddListener(EndConversation);
        range = GetComponentInChildren<DialogueRange>();
        inputManager = FindObjectOfType<InputManager>();

        inputManager.interactAction.performed += ctx => { if (range.InRange && !inConversation) StartConversation(); };
        //error was caused by multiple interactable in the scene!!!
    }

    void CheckPlayerDirection()
    {
        Vector3 dir = (transform.position - player.position).normalized;
        float delta = Vector3.Dot(dir, transform.forward);

        // If delta is 1, it's looking directly at the object, -1 is looking directly away
        // A good tolerance would be >= 0.8, then you can interact with the object
    }
    private void StartConversation()
    {
        inConversation = true;
        gm.sketchSubject = this;
        gm.currMode = CurrentMode.Conversation;

        dialogueRunner.StartDialogue(queerID.npcName + "Start");
    }

    public void StartSketchConversation()
    {
        gm.currMode = CurrentMode.Conversation;
        inConversation = true;
        dialogueRunner.StartDialogue(queerID.npcName + "Sketch");
    }
    private void EndConversation()
    {
        if (inConversation)
        {
            inConversation = false;
            if(gm.sketchbookOpen) gm.currMode = CurrentMode.Sketching;
            else gm.currMode = CurrentMode.Nothing;
        }
    }

    //[YarnCommand("disable")]
    public void DisableConversation()
    {
        interactable = false;
    }
    
    [YarnCommand("enter")]
    public void OnStage()
    {
        gameObject.SetActive(true);
    }

    [YarnCommand("leave")]
    public void LeaveStage()
    {
        gameObject.SetActive(false);
    }
}
