using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DrawableAreas
{
    public string label;
}

[System.Serializable]
public class GiftItem
{
    public string name;
    public Sprite icon;
    public ItemTag tag;
}

public enum ItemTag { Accessories, Outfit, Face, Hair}

[CreateAssetMenu(fileName = "Queer")]
public class Queer : ScriptableObject   
{
    public string npcName;
    
    //3d wearable item

    public GiftItem[] items;
    public DrawableAreas[] drawableAreas;

}
