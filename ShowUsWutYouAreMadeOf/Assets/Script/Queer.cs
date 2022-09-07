using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DrawableArea
{
    public string label;
    public List<Sprite> targetDrawings;
}

[System.Serializable]
public class GiftItem
{
    public string name;
    public Sprite icon;
    public ItemTag tag;
    internal bool clicked;
}

public enum ItemTag { Accessories, Outfit, Face, Hair, Shoes}

[CreateAssetMenu(fileName = "Queer")]
public class Queer : ScriptableObject   
{
    public string npcName;
    public string pronouns;

    //3d wearable item

    public GiftItem[] items;
    public DrawableArea[] drawableAreas;
    public int dialogueLength; //number of strokes before conversation end
}
