using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject mainUI, wardrobeUI, sketchbookUI, dialogueUI, newItemWindow;

    private WardrobeButton wardrobeBtn;
    private DialogueRunner dialogueRunner;

    public QueerNPC conversationTarget;

    private void Awake()
    {
        Instance = this;

        dialogueUI.SetActive(true);
        dialogueRunner = dialogueUI.GetComponent<DialogueRunner>();
        dialogueRunner.AddCommandHandler<bool>("sketch",OpenCloseSketchbook);
    }
    private void Start()
    {
        wardrobeBtn = FindObjectOfType<WardrobeButton>();

        OpenCloseSketchbook(false);
    }

    public void ContinueSketchChat()
    {
        conversationTarget.ContinueConversation();
    }
    public void OpenCloseSketchbook(bool open)
    {
        sketchbookUI.SetActive(open);
        mainUI.SetActive(!open);
        //find out the person we're sketching, and send the info the sketching system script.
    }

    //should be called from the queer npc, to access item name and image
    public void ReceivedNewItem(string text)
    {
        wardrobeBtn.ReiceivedItem(text);
    }

}
