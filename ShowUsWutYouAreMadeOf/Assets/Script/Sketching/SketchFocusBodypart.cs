using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class SketchFocusBodypart : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public string bodyName;
    Material materialToHighlight;
    Color highlightColor;
    internal bool selected = false; // there should only be one selected!
    SketchingSystem sketchingSystem;
    bool init = false;

    public void Init()
    {   
        if(init) return;

        gameObject.layer = 8;
        sketchingSystem = SketchingSystem.Instance;
        var npc = GetComponentInParent<QueerNPC>();
        var bodypartMesh = Array.Find(npc.bodyPartMeshes,t=> t.name == bodyName);
        materialToHighlight = bodypartMesh.material;
        GameManager.Instance.sketchManager.BodypartSelectEvent.AddListener(() => OnSelect());
        highlightColor = sketchingSystem.materialHighlightColor;
        init = true;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        HighlightMaterial();
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if(!selected) UnlightMaterial();
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        selected = true;
        sketchingSystem.ChosenBody = bodyName;
    }
    public void HighlightMaterial()
    {
        materialToHighlight.SetColor("_EmissionColor", highlightColor);
    }
    public void UnlightMaterial()
    {
        materialToHighlight.SetColor("_EmissionColor", Color.black);
    }

    //unselect other
    void OnSelect()
    {
        if(sketchingSystem.ChosenBody != bodyName) 
        {
            UnlightMaterial();
            selected = false;
        }
    }
}
