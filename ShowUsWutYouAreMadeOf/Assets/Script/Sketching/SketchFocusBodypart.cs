using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SketchFocusBodypart : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] SkinnedMeshRenderer bodypartMesh;
    [SerializeField] string bodyName;
    Material materialToHighlight;
    Color highlightColor;
    internal bool selected = false; // there should only be one selected!

    void Start()
    {   
        materialToHighlight = bodypartMesh.material;
        GameManager.Instance.sketchManager.BodypartSelectEvent.AddListener(() => OnSelect());
        highlightColor = SketchingSystem.Instance.materialHighlightColor;
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
        SketchingSystem.Instance.ChosenBody = bodyName;
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
        if(SketchingSystem.Instance.ChosenBody != bodyName) 
        {
            UnlightMaterial();
            selected = false;
        }
    }
}
