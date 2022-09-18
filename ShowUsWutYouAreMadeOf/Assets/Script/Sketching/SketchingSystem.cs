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
    List<DrawableArea> areaChoices;
    InputManager inputManager;
    List<Button> colorChoices;
    List<Image> drawings;
    Transform drawingParent;   //where drawing strokes will be instantiated
    List<Sprite> storedSketches;
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
        inputManager = GameManager.Instance.inputManager;
        DisableAllFollower();
    }
    Queer instantiatedCopy;
    void InitList()
    {
        var colors = transform.Find("CrayonBtns").GetComponentsInChildren<Button>();
        var strokes = transform.Find("Drawings").GetComponentsInChildren<Image>();

        colorChoices = new List<Button>();
        drawings = new List<Image>();

        foreach (Button i in colors) { colorChoices.Add(i); i.onClick.AddListener(()=>RegisterColorChoice(i)); }
        foreach (Image i in strokes) { drawings.Add(i); i.enabled = false; }

        //areaButtonParent = transform.Find("FocusButtons");
        drawingParent = transform.Find("Drawings");

        sketchbook.onClick.AddListener(Sketch);
        //chosenBody = null; 
        chosenColor = null;
    }
    public void PrepareToSketch(Queer queer)
    {
        instantiatedCopy = Instantiate(queer);
        
        areaChoices = new List<DrawableArea>(instantiatedCopy.drawableAreas.ToList());
        for (int i=0; i < areaChoices.Count(); i++)
        {
            DrawableArea choice = areaChoices[i];
            if (choice.targetDrawings.Count() < 1)
            {
                areaChoices.Remove(choice);
                continue;
            }
            /*Button btn = Instantiate(areaBtnPrefab, areaButtonParent);
            btn.name = choice.objName;
            btn.onClick.AddListener(() => RegisterAreaChoice(choice));*/
        }
        
    }

    //private void RegisterAreaChoice(DrawableArea areaInfo) => chosenBody = areaInfo; 
    GameObject lastCrayon = null;
    private void RegisterColorChoice(Button btn) 
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
    private void RegisterBodyChoice(GameObject target)
    {
        chosenBody = target;
        if(target!= null) chosenArea = areaChoices.Find(x=> x.objName == chosenBody.name);
    }
    //called when sketchbook is clicked
    private void Sketch()
    {
        if (chosenBody == null || chosenColor == null)
        {
            //display a notice on the ui!
            Debug.Log("Should choose both choices before clicking sketchbook!");
            return;
        }
        MakeADrawing();
        StartCrayonFollow(false);
        ChosenBody = null; chosenColor = null;
    }

    void MakeADrawing()
    {
        //instantiate the corresponding drawing
        Image stroke = Instantiate(drawPrefab, drawingParent);
        //go to the next drawing!!! remove the current one
        Sprite drawing = chosenArea.targetDrawings[0]; 
        stroke.sprite = drawing;

        Debug.Log("drawn with " + chosenArea.objName + " | remaining drawings: " + chosenArea.targetDrawings.Count);
        if (chosenArea.targetDrawings.Count < 1)
        {
            areaChoices.Remove(chosenArea);

            //disable the clickable area from being clickable!!! the drawing has been exhausted

            //GameObject btn = areaButtonParent.Find(chosenBody.objName).gameObject;
            //if (btn != null) Destroy(btn);
        }
        chosenBody = null;
        //advance the conversation
        GameManager.Instance.ContinueSketchChat();
    }

    private void StartCrayonFollow(bool start)
    {
       var follower = crayonPointers.Find(x => x.name == lastCrayon.name);
       follower.gameObject.SetActive(start);
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
