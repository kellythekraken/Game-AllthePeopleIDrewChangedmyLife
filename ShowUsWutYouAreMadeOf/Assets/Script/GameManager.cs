using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject mainUI, wardrobeUI, sketchbookUI, newItemWindow;

    private WardrobeButton wardrobeBtn;
    private DialogueRunner dialogueRunner;

    private void Awake()
    {
        Instance = this;

        dialogueRunner = FindObjectOfType<DialogueRunner>();
        dialogueRunner.AddCommandHandler<bool>("sketch",OpenCloseSketchbook);
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
    public void ReceivedNewItem(string text)
    {
        wardrobeBtn.ReiceivedItem(text);
    }

}
