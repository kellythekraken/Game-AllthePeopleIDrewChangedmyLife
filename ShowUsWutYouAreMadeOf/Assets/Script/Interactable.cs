using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(SphereCollider))]
public class Interactable : MonoBehaviour
{
    [SerializeField] private string dialogueTitle;
    [TextArea(1,3)][SerializeField] private string notification;
    [SerializeField] private float centerFacingBias = 0.3f;
    protected DialogueRunner dialogueRunner;
    protected GameManager gm;
    protected InteractIndicator indicator;
    private Collider _collider;
    protected bool InRange = false;
    private Transform playerTransform;
    protected bool interactable = true;

    protected virtual void Start()
    {
        gm = GameManager.Instance;
        indicator = InteractIndicator.Instance;
        dialogueRunner = gm.dialogueRunner;
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        InputManager.Instance.interactAction.performed += InteractionAction;
        dialogueRunner.onDialogueComplete.AddListener(EndInteraction);
    }

    void OnDisable()
    {
        InputManager.Instance.interactAction.performed -= InteractionAction;
    }

    //if in range, communicate with the interactindicator, change the text
    //if face the object, the indicator will show up, and then the condition to interact should THEN be satisfied.
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !interactable) return;
        InRange = true;
        indicator.ChangeText(name); 
    }
    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") || !interactable) return;
        InRange = true;
        indicator.CheckFaceDir(transform,centerFacingBias);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        InRange = false;
        indicator.DisplayIndicator(false);
    }
    public void InteractionAction(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        StartInteraction();
    }
    protected virtual void StartInteraction()
    {
        if (indicator.facingSubject && InRange) 
        {
            gm.currMode = CurrentMode.Conversation;
            indicator.currentInteract = this;
            dialogueRunner.StartDialogue(dialogueTitle);
            InputManager.Instance.LockInputOnDialogueStart();
        }
    }
    protected virtual void EndInteraction()
    {
        if(indicator.currentInteract == this)
        {
            StartCoroutine(TimerBeforeNextInteraction());
            gm.currMode = CurrentMode.Nothing;
        }
    }
    protected IEnumerator TimerBeforeNextInteraction()
    {
        interactable = false;
        yield return new WaitForSeconds(5f);
        interactable = true;
    }
}
