using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Dialogue
{
    [Multiline] public string DialogueLine;
}

[RequireComponent(typeof(BoxCollider))]
public class NPCDialogue : MonoBehaviour
{
    //text color?
    //name tag bg color
    //chat progress? continue to the next set of dialogues

    public float TransitionTime = 0.2f;
    public string npcName;
    public Color nameTag;
    public Dialogue[] Dialogues;

    protected int _currentIndex;    //in current dialogue
    protected bool _talking = false;
    protected bool _available = true;
    protected WaitForSeconds _transitionTimeWFS;

    DialogueManager dManager;
    BoxCollider _collider;
    bool inDialogueRange = false;

    private void OnEnable()
    {
        dManager = FindObjectOfType<DialogueManager>();
        _currentIndex = 0;
        _transitionTimeWFS = new WaitForSeconds(TransitionTime);

        _collider = gameObject.GetComponent<BoxCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("player in range");
            //inDialogueRange = true;

            if(Input.GetKey(KeyCode.A))
            {
                TriggerDialogue();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //inDialogueRange = false;
        }
    }

    public void TriggerDialogue()
    {
        if (!_available || _talking) return;

        Debug.Log("Start dialogue");

        OpenDialogue();
    }

    public void OpenDialogue()
    {
        // if it's not already playing, we'll initialize the dialogue box
        if (!_talking)
        {
            //_dialogueBox = Instantiate(DialogueBoxPrefab);
            dManager.DisplayDialogue(true);
            _talking = true;
        }
        // we start the next dialogue
        dManager.ChangeDialogueName(npcName, nameTag);
        StartCoroutine(PlayNextDialogue());
    }

    protected IEnumerator PlayNextDialogue()
    {
        Debug.Log("playing next line");
        // if we've reached the last dialogue line, we exit
        if (_currentIndex >= Dialogues.Length)
        {
            _currentIndex = 0;
            
            //hide ui

            _talking = false;
            yield break;
        }
        dManager.ChangeDialogueText(Dialogues[_currentIndex].DialogueLine);

        _currentIndex++;
    }

}
