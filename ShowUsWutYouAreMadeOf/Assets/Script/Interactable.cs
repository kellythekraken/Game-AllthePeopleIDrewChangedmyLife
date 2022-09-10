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
    
    DialogueRunner dialogueRunner;
    Collider _collider;
    bool InRange = false;

    void Start()
    {
        dialogueRunner = GameManager.Instance.dialogueRunner;
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        InputManager.Instance.chatAction.performed += ctx => { if (InRange && GameManager.Instance.currMode == CurrentMode.Nothing) 
        TriggerInteraction(); };
        dialogueRunner.onDialogueComplete.AddListener(FinishedInteraction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) InRange = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) InRange = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) InRange = false;
    }

    void TriggerInteraction()
    {
        switch(type)
        {
            case InteractType.Dialogue:
            dialogueRunner.StartDialogue(dialogueTitle);
            break;
            case InteractType.Notification:
            break;
        }
        GameManager.Instance.currMode = CurrentMode.Conversation;
    }

    void FinishedInteraction()
    {
        GameManager.Instance.currMode = CurrentMode.Nothing;
    }
}
