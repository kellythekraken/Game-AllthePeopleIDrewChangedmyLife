using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SketchFocusBodypart : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    Material parentMaterial;
    GameObject parentObj;
    internal bool selected = false; // there should only be one selected!

    void Start()
    {   
        parentMaterial = GetComponentInParent<SkinnedMeshRenderer>().material;
        parentObj = transform.parent.gameObject;
        SketchingSystem.Instance.BodypartSelectEvent.AddListener(() => OnSelect());
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
        SketchingSystem.Instance.chosenBody = parentObj;
    }
     
     public void HighlightMaterial()
     {
        parentMaterial.color = Color.black;
     }
     public void UnlightMaterial()
     {
        parentMaterial.color = Color.white;
     }
     void OnSelect()
     {
        if(SketchingSystem.Instance.chosenBody != parentObj) 
        {
            UnlightMaterial();
            selected = false;
        }
     }
}
