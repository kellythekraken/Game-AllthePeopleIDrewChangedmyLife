using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class QueerNPC : MonoBehaviour
{
    [SerializeField] private string conversationStartNode;

    private DialogueRunner dialogueRunner;
    private Light lightIndicatorObject = null;
    private bool interactable = true;
    private bool isCurrentConversation = false;
    private float defaultIndicatorIntensity;

    DialogueRange range;

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
    }

    public void OnMouseDown()
    {
        Debug.Log("mouse down!");
        StartConversation();

        //player should be in the zone
        //interactable && !dialogueRunner.IsDialogueRunning && 
/*        if (range.PlayerInRange())
        {
        }*/
    }

    private void StartConversation()
    {
        Debug.Log($"Started conversation with {name}.");
        isCurrentConversation = true;
        if (lightIndicatorObject != null)
        {
            lightIndicatorObject.intensity = defaultIndicatorIntensity;
        }
        dialogueRunner.StartDialogue(conversationStartNode);
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
}
