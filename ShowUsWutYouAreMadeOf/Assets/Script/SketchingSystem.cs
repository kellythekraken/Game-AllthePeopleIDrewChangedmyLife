using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Button areaBtnPrefab;

    //load the choices, color remain the same
    List<Button> areaChoices, colorChoices;
    List<Image> drawings;

    Button chosenArea, chosenColor;
    Transform areaButtonParent;

    private void Start()
    {
        InitList();

        sketchbook.onClick.AddListener(Sketch);
        chosenArea = chosenColor = null;
    }
    
    void InitList()
    {
        var colors = transform.Find("PaletteButtons").GetComponentsInChildren<Button>();
        var strokes = transform.Find("Drawings").GetComponentsInChildren<Image>();

        colorChoices = new List<Button>();
        areaChoices = new List<Button>();
        drawings = new List<Image>();

        foreach (Button i in colors) { colorChoices.Add(i); i.onClick.AddListener(()=>RegisterColorChoice(i)); }
        foreach (Image i in strokes) { drawings.Add(i); i.enabled = false; }

        Debug.Log("color size " + colorChoices.Count + " | area size: " + areaChoices.Count);

        areaButtonParent = transform.Find("FocusButtons");
    }

    //the chosen one should have a visual indication that they're being selected
    private void RegisterAreaChoice(Button btn) => chosenArea = btn; 
    private void RegisterColorChoice(Button btn) => chosenColor = btn;

    public void PrepareToSketch(Queer queer)
    {
        //take the scriptable obj and instantiate buttons 
        foreach (Transform child in areaButtonParent) Destroy(child.gameObject);

        var areaLabels = queer.drawableAreas;
        foreach (var i in areaLabels)
        {
            Button btn = Instantiate(areaBtnPrefab, areaButtonParent);
            btn.name = i.label;
            areaChoices.Add(btn);
            btn.onClick.AddListener(() => RegisterAreaChoice(btn));
        }
    }
    private void Sketch()
    {
        Debug.Log(chosenArea);
        if (chosenArea == null || chosenColor == null)
        {
            Debug.Log("Should choose both choices before clicking sketchbook!");
            return;
        }
        MakeADrawing();

        chosenArea.gameObject.SetActive(false);
        areaChoices.Remove(chosenArea);

        chosenArea = chosenColor = null;
    }

    void MakeADrawing()
    {
        Debug.LogFormat("sketch using {0} drawing {1}", chosenColor, chosenArea);
        Image targetDrawing = drawings.Find(x => x.name == chosenArea.name);
        targetDrawing.enabled = true;

        //advance the conversation
        GameManager.Instance.ContinueSketchChat();
    }
}
