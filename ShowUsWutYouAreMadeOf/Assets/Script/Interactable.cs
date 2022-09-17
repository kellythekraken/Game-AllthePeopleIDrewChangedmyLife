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
    [TextArea(1,3)][SerializeField] private string notification;
    [SerializeField]float centerFacingBias = 0.3f;
    protected DialogueRunner dialogueRunner;
    protected GameManager gm;
    protected InteractIndicator indicator;
    private Collider _collider;
    protected bool InRange = false;
    private Transform playerTransform;
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
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        InRange = true;
        indicator.ChangeText(name); 
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        InRange = true;
        indicator.CheckFaceDir(transform,centerFacingBias);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        InRange = false;
        indicator.DisplayIndicator(false);
    }
    protected virtual void StartInteraction()
    {
        InputManager.Instance.LockInputOnDialogueStart();
        indicator.currentInteract = this;
        switch(type)
        {
            case InteractType.Dialogue:
            dialogueRunner.StartDialogue(dialogueTitle);
            gm.currMode = CurrentMode.Conversation;
            break;
            case InteractType.Notification:
            UIManager.Instance.DisplayNotification(notification);
            break;
        }
    }

    protected virtual void EndInteraction()
    {
        if(indicator.currentInteract == this)
        {
            if(gm.sketchbookOpen) gm.currMode = CurrentMode.Sketching;
            else gm.currMode = CurrentMode.Nothing;
        }
    }
}
