using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [CreateAssetMenu(fileName = "Inventory_SO", menuName = "Data/Inventory/Inventory_Data")]
    public class PlayerInventory_SO: ScriptableObject
    {
        [field: SerializeField] public List<InventoryItem_SO> inventoryItemList;

        // ���ڲ��������һ���ո���
        public bool InsertInNearestEmptyGrid(InventoryItem_SO newItem)
        {
            for (int i = 0; i < inventoryItemList.Count; ++i)
            {
                if (inventoryItemList[i] == null)
                {
                    inventoryItemList[i] = newItem;
                    return true;
                }
            }
            return false;
        }
    }
}
