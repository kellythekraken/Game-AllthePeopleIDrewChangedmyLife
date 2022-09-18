using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DrawableArea
{
    public string objName;
    public List<Sprite> targetDrawings;
}

public enum ItemSection { Accessories, Outfit, Face, Hair, Shoes}
public enum ItemType {Mesh,Material,Gameobject}

[CreateAssetMenu(fileName = "Queer")]
public class Queer : ScriptableObject   
{
    public string npcName;
    public string pronouns;

    //3d wearable item

    public GiftItem[] items;
    public DrawableArea[] drawableAreas;
    public Sprite backgroundDrawing;
    public int dialogueLength; //number of strokes before conversation end
}
