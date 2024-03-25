using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Loot")]
public class LootData : InventoryItemData
{
    [Header("Loot Details")]
    public Sprite LootSprite;
    public float DropChance;
}
