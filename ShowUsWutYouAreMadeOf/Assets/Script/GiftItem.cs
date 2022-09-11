using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gift Item")]
public class GiftItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemSection section;
    public ItemType type;
    public Mesh mesh;
    public Material material;
}
