using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;
using System;
public class SketchingSystem : MonoBehaviour
{
    public static SketchingSystem Instance;
    internal UnityEvent BodypartSelectEvent = new UnityEvent();
    internal string highlightedBody;
    private string chosenBody;
    internal string ChosenBody
    {
        get { return chosenBody; } 
        set { RegisterBodyChoice(value); BodypartSelectEvent.Invoke(); }
    }
    Button chosenColor;
    DrawableArea chosenArea;
    public Button sketchbook;
    //[SerializeField] private Button areaBtnPrefab;
    [SerializeField] private Button doneBtn,stopBtn;
    [SerializeField] private Image drawPrefab;
    [SerializeField] private GameObject drawCategoryPrefab;
    [SerializeField] private GameObject crayonButtons;  
    [SerializeField] private List<PointerFollower> crayonPointers;
    [SerializeField] private List<Color> crayonColors;  //stroe the color, access through the index of crayonpointer?
    public Color materialHighlightColor;
    List<DrawableArea> availableChoices; //copy of the SO data with sketches, to be removed
    List<Button> colorChoices;  //crayon buttons
    List<Image> drawings;
    List<SketchFocusBodypart> bodypartLists; //reference of the list of sketchable area component
    Transform drawingParent;   //where drawing strokes will be instantiated
    //List<Sprite> storedSketches;
    GameManager gm;
    Queer copiedQueerID; //instantiated copy of queer SO so delete doesn't affect the actual SO
    int strokeCount;    //keep track in purpose to show stop btn when you can stop sketching
    bool initialized = false;
    int sketchCount;

    void OnDisable()
    {
        ChosenBody = null; chosenColor = null; lastCrayon = null;
        Destroy(copiedQueerID);
        //save the sketch to a storage, then delete the sketches in scene
    }
    public void InitSketchbook()
    {
        if(initialized) return;
        Instance = this;
        gm = GameManager.Instance;
        DisableAllFollower();
        gm.dialogueRunner.AddCommandHandler("lastdraw", LastStroke);
        gm.dialogueRunner.AddCommandHandler("sketchfin", SketchCompleted);
        doneBtn.onClick.AddListener(PlayAfterSketchDialogue);
        doneBtn.gameObject.SetActive(false);
        stopBtn.onClick.AddListener(StopSketching);
        InitList();
        initialized = true;
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
    //start a new sketch
    public void PrepareToSketch(QueerNPC targetNPC)
    {
        ClearChild(drawingParent);

        targetNPC.alreadySketched = true;
        copiedQueerID = Instantiate(targetNPC.queerID);
        
        bodypartLists = new List<SketchFocusBodypart>();
        gm.variableStorage.SetValue("$MaxStrokes",copiedQueerID.maximumStrokes);
        
        //make empty parent for each sketchable area, to create layer hierarchy
        foreach(var i in targetNPC.bodyPartMeshes)
        {
            GameObject categoryParent = Instantiate(drawCategoryPrefab,drawingParent);
            categoryParent.name = i.name;
        }

        foreach(var i in targetNPC.sketchableAreas) { bodypartLists.Add(i); i.enabled = true; i.Init();}
        availableChoices = new List<DrawableArea>(copiedQueerID.drawableAreas.ToList());
        
        //remove empty choices
        for (int i=0; i < availableChoices.Count(); i++)
        {
            DrawableArea choice = availableChoices[i];
            if (choice.targetDrawings.Count() < 1)
            {
                availableChoices.Remove(choice);
                continue;
            }
        }
        foreach(Button i in colorChoices) {i.enabled = true; }
        gm.EnableWardrobeAction(false);
        sketchbook.enabled = true;
        crayonButtons.SetActive(true);
        stopBtn.gameObject.SetActive(false);
        strokeCount = 0;
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
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.pen_pickup);
        StartCrayonFollow(true);
    }
    
    int bodyIndex;
    private void RegisterBodyChoice(string target)  //called by changing clicked body
    {
        chosenBody = target;
        if(target!= null) 
        {
            chosenArea = availableChoices.Find(x=> x.objName == chosenBody);
        }
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
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.draw);
        MakeADrawing();
        StartCrayonFollow(false);
        ChosenBody = null; chosenColor = null;
        lastCrayon.SetActive(true);

        //change sketchindex to trigger and advance the dialogue
        bodyIndex = Array.FindIndex(copiedQueerID.drawableAreas, x=> x.objName == chosenArea.objName);
        gm.variableStorage.SetValue("$SketchIndex",bodyIndex);
        gm.ContinueSketchChat();

        if(copiedQueerID.minimumStrokes != 0)
        {
            if(strokeCount >= copiedQueerID.minimumStrokes) stopBtn.gameObject.SetActive(true);
        }
    }

    void MakeADrawing()
    {
        //Debug.Log("drawn with " + chosenArea.objName + " | remaining drawings: " + chosenArea.targetDrawings.Count);

        //use the sketchablearea as the reference to layer? 0= top, 5 = bottom e.g. hair > clothes > body
        Transform drawingCategoryParent = drawingParent.Find(chosenArea.objName);

        //instantiate the corresponding drawing
        Image stroke = Instantiate(drawPrefab, drawingCategoryParent);

        //go to the next drawing!!! remove the current one
        Sprite drawing = chosenArea.targetDrawings[0]; 
        stroke.sprite = drawing;

        //set the color of the drawing
        int currCrayonIndex = crayonPointers.IndexOf(currentCrayonFollower);
        stroke.color = crayonColors[currCrayonIndex];
        strokeCount++;

        //remove the drawing from lists
        chosenArea.targetDrawings.Remove(drawing);
        if (chosenArea.targetDrawings.Count < 1)
        {
            availableChoices.Remove(chosenArea);
            
            //disable the clickable area from being clickable after the drawing has been exhausted
            var completedBody = bodypartLists.FindAll(x=>x.bodyName == chosenArea.objName);
            foreach(var i in completedBody) { i.enabled = false; bodypartLists.Remove(i);}
        }
    }

    void StopSketching()
    {
        gm.dialogueRunner.StartDialogue(copiedQueerID.npcName + "LastStroke");
    }
    //command: lastdraw
    void LastStroke()
    {
        crayonButtons.SetActive(false);
        Image stroke = Instantiate(drawPrefab, drawingParent); 
        stroke.sprite = copiedQueerID.backgroundDrawing;
        sketchbook.enabled = false;
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.draw);
        stopBtn.gameObject.SetActive(false);
    }
    //command: sketchfin
    void SketchCompleted()
    {
        //what about giving the player option to keep drawing until all options exhaust? But there will be no more dialogues.
        doneBtn.gameObject.SetActive(true);
        gm.LockCursor(false);
        foreach(var i in bodypartLists) { i.enabled = false;}
        foreach(Button i in colorChoices) {i.enabled = false; }        
        availableChoices.Clear();

        sketchCount++;
        if(sketchCount>=1) gm.UnlockEndGame();
    }

    //called by pressing done button
    void PlayAfterSketchDialogue()
    {
        //give logic to back button at start: trigger the fin dialogue.
        doneBtn.gameObject.SetActive(false);
        string dialogueTitle = copiedQueerID.npcName + "SketchFin";
        gm.dialogueRunner.StartDialogue(dialogueTitle);
        gm.currMode = CurrentMode.Conversation;
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
