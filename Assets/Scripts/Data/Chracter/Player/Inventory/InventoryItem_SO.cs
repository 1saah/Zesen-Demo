using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [CreateAssetMenu (fileName = "InventoryItem_SO", menuName = "Data/Inventory/Item_Data")]
    public class InventoryItem_SO : ScriptableObject
    {
        [field: SerializeField] public string itemName { get; private set; } = "Enter Item Name";
        [field: SerializeField] [field: Range(0,999)] public int itemNum { get; set; } = 0;
        [field: SerializeField] public Sprite itemSprite { get; private set; }
        [field: SerializeField] [TextAreaAttribute (0, 10)] public string itemDesciption = "Enter Item Description";
        [field: SerializeField] public GameObject itemPrefabOnWorld { get; private set; } = null;
        [field: SerializeField] public bool isStackable { get; private set; }
        [field: SerializeField] public int maxAmount { get; private set; }
    }
}
