using UnityEngine;
using Yarn.Unity;
using System.Collections;

public class QueerNPC : Interactable
{
    public Queer queerID;
    public SketchFocusBodypart[] sketchableAreas; //load all the scripts, for the sketching system to access
    private bool introduced = false;
    private Animator _animator;
    
    void OnEnable() => introduced = false;
    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
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
    [YarnCommand("pose")]
    void ChangePose(string anim)
    {
        _animator.CrossFade(anim,12f,0);
    }

    [YarnCommand("gift")]
    public void GiveItem()
    {
        gm.wardrobeBtn.DisplayReceivedItem(queerID.giftLine, queerID.items);
    }

    [YarnCommand("silence")]
    public void DisableConversation()
    {
        interactable = false;
    }
}
