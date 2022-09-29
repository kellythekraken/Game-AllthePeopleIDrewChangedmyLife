using UnityEngine;
using Yarn.Unity;
using System.Collections;

public class QueerNPC : Interactable
{
    public Queer queerID;
    public SketchFocusBodypart[] sketchableAreas; //load all the scripts, for the sketching system to access
    private bool introduced = false;
    private Animator _animator;
    internal bool alreadySketched, alreadyGifted;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        alreadyGifted = alreadySketched = false;
    }
    protected override void StartInteraction()
    {
        if(!interactable) return;
        base.StartInteraction();
        introduced = true;
        gm.sketchSubject = this;
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !interactable) return;
        InRange = true;
        var displayTxt = introduced? queerID.npcName : "Chat"; 
        indicator.ChangeText(displayTxt); 
    }

    public void StartSketchConversation()
    {
        gm.currMode = CurrentMode.Conversation;
        dialogueRunner.StartDialogue(queerID.npcName + "Sketch");
    }

    protected override void EndInteraction()
    {
        if(indicator.currentInteract == this)
        {
            if(!silence) {StartCoroutine(TimerBeforeNextInteraction());}
            if(gm.sketchbookOpen) gm.currMode = CurrentMode.Sketching;
            else if (!alreadySketched) { Debug.Log("not sketched, change mode"); gm.currMode = CurrentMode.Nothing;} 
            else if (alreadyGifted){ Debug.Log("gifted, change mode"); gm.currMode = CurrentMode.Nothing;} 
            //if the sketch has been initiated, then they will give an item after sketching. 
            //so we do nothing and wait for the wardrobe btn to change mode.
        }
    }

    [YarnCommand("pose")]
    void ChangePose(string anim)
    {
        _animator.CrossFade(anim,12f,0);
    }

    [YarnCommand("gift")]
    public void GiveItem()
    {
        gm.wardrobeBtn.DisplayReceivedItem(this);
    }
    bool silence = false;
    [YarnCommand("silence")]
    public void DisableConversation()
    {
        silence = true;
    }
}
