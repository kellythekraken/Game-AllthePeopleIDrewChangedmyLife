using System.Collections;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Collider))]
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
    protected bool interactable 
    {
        get { return _interactable;}
        set { if(!silence) _interactable = value; }
    }
    bool _interactable = true;
    bool silence = false;

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
    protected virtual void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") || !interactable) return;
        InRange = true;
        indicator.DrawRay(name);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        InRange = false;
        indicator.DisplayIndicator(false);
    }
    void InteractionAction(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        if (indicator.facingSubject && InRange && interactable) StartInteraction();
    }
    protected virtual void StartInteraction()
    {
        gm.HidePronoun();
        gm.currMode = CurrentMode.Conversation;
        indicator.currentInteract = this;
        dialogueRunner.StartDialogue(dialogueTitle);
        InputManager.Instance.LockInputOnDialogueStart();
    }
    protected virtual void EndInteraction()
    {
        if(indicator.currentInteract == this)
        {
            StartCoroutine(InteractionCooldown());
            gm.currMode = CurrentMode.Nothing;
        }
    }
    protected IEnumerator InteractionCooldown(float timer = 4f)
    {
        interactable = false;
        yield return new WaitForSeconds(timer);
        interactable = true;
    }

    [YarnCommand("silence")]
    public void DisableConversation()
    {
        interactable = false;
        silence = true;
    }
}
