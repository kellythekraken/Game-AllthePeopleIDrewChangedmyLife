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
    private InputManager inputManager;
    private Transform player;

    public void Start()
    {
        gm = GameManager.Instance;
        dialogueRunner = gm.dialogueRunner;
        range = GetComponentInChildren<DialogueRange>();
        inputManager = FindObjectOfType<InputManager>();

        dialogueRunner.onDialogueComplete.AddListener(EndConversation);
        inputManager.chatAction.performed += ctx => { if (range.InRange && !inConversation) StartConversation(); };
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
        //make sure only one script get executed, not all of them.
        if(!interactable) return;
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

    [YarnCommand("silence")]
    public void DisableConversation()
    {
        interactable = false;
    }
}
