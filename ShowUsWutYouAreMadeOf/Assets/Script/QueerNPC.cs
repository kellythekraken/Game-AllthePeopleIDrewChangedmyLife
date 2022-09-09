using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.InputSystem;

public class QueerNPC : MonoBehaviour
{
    public Queer queerID;
    
    private DialogueRunner dialogueRunner;
    private Light lightIndicatorObject = null;
    private bool interactable = true;

    private bool inConversation = false;
    private float defaultIndicatorIntensity;

    private DialogueRange range;

    InputManager inputManager;
    Transform player;

    public void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        dialogueRunner.onDialogueComplete.AddListener(EndConversation);
        lightIndicatorObject = GetComponentInChildren<Light>();
        range = GetComponentInChildren<DialogueRange>();
        inputManager = FindObjectOfType<InputManager>();

        // get starter intensity of light then
        // if we're using it as an indicator => hide it 
        if (lightIndicatorObject != null)
        {
            defaultIndicatorIntensity = lightIndicatorObject.intensity;
            lightIndicatorObject.intensity = 0;
        }
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
        GameManager.Instance.sketchSubject = this;
        GameManager.Instance.currMode = CurrentMode.Conversation;

        if (lightIndicatorObject != null)
        {
            lightIndicatorObject.intensity = defaultIndicatorIntensity;
        }
        dialogueRunner.StartDialogue(queerID.npcName + "Start");
    }

    public void StartSketchConversation()
    {
        dialogueRunner.StartDialogue(queerID.npcName + "Sketch");
    }
    private void EndConversation()
    {
        if (inConversation)
        {
            if (lightIndicatorObject != null)
            {
                lightIndicatorObject.intensity = 0;
            }
            inConversation = false;
            GameManager.Instance.currMode = CurrentMode.Nothing;
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
