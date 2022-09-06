using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DrawableAreas
{
    public string label;
    public int repetition;
    public Sprite[] targetDrawings;
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
    public int dialogueLength; //number of strokes before conversation end

}
