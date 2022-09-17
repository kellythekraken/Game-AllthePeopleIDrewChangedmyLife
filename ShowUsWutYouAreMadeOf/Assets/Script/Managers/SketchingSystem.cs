using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class SketchingSystem : MonoBehaviour
{
    public Button sketchbook;
    [SerializeField] private Button areaBtnPrefab;
    [SerializeField] private Image drawPrefab;
    [SerializeField] PointerFollower[] crayonPointers;
    List<DrawableArea> areaChoices;
    InputManager inputManager;
    List<Button> colorChoices;
    List<Image> drawings;

    DrawableArea chosenArea;
    Button chosenColor;
    Transform areaButtonParent, drawingParent;
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
        ClearChild(areaButtonParent);
    }
    void OnDisable()
    {
        chosenArea = null; chosenColor = null;
        //save the sketch to a storage, then delete the sketches in scene
        Destroy(instantiatedCopy);
    }
    void Start()
    {
        inputManager = GameManager.Instance.inputManager;
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

        areaButtonParent = transform.Find("FocusButtons");
        drawingParent = transform.Find("Drawings");

        sketchbook.onClick.AddListener(Sketch);
        chosenArea = null; chosenColor = null;
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
            Button btn = Instantiate(areaBtnPrefab, areaButtonParent);
            btn.name = choice.label;
            btn.onClick.AddListener(() => RegisterAreaChoice(choice));
        }
    }

    GameObject lastCrayon = null;
    //the chosen one should have a visual indication that they're being selected
    private void RegisterAreaChoice(DrawableArea areaInfo) => chosenArea = areaInfo; 
    private void RegisterColorChoice(Button btn) 
    {
        chosenColor = btn;
        GameObject obj = btn.gameObject;
        if(lastCrayon!= null) lastCrayon.SetActive(true);
        obj.SetActive(false);
        lastCrayon = obj;
        //disable the last crayon follow cursor
        //enable the correct crayon to follow cursor
    }

    private void Sketch()
    {
        if (chosenArea == null || chosenColor == null)
        {
            Debug.Log("Should choose both choices before clicking sketchbook!");
            return;
        }
        //disable the crayon follow cursor
        MakeADrawing();

        chosenArea = null; chosenColor = null;
    }

    void MakeADrawing()
    {
        //Debug.LogFormat("sketch using {0} drawing {1}", chosenColor, chosenArea);

        var stroke = Instantiate(drawPrefab, drawingParent);

        //instantiate the corresponding drawing
        Sprite drawing = chosenArea.targetDrawings[0];
        stroke.sprite = drawing;

        chosenArea.targetDrawings.Remove(drawing);

        Debug.Log("drawn with " + chosenArea.label + " | remaining drawings: " + chosenArea.targetDrawings.Count);
        if (chosenArea.targetDrawings.Count < 1)
        {
            areaChoices.Remove(chosenArea);

            GameObject btn = areaButtonParent.Find(chosenArea.label).gameObject;
            if (btn != null) Destroy(btn);
        }

        //advance the conversation
        GameManager.Instance.ContinueSketchChat();
    }

    void ClearChild(Transform parent)
    {
        foreach (Transform child in parent) Destroy(child.gameObject);
    }
}
