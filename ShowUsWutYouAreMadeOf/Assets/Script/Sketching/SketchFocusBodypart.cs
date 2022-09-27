using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SketchFocusBodypart : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    internal bool focusable = false;
    Material parentMaterial;
    GameObject parentObj;
    Color highlightColor;
    internal bool selected = false; // there should only be one selected!

    void Start()
    {   
        parentMaterial = GetComponentInParent<SkinnedMeshRenderer>().material;
        parentObj = transform.parent.gameObject;
        GameManager.Instance.sketchManager.BodypartSelectEvent.AddListener(() => OnSelect());
        highlightColor = SketchingSystem.Instance.materialHighlightColor;
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if(focusable)HighlightMaterial();
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if(!selected) UnlightMaterial();
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if(!focusable) return;
        selected = true;
        SketchingSystem.Instance.ChosenBody = parentObj;
    }
     
     public void HighlightMaterial()
     {
        parentMaterial.SetColor("_EmissionColor", highlightColor);
     }
     public void UnlightMaterial()
     {
        parentMaterial.SetColor("_EmissionColor", Color.black);
     }
     void OnSelect()
     {
        if(SketchingSystem.Instance.ChosenBody != parentObj) 
        {
            UnlightMaterial();
            selected = false;
        }
     }
}
