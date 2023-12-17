using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName ="New Item/item")]
public class Item : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public GameObject itemPrefab;

    public enum ItemType
    {
        CatPunch,
        Lullaby,
        SportsDrink,
        Kitten,
        Trap,
        CoolDownDrink
    }
}
