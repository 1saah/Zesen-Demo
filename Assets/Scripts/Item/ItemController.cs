using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    public class ItemController : MonoBehaviour
    {
        public InventoryItem_SO item_SO;

        public void SpecifyItem(InventoryItem_SO item)
        { 
            item_SO = item; 
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 3) // player
            {
                if(InventoryManager.Instance.InventoryData.InsertInNearestEmptyGrid(Instantiate(item_SO)))
                {
                    InventoryManager.Instance.UpdateInventory();
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
