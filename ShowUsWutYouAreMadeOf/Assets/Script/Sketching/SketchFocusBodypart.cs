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
    SketchingSystem sketchingSystem;

    void Start()
    {   
        sketchingSystem = SketchingSystem.Instance;
        materialToHighlight = bodypartMesh.material;
        GameManager.Instance.sketchManager.BodypartSelectEvent.AddListener(() => OnSelect());
        highlightColor = sketchingSystem.materialHighlightColor;
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
        Debug.Log("chosen" + bodyName);
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
