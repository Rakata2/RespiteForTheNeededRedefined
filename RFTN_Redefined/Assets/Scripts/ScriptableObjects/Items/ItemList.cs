using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemName
{
    None,
    Cutter,
    Drug,
    Key,
    Lighter,
    NailClipper,
    PocketKnife,
    Toothbrush,
    TransitCard
}

public enum ItemSafety
{ 
    Safe,
    Dangerous
}


[CreateAssetMenu(fileName = "Items", menuName = "List of items")]
public class ItemList : ScriptableObject
{
    [System.Serializable]
    public struct ItemEntry
    {
        public ItemName Name;
        public ItemSafety Safety;
        public Sprite SmallSprite;
        public Sprite BigSprite;
        public Sprite OutlinedSprite;
    }
    public List<ItemEntry> Items = new List<ItemEntry>();
}
