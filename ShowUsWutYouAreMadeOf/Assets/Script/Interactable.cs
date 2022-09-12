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
    Collider _collider;
    [SerializeField] bool InRange = false;
    private Transform playerTransform;

    protected virtual void Start()
    {
        gm = GameManager.Instance;
        dialogueRunner = gm.dialogueRunner;
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        InputManager.Instance.chatAction.performed += ctx => { if (InRange && gm.currMode == CurrentMode.Nothing) 
        StartInteraction(); };
        dialogueRunner.onDialogueComplete.AddListener(EndInteraction);
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
        switch(type)
        {
            case InteractType.Dialogue:
            dialogueRunner.StartDialogue(dialogueTitle);
            break;
            case InteractType.Notification:
            Debug.Log("show notice window!");
            break;
        }
        gm.currMode = CurrentMode.Conversation;
    }

    protected virtual void EndInteraction()
    {
        gm.currMode = CurrentMode.Nothing;
    }
}
