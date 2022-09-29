using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractIndicator : MonoBehaviour
{
    //to be displayed on world canvas
    //take info from interactables, check facing direction
    //attached to the gameobject under interface canvas
    public static InteractIndicator Instance;
    internal bool facingSubject;
    GameManager gm;
    Camera mainCam;
    TextMeshProUGUI myText;
    internal Interactable currentInteract = null;

    void Awake() => Instance = this; 
    void Start()
    {
        gm = GameManager.Instance;
        mainCam = Camera.main;
        myText = GetComponent<TextMeshProUGUI>();
        myText.enabled = false;
    }

    //display when the player is facing this ui element
    public void CheckFaceDir(Transform target, float biasValue)
    {
        if(gm.inConversation) 
        {
            DisplayIndicator(false);
            return;
        }
        //called in the interactive
        //change the child text
        var viewportPoint = mainCam.WorldToViewportPoint(target.position);
        var distanceFromCenter = Vector2.Distance(viewportPoint,Vector2.one * .5f);
        var show = distanceFromCenter < biasValue;
        DisplayIndicator(show);
    }

    public void DisplayIndicator(bool display)
    {
        myText.enabled = facingSubject = display;
        if(myText.enabled) UIManager.Instance.ShowInstruction("E / Left Mouse to interact");
        else if(!myText.enabled || gm.inConversation){ UIManager.Instance.HideInstruction();}
    }
    public void ChangeText(string text)
    {
        myText.text = text;
    }
}
