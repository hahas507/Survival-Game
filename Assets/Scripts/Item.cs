using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "NewItem/item")]
public class Item : ScriptableObject
{
    public ItemType itemType;

    [TextArea]
    public string itemDescription;

    public string itemName;
    public Sprite itemImage;
    public GameObject itemPrefab;

    public string weaponType;

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }
}