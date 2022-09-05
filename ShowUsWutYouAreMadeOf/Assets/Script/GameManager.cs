using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject mainUI, wardrobeUI, sketchbookUI, newItemWindow;
    public Button wardrobeOpenBtn,wardrobeCloseBtn;

    private DialogueRunner dialogueRunner;

    private void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        dialogueRunner.AddCommandHandler<bool>("sketch",OpenCloseSketchbook);
        dialogueRunner.AddCommandHandler<string,string>("gift", ReceivedNewItem);

    }
    private void Start()
    {
        wardrobeOpenBtn.onClick.AddListener(() => OpenCloseWardrobe(true));
        wardrobeCloseBtn.onClick.AddListener(() => OpenCloseWardrobe(false));

        OpenCloseWardrobe(false);
        OpenCloseSketchbook(false);
        ItemPopupWindow();

    }
    public void OpenCloseSketchbook(bool open)
    {
        sketchbookUI.SetActive(open);
        wardrobeOpenBtn.gameObject.SetActive(!open);
    }

    void OpenCloseWardrobe(bool open)
    {
        mainUI.SetActive(!open);
        wardrobeUI.SetActive(open);
    }
    public void ReceivedNewItem(string name, string item)
    {
        ItemPopupWindow(true);
        string text = string.Format("You received {1} from {0}!", name, item);

        newItemWindow.GetComponentInChildren<TextMeshProUGUI>().text = text;
//        Debug.Log(text);
    }

    public void ItemPopupWindow(bool open = false)
    {
        newItemWindow.SetActive(open);
    }

}
