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
    GameObject player;

    [SerializeField] LayerMask layerMask;

    void Awake() => Instance = this; 
    void Start()
    {
        gm = GameManager.Instance;
        player = gm.player;
        mainCam = gm.mainCam;
        myText = GetComponent<TextMeshProUGUI>();
        myText.enabled = false;
    }

    /*
    //display when the player is facing this ui element
    public void CheckFaceDir(Transform target, float biasValue)
    {
        //also check if the object is in the canvas rect? 

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
    */

    public void DrawRay(string interactName)
    {
        var pos = player.transform.position + new Vector3(0,1.6f,0);
        var forward = mainCam.transform.forward;
        var rayLength = 4f;
        RaycastHit hit;
        
        Debug.DrawRay(pos,forward,Color.yellow);

        if (Physics.Raycast(pos, forward, out hit,rayLength,layerMask)) {
            Transform objectHit = hit.transform;
            bool show = (hit.transform.gameObject.layer == 11) && (objectHit.name == interactName);
            myText.enabled = facingSubject = show;
        }
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
