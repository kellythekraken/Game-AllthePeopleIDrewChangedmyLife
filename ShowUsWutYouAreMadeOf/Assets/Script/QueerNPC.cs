using UnityEngine;
using Yarn.Unity;
using System.Collections;

public class QueerNPC : Interactable
{
    public Queer queerID;
    public SketchFocusBodypart[] sketchableAreas; //load all the scripts, for the sketching system to access
    public SkinnedMeshRenderer[] bodyPartMeshes;    //automate reference for the sketchfocusbodypart
    internal bool introduced, pronounKnown;
    internal bool alreadySketched, alreadyGifted;
    Animator _animator;
    protected override void Start() 
    {
        base.Start();
        _animator = GetComponent<Animator>();
        alreadyGifted = alreadySketched = false;
        foreach(var i in sketchableAreas) i.enabled = false;
    }

    public override void StartInteraction()
    {
        if(!interactable) return;

        if(alreadyGifted)
        {
            bool wearingGiftedItem = WardrobeManager.Instance.IsWearingGiftedItem(queerID.npcName);
            GameManager.Instance.variableStorage.SetValue("$SpecialDialogue", wearingGiftedItem);
        }
        base.StartInteraction();
        if(indicator.currentInteract == this)
        {
            if(pronounKnown) ChangePronounTag();
            introduced = true;
            gm.sketchSubject = this;
        }
    }
    //IsDialogueRunning after first interaction. because this is when you start sketching.

    public void ChangePronounTag()
    {
        gm.pronounText.text = queerID.pronouns;
        gm.ShowPronoun();
    }
     public override string GetName() 
     {
        return introduced? queerID.npcName : "Chat"; 
     }

    public void StartSketchConversation()
    {
        gm.currMode = CurrentMode.Conversation;
        dialogueRunner.StartDialogue(queerID.npcName + "Sketch");
        if(pronounKnown) ChangePronounTag();
    }

    protected override void EndInteraction()
    {
        if(indicator.currentInteract == this)
        {
            StartCoroutine(InteractionCooldown());
            if(gm.sketchbookOpen) gm.currMode = CurrentMode.Sketching;
            else if (!alreadySketched) { gm.currMode = CurrentMode.Nothing;} 
            else if (alreadyGifted){ gm.currMode = CurrentMode.Nothing;} 
            //if the sketch has been initiated, then they will give an item after sketching. 
            //so we do nothing and wait for the wardrobe btn to change mode.
        }
    }

    [YarnCommand("pose")]
    void ChangePose(string anim, float transitionTime = 1f)
    {
        if(transitionTime!=1f)
        {
            StartCoroutine(DelayedAnim(anim,transitionTime));
        }
        else {_animator.CrossFadeInFixedTime(anim,1f,0);}
    }
    
    IEnumerator DelayedAnim(string anim, float transitionTime)
    {
        var randWait = Random.Range(.3f,.7f);
        yield return new WaitForSeconds(randWait);
        _animator.CrossFadeInFixedTime(anim,transitionTime,0);
    }
        
    [YarnCommand("gift")]
    public void GiveItem()
    {
        gm.wardrobeBtn.DisplayReceivedItem(this);
    }

}
