using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

public class SketchingSystem : MonoBehaviour
{
    public static SketchingSystem Instance;
    internal UnityEvent BodypartSelectEvent = new UnityEvent();
    public GameObject ChosenBody
    {
        get { return chosenBody; } 
        set { RegisterBodyChoice(value); BodypartSelectEvent.Invoke(); }
    }
    private GameObject chosenBody;
    Button chosenColor;
    DrawableArea chosenArea;
    public Button sketchbook;
    //[SerializeField] private Button areaBtnPrefab;
    [SerializeField] private Image drawPrefab;
    [SerializeField] List<PointerFollower> crayonPointers;
    [SerializeField] List<Color> crayonColors;  //stroe the color, access through the index of crayonpointer?
    List<DrawableArea> areaChoices; //copy of the SO data with sketches
    List<Button> colorChoices;  //crayon buttons
    List<Image> drawings;
    List<SketchFocusBodypart> bodypartLists; //reference of the list of sketchable area component
    Transform drawingParent;   //where drawing strokes will be instantiated
    List<Sprite> storedSketches;
    Queer instantiatedCopy; //instantiated copy of queer SO so delete doesn't affect the actual SO
    bool initialized = false;
    private void OnEnable()
    {
        if(!initialized)
        {
            InitList();
            initialized = true;
        }
        ClearChild(drawingParent);
    }
    void OnDisable()
    {
        ChosenBody = null; chosenColor = null; lastCrayon = null;
        //save the sketch to a storage, then delete the sketches in scene
        Destroy(instantiatedCopy);
    }

    void Awake() 
    {
        Instance = this;
    }
    void Start()
    {
        DisableAllFollower();
    }
    
    void InitList()
    {
        var colors = transform.Find("CrayonBtns").GetComponentsInChildren<Button>();
        var strokes = transform.Find("Drawings").GetComponentsInChildren<Image>();

        colorChoices = new List<Button>();
        drawings = new List<Image>();

        foreach (Button i in colors) { colorChoices.Add(i); i.onClick.AddListener(()=>RegisterColorChoice(i)); }
        foreach (Image i in strokes) { drawings.Add(i); i.enabled = false; }

        drawingParent = transform.Find("Drawings");

        sketchbook.onClick.AddListener(Sketch);
        chosenBody = null; chosenColor = null;
    }
    public void PrepareToSketch(QueerNPC queer)
    {
        instantiatedCopy = Instantiate(queer.queerID);
        
        bodypartLists = new List<SketchFocusBodypart>();

        foreach(var i in queer.sketchableAreas) { bodypartLists.Add(i); i.focusable = true; }
        areaChoices = new List<DrawableArea>(instantiatedCopy.drawableAreas.ToList());
        for (int i=0; i < areaChoices.Count(); i++)
        {
            DrawableArea choice = areaChoices[i];
            if (choice.targetDrawings.Count() < 1)
            {
                areaChoices.Remove(choice);
                continue;
            }
        }
    }
    GameObject lastCrayon = null;
    private void RegisterColorChoice(Button btn) //called by clicking different color
    {
        chosenColor = btn;
        GameObject obj = btn.gameObject;
        if(lastCrayon!= null) 
        {
            lastCrayon.SetActive(true);
            StartCrayonFollow(false);
        }
        obj.SetActive(false);
        lastCrayon = obj;
        StartCrayonFollow(true);
    }
    private void RegisterBodyChoice(GameObject target)  //called by changing clicked body
    {
        chosenBody = target;
        if(target!= null) chosenArea = areaChoices.Find(x=> x.objName == chosenBody.name);
    }
    
    //called when sketchbook is clicked
    private void Sketch()
    {
        if (chosenBody == null) {
            UIManager.Instance.DisplayInstruction("Which area of their body could I focus on?",3f);
            return;
        }
        else if(chosenColor == null){
            UIManager.Instance.DisplayInstruction("Which color shall I use this time?", 3f);
            return;
        }
        MakeADrawing();
        StartCrayonFollow(false);
        ChosenBody = null; chosenColor = null;
        //advance the conversation
        GameManager.Instance.ContinueSketchChat();
    }

    void MakeADrawing()
    {
        Debug.Log("drawn with " + chosenArea.objName + " | remaining drawings: " + chosenArea.targetDrawings.Count);

        //instantiate the corresponding drawing
        Image stroke = Instantiate(drawPrefab, drawingParent);
        //go to the next drawing!!! remove the current one
        Sprite drawing = chosenArea.targetDrawings[0]; 
        stroke.sprite = drawing;

        //set the color of the drawing
        int currCrayonIndex = crayonPointers.IndexOf(currentCrayonFollower);
        stroke.color = crayonColors[currCrayonIndex];

        //remove the drawing from lists
        chosenArea.targetDrawings.Remove(drawing);
        if (chosenArea.targetDrawings.Count < 1)
        {
            areaChoices.Remove(chosenArea);
            
            //disable the clickable area from being clickable after the drawing has been exhausted
            var completedBody = bodypartLists.FindAll(x=>x.name == chosenArea.objName);
            foreach(var i in completedBody) { i.focusable = false; bodypartLists.Remove(i);}
        }
    }
    PointerFollower currentCrayonFollower;
    private void StartCrayonFollow(bool start)
    {
       var follower = crayonPointers.Find(x => x.name == lastCrayon.name);
       follower.gameObject.SetActive(start);
       if(start)currentCrayonFollower = follower;
    }
    private void DisableAllFollower()
    {
        foreach(var i in crayonPointers) i.gameObject.SetActive(false);
    }

    void ClearChild(Transform parent)
    {
        foreach (Transform child in parent) Destroy(child.gameObject);
    }
}
