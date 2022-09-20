using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SketchFocusBodypart : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    internal bool focusable = false;
    Material parentMaterial;
    GameObject parentObj;
    internal bool selected = false; // there should only be one selected!

    void Start()
    {   
        parentMaterial = GetComponentInParent<SkinnedMeshRenderer>().material;
        parentObj = transform.parent.gameObject;
        GameManager.Instance.sketchManager.BodypartSelectEvent.AddListener(() => OnSelect());
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
        parentMaterial.SetColor("_EMISSION", new Color(0.0927F, 0.4852F, 0.2416F, 0.42F));
        parentMaterial.EnableKeyword("_EMISSION");
     }
     public void UnlightMaterial()
     {
        parentMaterial.DisableKeyword("_EMISSION");
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
