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

    //load the choices, color remain the same
    List<Button> areaChoices, colorChoices;

    //saved variable
    Button chosenArea, chosenColor;

    private void Start()
    {
        InitList();

        sketchbook.onClick.AddListener(Sketch);
        chosenArea = chosenColor = null;
    }

    void InitList()
    {
        var colors = transform.Find("PaletteButton").GetComponentsInChildren<Button>();
        var areas = transform.Find("FocusButton").GetComponentsInChildren<Button>();
        colorChoices = new List<Button>();
        areaChoices = new List<Button>();
        foreach (Button i in colors) { colorChoices.Add(i); i.onClick.AddListener(()=>RegisterColorChoice(i)); }
        foreach (Button i in areas) { areaChoices.Add(i); i.onClick.AddListener(()=>RegisterAreaChoice(i)); }
        Debug.Log("color size " + colorChoices.Count + " | area size: " + areaChoices.Count);
    }

    private void RegisterAreaChoice(Button btn) => chosenArea = btn;
    private void RegisterColorChoice(Button btn) => chosenColor = btn;


    private void Sketch()
    {
        if (chosenArea == null || chosenColor == null)
        {
            Debug.Log("Should choose both choices before clicking sketchbook!");
            return;
        }
        Debug.LogFormat("sketch using {0} drawing {1}", chosenColor, chosenArea);
        chosenArea = chosenColor = null;
        //chosenArea.gameObject.SetActive(false);
    }


}
