using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractIndicator : MonoBehaviour
{
    //to be displayed on world canvas
    //take info from interactables, check facing direction
    //attached to the gameobject under interface canvas
    public static InteractIndicator Instance;
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
        InputManager.Instance.interactAction.performed += InteractionAction;
    }

    void InteractionAction(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        //if hit 
        if (rayHitObject!=null && myText.enabled) 
        {
            rayHitObject.GetComponent<Interactable>().StartInteraction();
            rayHitObject = null;
        }
    }

    void Update()
    {
        DrawRay();
    }

//make the name change when you can interact already! the ontrigger enter function

    /*
    //display when the player is facing this ui element
    public void CheckFaceDir(Transform target, float biasValue)
    {
        //also check if the object is in the canvas rect? 

        if(!gm.CanFreelyInteract()) 
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
    
    [SerializeField] Transform rayHitObject;
    public void DrawRay()
    {
        if(!gm.CanFreelyInteract()) 
        {
            return;
        }
        var pos = player.transform.position + new Vector3(0,1.6f,0);
        var forward = mainCam.transform.forward;
        var rayLength = 5f;
        RaycastHit hit;

        if (Physics.Raycast(pos, forward, out hit,rayLength,layerMask)) {
            Transform objectHit = hit.transform;
            if(Vector3.Distance(pos, objectHit.position) < 2.5f)
            {
                rayHitObject = objectHit.transform;
                ChangeText(rayHitObject.name);
                DisplayIndicator(true);
            }
            else{DisplayIndicator(false);}
        }
    }

/*    public void DrawRay1(string interactName)
    {
        if(!gm.CanFreelyInteract()) 
        {
            DisplayIndicator(false);
            return;
        }
        var pos = player.transform.position + new Vector3(0,1.6f,0);
        var forward = mainCam.transform.forward;
        var rayLength = 4f;
        RaycastHit hit;

        if (Physics.Raycast(pos, forward, out hit,rayLength,layerMask)) {
            Transform objectHit = hit.transform;
            bool show = (hit.transform.gameObject.layer == 11) && (objectHit.name == interactName);
            DisplayIndicator(show);
        }
    }*/

    public void DisplayIndicator(bool display)
    {
        myText.enabled = display;
        if(myText.enabled) UIManager.Instance.ShowInteractInstruction();
        else if(!myText.enabled || gm.InConversation()){ UIManager.Instance.HideInstruction();}
    }
    public void ChangeText(string text)
    {
        myText.text = text;
    }
}
