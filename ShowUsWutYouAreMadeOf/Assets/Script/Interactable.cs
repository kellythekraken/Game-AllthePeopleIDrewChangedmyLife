using System.Collections;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    public string interactName;
    [SerializeField] private string dialogueTitle;
    protected DialogueRunner dialogueRunner;
    protected GameManager gm;
    protected InteractIndicator indicator;
    internal bool interactable 
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
        dialogueRunner.onDialogueComplete.AddListener(EndInteraction);
        //Debug.Log("tags: " + dialogueRunner.GetTagsForNode(dialogueTitle));
    }

    public virtual string GetName() 
    {
        return interactName;
    }

    public virtual void StartInteraction()
    {
        if(!interactable) return;
        gm.HidePronoun();
        gm.currMode = CurrentMode.Conversation;
        indicator.currentInteract = this;
        dialogueRunner.StartDialogue(dialogueTitle);
        gm.inputManager.LockInputOnDialogueStart();
    }
    protected virtual void EndInteraction()
    {
        if(indicator.currentInteract == this)
        {
            StartCoroutine(InteractionCooldown());
            gm.currMode = CurrentMode.Nothing;
        }
    }
    protected IEnumerator InteractionCooldown(float timer = 3f)
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
