using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.InputSystem;

public class QueerNPC : Interactable
{
    public Queer queerID;
    private WardrobeButton wardrobeBtn;
    private bool interactable = true;
    protected override void StartInteraction()
    {
        if(!interactable) return;
        base.StartInteraction();
        gm.sketchSubject = this;
    }
    public void StartSketchConversation()
    {
        gm.currMode = CurrentMode.Conversation;
        dialogueRunner.StartDialogue(queerID.npcName + "Sketch");
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
