using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class SketchingSystem : MonoBehaviour
{
    //sketch: combine area of focus + color + click the sketchbook? test this idea out.

    //make the characters into scriptable obj? include info of their drawable area, art, and help with the spawn randomization

    //first try clicking the area of focus and image appear on sketchbook accordingly.

    //the focus will be generated for each individual?

    //do you continually click the same area until it's done? or it auto complete everything of that one area?

    //what if, say it's pencil layer, each detail layer is colored in the chosen color. but you only have limited choices
    //e.g. 4 strokes before it's done. then you end up with different sketches.

    //what does this do to the gameplay? except making it less boring.
    //would it clash with reading the dialogue?

    public Button sketchbook;
    [SerializeField] private Button areaBtnPrefab;
    [SerializeField] private Image drawPrefab;

    //load the choices, color remain the same
    List<DrawableAreas> areaChoices;
    List<Button> colorChoices;
    List<Image> drawings;

    DrawableAreas chosenArea;
    Button chosenColor;
    Transform areaButtonParent, drawingParent;

    private void Start()
    {
        InitList();

        sketchbook.onClick.AddListener(Sketch);
        chosenArea = null; chosenColor = null;
    }

    void OnDisable()
    {
        ClearChild(drawingParent);
        ClearChild(areaButtonParent);
    }

    void InitList()
    {
        var colors = transform.Find("PaletteButtons").GetComponentsInChildren<Button>();
        var strokes = transform.Find("Drawings").GetComponentsInChildren<Image>();

        colorChoices = new List<Button>();
        drawings = new List<Image>();

        foreach (Button i in colors) { colorChoices.Add(i); i.onClick.AddListener(()=>RegisterColorChoice(i)); }
        foreach (Image i in strokes) { drawings.Add(i); i.enabled = false; }

        areaButtonParent = transform.Find("FocusButtons");
        drawingParent = transform.Find("Drawings");
    }

    //the chosen one should have a visual indication that they're being selected
    private void RegisterAreaChoice(DrawableAreas areaInfo) => chosenArea = areaInfo; 
    private void RegisterColorChoice(Button btn) => chosenColor = btn;

    public void PrepareToSketch(Queer queer)
    {
        areaChoices = new List<DrawableAreas>();

        areaChoices = queer.drawableAreas.ToList();

        foreach (var i in areaChoices)
        {
            if (i.targetDrawings.Count() < 1)
            {
                areaChoices.Remove(i);
                continue;
            }
            Button btn = Instantiate(areaBtnPrefab, areaButtonParent);
            btn.name = i.label;
            btn.onClick.AddListener(() => RegisterAreaChoice(i));
        }
    }

    private void Sketch()
    {
        if (chosenArea == null || chosenColor == null)
        {
            Debug.Log("Should choose both choices before clicking sketchbook!");
            return;
        }
        MakeADrawing();

        chosenArea = null; chosenColor = null;
    }

    void MakeADrawing()
    {
        Debug.LogFormat("sketch using {0} drawing {1}", chosenColor, chosenArea);

        var stroke = Instantiate(drawPrefab, drawingParent);

        //instantiate the corresponding drawing
        Sprite drawing = chosenArea.targetDrawings[0];
        stroke.sprite = drawing;

        //remove the button from the list if exhaust the repetition

        // chosenArea.gameObject.SetActive(false);

        //advance the conversation
        GameManager.Instance.ContinueSketchChat();
    }

    void ClearChild(Transform parent)
    {
        foreach (Transform child in parent) Destroy(child.gameObject);
    }
}
