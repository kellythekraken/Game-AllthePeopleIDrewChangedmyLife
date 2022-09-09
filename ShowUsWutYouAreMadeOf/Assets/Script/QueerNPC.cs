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
    private bool isCurrentConversation = false;
    private float defaultIndicatorIntensity;

    private DialogueRange range;

    InputAction interactBtn;

    public void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        dialogueRunner.onDialogueComplete.AddListener(EndConversation);
        lightIndicatorObject = GetComponentInChildren<Light>();
        range = GetComponentInChildren<DialogueRange>();

        // get starter intensity of light then
        // if we're using it as an indicator => hide it 
        if (lightIndicatorObject != null)
        {
            defaultIndicatorIntensity = lightIndicatorObject.intensity;
            lightIndicatorObject.intensity = 0;
        }

        //interactBtn = InputAction()
    }

/*    private void Update()
    {
        if(Input.GetKey(KeyCode.E))
        {
            if(range.InRange) StartConversation();
        }
    }*/
    private void StartConversation()
    {
        isCurrentConversation = true;
        GameManager.Instance.sketchSubject = this;

        if (lightIndicatorObject != null)
        {
            lightIndicatorObject.intensity = defaultIndicatorIntensity;
        }
        dialogueRunner.StartDialogue(queerID.npcName + "Start");
    }

    public void ContinueConversation()
    {
        dialogueRunner.StartDialogue(queerID.npcName + "Sketch");
    }
    private void EndConversation()
    {
        if (isCurrentConversation)
        {
            if (lightIndicatorObject != null)
            {
                lightIndicatorObject.intensity = 0;
            }
            isCurrentConversation = false;
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
