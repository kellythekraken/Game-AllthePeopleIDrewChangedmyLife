using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SketchFocusBodypart : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    Material parentMaterial;
    public bool selected = false; // there should only be one selected!

    void Start()
    {   
        parentMaterial = GetComponentInParent<SkinnedMeshRenderer>().material;
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
        Debug.Log(name + "is selected!");
        selected = true;
    }
     
     public void HighlightMaterial()
     {
        parentMaterial.color = Color.black;

     }
     public void UnlightMaterial()
     {
        parentMaterial.color = Color.white;

     }
     public void Selected()
     {

     }
}
