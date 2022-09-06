using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{
    public GameObject mainUI, wardrobeUI, sketchbookUI, newItemWindow;

    private WardrobeButton wardrobeBtn;
    private DialogueRunner dialogueRunner;

    private void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        dialogueRunner.AddCommandHandler<bool>("sketch",OpenCloseSketchbook);
        dialogueRunner.AddCommandHandler<string,string>("gift", ReceivedNewItem);
    }
    private void Start()
    {
        wardrobeBtn = FindObjectOfType<WardrobeButton>();

        OpenCloseSketchbook(false);
    }

    public void OpenCloseSketchbook(bool open)
    {
        sketchbookUI.SetActive(open);
        mainUI.SetActive(!open);
    }

    //should be called from the queer npc, to access item name and image
    public void ReceivedNewItem(string name, string item)
    {
        string text = string.Format("You received {1} from {0}!", name, item);
        Debug.Log(text);
        wardrobeBtn.ReiceivedItem(text);
    }

}
