using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractIndicator : MonoBehaviour
{
    //to be displayed on world canvas
    public static InteractIndicator Instance;
    internal bool facingSubject;
    GameManager gm;
    Camera mainCam;
    Image myImage;
    TextMeshProUGUI myText;
    void Awake() => Instance = this; 
    void Start()
    {
        gm = GameManager.Instance;
        mainCam = Camera.main;
        myImage = GetComponent<Image>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myImage.enabled = false;
        myText.enabled = false;
    }

    //display when the player is facing this ui element
    public void CheckFaceDir(Transform target)
    {
        if(gm.inConversation) 
        {
            DisplayIndicator(false);
            return;
        }
        //should disable in conversation
        //change the target transform?
        //called in the interactive
        //change the child text
        var viewportPoint = mainCam.WorldToViewportPoint(target.position);
        viewportPoint.z = 0f;
        var distanceFromCenter = Vector2.Distance(viewportPoint,Vector2.one * .5f);
        var show = distanceFromCenter < 0.3f;
        DisplayIndicator(show);
    }

    public void DisplayIndicator(bool display)
    {
        myImage.enabled = myText.enabled = facingSubject = display;

    }
    public void ChangeText(string text)
    {
        myText.text = text;
    }
}
