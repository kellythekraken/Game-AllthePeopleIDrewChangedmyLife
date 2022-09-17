using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SketchFocusBodypart : MonoBehaviour
{
    //attached to each part of mesh that could be sketched
    //should be loaded into each npc data, and unlock when you can sketch each body part, and if there are any more stroke left.

    void OnMouseOver()
    {
        Debug.Log("mouse enter");
    }
     void OnMouseExit()
     {
        Debug.Log("mouse exit");
     }
     
}
