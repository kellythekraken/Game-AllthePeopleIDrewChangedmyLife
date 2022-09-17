using UnityEngine;
using Yarn.Unity;

public class QueerNPC : Interactable
{
    public Queer queerID;
    private bool interactable = true;
    private bool introduced = false;
    private Animator _animator;

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
        if (!other.CompareTag("Player")) return;
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
        Debug.Log(name + " change pose to" + anim);
        _animator.CrossFade(anim,12f,0);
    }

    [YarnCommand("gift")]
    public void GiveItem()
    {
        WardrobeManager.Instance.AddItemToWardrobe(queerID.items[0]);
        gm.wardrobeBtn.DisplayReceivedItem(queerID.npcName, queerID.items[0]);
        //for multiple items
/*        for (int i =0 ; i < queer.items.Length ; i++)
        {
            string text = string.Format("You received {0} from {1}!", queer.items[i].name, queer.npcName);
            Sprite image = queer.items[i].icon;
            Debug.Log(text);
            wardrobeBtn.DisplayReceivedItem(text,image);
        }*/
    }

    [YarnCommand("silence")]
    public void DisableConversation()
    {
        interactable = false;
    }
}
