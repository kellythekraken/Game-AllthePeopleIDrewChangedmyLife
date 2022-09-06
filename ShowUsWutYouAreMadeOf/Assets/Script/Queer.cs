using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class DrawableAreas
{
    public string label;
}

[CreateAssetMenu(fileName = "Queer")]
public class Queer : ScriptableObject   
{
    public string npcName;
    
    public string itemName;
    public Sprite itemIcon;
    //3d wearable item

    public DrawableAreas[] drawableAreas;

}
