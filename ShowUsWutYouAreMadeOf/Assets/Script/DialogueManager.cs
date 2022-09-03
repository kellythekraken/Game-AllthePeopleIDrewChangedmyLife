using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueUI;
    public Image nameBackground;
    public TextMeshProUGUI nameText,dialogueText;


    private void OnEnable()
    {
        dialogueUI.SetActive(false);
    }
    public void DisplayDialogue(bool _bool)
    {
        dialogueUI.SetActive(_bool);
    }
    public void ChangeDialogueName(string name, Color color)
    {
        //change the color and name of the dialogue box
        nameBackground.color = color;
        nameText.text = name;
    }

    public void ChangeDialogueText(string text)
    {
        dialogueText.text = text;
    }
}
