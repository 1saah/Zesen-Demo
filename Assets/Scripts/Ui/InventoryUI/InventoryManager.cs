using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GenshinImpactMovement
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        public PlayerController controller;

        public PlayerInventory_SO InventoryData;

        // 控制背包UI显示
        public GameObject InventoryPanel;

        // 控制每个格子
        public List<GridUIController> gridLists;

        public Button sortButton;

        public bool isClosed;
        public Button closeButton;

        public GameObject tipPrefab;

        protected override void Awake()
        {
            base.Awake();
            UpdateInventory();
            sortButton = transform.Find("InventoryPanel/SortButton").GetComponent<Button>();
            sortButton.onClick.AddListener(SortingInventor);
            closeButton.onClick.AddListener(CloseInventoryUI);
        }

        private void Update()
        {
            UpdateInventoryStat();
        }


        // 根据按键打开或关闭背包状态
        public void ChangeStateAction(InputAction.CallbackContext context)
        {
            isClosed = !isClosed;
        }

        private void CloseInventoryUI()
        {
            isClosed = true;
            UpdateInventoryStat();
        }

        private void UpdateInventoryStat()
        {
            if(isClosed)
            {
                InventoryPanel.SetActive(false);
                PauseManager.Instance.ResumeGame();
            }    
            else
            {
                InventoryPanel.SetActive(true);
                PauseManager.Instance.PauseGame();
            }
        }

        // 整理Inventory_SO中物品排序，并刷新背包UI
        private void SortingInventor()
        {
            for(int i = 0; i < InventoryData.inventoryItemList.Count; ++i)
            {
                if (InventoryData.inventoryItemList[i] == null)
                {
                    for(int j = i + 1; j < InventoryData.inventoryItemList.Count; ++j)
                    {
                        if (InventoryData.inventoryItemList[j] != null)
                        {
                            InventoryData.inventoryItemList[i] = InventoryData.inventoryItemList[j];
                            InventoryData.inventoryItemList[j] = null;
                            break;
                        }
                    }
                }
                else // 相同物品叠加逻辑
                {
                    for (int j = i + 1; j < InventoryData.inventoryItemList.Count; ++j)
                    {
                        if (InventoryData.inventoryItemList[j] != null&& InventoryData.inventoryItemList[j].itemName.Equals(InventoryData.inventoryItemList[i].itemName))
                        {
                            if(InventoryData.inventoryItemList[j].isStackable)
                            {
                                int totalAmount = InventoryData.inventoryItemList[j].itemNum + InventoryData.inventoryItemList[i].itemNum;
                                // 数量足够叠加
                                if (totalAmount <= InventoryData.inventoryItemList[j].maxAmount)
                                {
                                    InventoryData.inventoryItemList[i].itemNum = totalAmount;
                                    InventoryData.inventoryItemList[j] = null;
                                }
                                else // 数量不够叠加
                                {
                                    InventoryData.inventoryItemList[i].itemNum = InventoryData.inventoryItemList[j].maxAmount;
                                    InventoryData.inventoryItemList[j].itemNum = totalAmount - InventoryData.inventoryItemList[j].maxAmount;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            UpdateInventory();
        }

        // 根据Inventory_SO的数据信息 更新UI
        public void UpdateInventory()
        {
            for (int i = 0; i < InventoryData.inventoryItemList.Count; ++i)
            {
                if (InventoryData.inventoryItemList[i] != null)
                {
                    gridLists[i].UpdateItemInfo(i, InventoryData.inventoryItemList[i].itemSprite, InventoryData.inventoryItemList[i].itemNum);
                }
                else
                {
                    gridLists[i].UpdateItemInfo(i);
                }
            }
        }
    }
}
