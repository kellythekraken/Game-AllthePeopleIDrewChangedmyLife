using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(SphereCollider))]
public class Interactable : MonoBehaviour
{
    enum InteractType {Dialogue,Notification};
    [SerializeField] private InteractType type;
    [SerializeField] private string dialogueTitle;
    protected DialogueRunner dialogueRunner;
    protected GameManager gm;
    protected InteractIndicator indicator;
    private Collider _collider;
    private bool InRange = false;
    private Transform playerTransform;
    private Interactable currentInteract = null;

    protected virtual void Start()
    {
        gm = GameManager.Instance;
        indicator = InteractIndicator.Instance;
        dialogueRunner = gm.dialogueRunner;
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        InputManager.Instance.chatAction.performed += ctx => { if (indicator.facingSubject && InRange) 
        StartInteraction(); };
        dialogueRunner.onDialogueComplete.AddListener(EndInteraction);
    }

    //if in range, communicate with the interactindicator, change the text
    //if face the object, the indicator will show up, and then the condition to interact should THEN be satisfied.
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        InRange = true;
        indicator.ChangeText(name); //if the npc name is unknown, change to 'chat'
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        InRange = true;
        indicator.CheckFaceDir(transform);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        InRange = false;
        indicator.DisplayIndicator(false);
    }
    
    //check player face direction 
    void CheckPlayerDirection()
    {
        Vector3 dir = (transform.position - playerTransform.position).normalized;
        float delta = Vector3.Dot(dir, transform.forward);

        // If delta is 1, it's looking directly at the object, -1 is looking directly away
        // A good tolerance would be >= 0.8, then you can interact with the object
    }
    protected virtual void StartInteraction()
    {
        currentInteract = this;
        switch(type)
        {
            case InteractType.Dialogue:
            dialogueRunner.StartDialogue(dialogueTitle);
            break;
            case InteractType.Notification:
            Debug.Log(name + " want to show notice window!");
            break;
        }
        gm.currMode = CurrentMode.Conversation;
    }

    protected virtual void EndInteraction()
    {
        if(currentInteract == this)
        {
            if(gm.sketchbookOpen) gm.currMode = CurrentMode.Sketching;
            else gm.currMode = CurrentMode.Nothing;
        }
    }
}
