using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DrawableArea
{
    public string objName;
    public List<Sprite> targetDrawings;
}

public enum ItemSection { Top, Bottom, Face, Hair, Shoes}
public enum ItemType {Mesh,Material,Gameobject}

[CreateAssetMenu(fileName = "Queer")]
public class Queer : ScriptableObject   
{
    public string npcName;
    public string pronouns;

    //3d wearable item
    public string giftLine;
    public GiftItem[] items;
    public DrawableArea[] drawableAreas;
    public Sprite backgroundDrawing;
    public Sprite signature;
    public int minimumStrokes; //number of strokes before able to stop
    public int maximumStrokes; //number of strokes before conversation end
}
