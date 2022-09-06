using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject mainUI, wardrobeUI, sketchbookUI, dialogueUI, newItemWindow;

    private SketchingSystem sketchManager;
    private WardrobeButton wardrobeBtn;
    private DialogueRunner dialogueRunner;

    internal QueerNPC sketchSubject;

    private void Awake()
    {
        Instance = this;

        dialogueUI.SetActive(true);
        dialogueRunner = dialogueUI.GetComponent<DialogueRunner>();
        dialogueRunner.AddCommandHandler<bool>("sketch",OpenCloseSketchbook);
        dialogueRunner.AddCommandHandler("gift", GiveItem);

    }
    private void Start()
    {
        wardrobeBtn = FindObjectOfType<WardrobeButton>();
        sketchManager = FindObjectOfType<SketchingSystem>();
        OpenCloseSketchbook(false);
    }

    public void ContinueSketchChat()
    {
        sketchSubject.ContinueConversation();
    }
    public void OpenCloseSketchbook(bool open)
    {
        sketchbookUI.SetActive(open);
        mainUI.SetActive(!open);

        if(open) sketchManager.PrepareToSketch(sketchSubject.queerID);
    }

    //should be called from the queer npc, to access item name and image
    public void GiveItem()
    {
        var queer = sketchSubject.queerID;
        string text = string.Format("You received {0} from {1}!", queer.items[0].name, queer.npcName);
        Sprite image = queer.items[0].icon;
        Debug.Log(text);
        wardrobeBtn.DisplayReceivedItem(text, image);

        //for multiple items
/*        for (int i =0 ; i < queer.items.Length ; i++)
        {
            string text = string.Format("You received {0} from {1}!", queer.items[i].name, queer.npcName);
            Sprite image = queer.items[i].icon;
            Debug.Log(text);
            wardrobeBtn.DisplayReceivedItem(text,image);
        }*/

    }

}
