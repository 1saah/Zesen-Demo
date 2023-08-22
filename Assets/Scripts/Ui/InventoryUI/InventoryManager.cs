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

        // ���Ʊ���UI��ʾ
        public GameObject InventoryPanel;

        // ����ÿ������
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


        // ���ݰ����򿪻�رձ���״̬
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

        // ����Inventory_SO����Ʒ���򣬲�ˢ�±���UI
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
                else // ��ͬ��Ʒ�����߼�
                {
                    for (int j = i + 1; j < InventoryData.inventoryItemList.Count; ++j)
                    {
                        if (InventoryData.inventoryItemList[j] != null&& InventoryData.inventoryItemList[j].itemName.Equals(InventoryData.inventoryItemList[i].itemName))
                        {
                            if(InventoryData.inventoryItemList[j].isStackable)
                            {
                                int totalAmount = InventoryData.inventoryItemList[j].itemNum + InventoryData.inventoryItemList[i].itemNum;
                                // �����㹻����
                                if (totalAmount <= InventoryData.inventoryItemList[j].maxAmount)
                                {
                                    InventoryData.inventoryItemList[i].itemNum = totalAmount;
                                    InventoryData.inventoryItemList[j] = null;
                                }
                                else // ������������
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

        // ����Inventory_SO��������Ϣ ����UI
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
