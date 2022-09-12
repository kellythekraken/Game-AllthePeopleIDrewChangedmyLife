using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.InputSystem;

public class QueerNPC : MonoBehaviour
{
    public Queer queerID;
    
    private GameManager gm;
    private WardrobeButton wardrobeBtn;
    private DialogueRunner dialogueRunner;
    private bool interactable = true;
    private bool inConversation = false;
    private float defaultIndicatorIntensity;
    private DialogueRange range;
    private InputManager inputManager;
    private Transform player;

    public void Start()
    {
        gm = GameManager.Instance;
        dialogueRunner = gm.dialogueRunner;
        inputManager = gm.inputManager;
        range = GetComponentInChildren<DialogueRange>();

        dialogueRunner.onDialogueComplete.AddListener(EndConversation);
        inputManager.chatAction.performed += ctx => { if (range.InRange && !inConversation) StartConversation(); };
    }

    void CheckPlayerDirection()
    {
        Vector3 dir = (transform.position - player.position).normalized;
        float delta = Vector3.Dot(dir, transform.forward);

        // If delta is 1, it's looking directly at the object, -1 is looking directly away
        // A good tolerance would be >= 0.8, then you can interact with the object
    }
    private void StartConversation()
    {
        if(!interactable) return;
        inConversation = true;
        gm.sketchSubject = this;
        gm.currMode = CurrentMode.Conversation;

        dialogueRunner.StartDialogue(queerID.npcName + "Start");
    }

    public void StartSketchConversation()
    {
        gm.currMode = CurrentMode.Conversation;
        inConversation = true;
        dialogueRunner.StartDialogue(queerID.npcName + "Sketch");
    }
    private void EndConversation()
    {
        if (inConversation)
        {
            inConversation = false;
            if(gm.sketchbookOpen) gm.currMode = CurrentMode.Sketching;
            else gm.currMode = CurrentMode.Nothing;
        }
    }

    [YarnCommand("gift")]
    public void GiveItem()
    {
        WardrobeManager.Instance.AddItemToWardrobe(queerID.items[0]);
        wardrobeBtn.DisplayReceivedItem(queerID.npcName, queerID.items[0]);
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
