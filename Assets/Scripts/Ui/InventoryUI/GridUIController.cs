using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace GenshinImpactMovement
{
    public class GridUIController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject itemHolder;
        public UnityEngine.UI.Image itemImage;
        public TextMeshProUGUI itemNum;
        public int sortingInIventor;

        public GameObject tip;

        public void UpdateItemInfo(int sort, Sprite image = null, int num = -1)
        {
            if(image != null && num != -1) 
            {
                itemHolder.SetActive(true);
                itemImage.sprite = image;
                itemNum.text = num + "";
                sortingInIventor = sort;
            }
            else
            {
                itemHolder.SetActive(false);
                sortingInIventor = sort;
            }
        }

        public void SwapItem(int targetSort)
        {
            if(targetSort != sortingInIventor)
            {
                var temp = InventoryManager.Instance.InventoryData.inventoryItemList[sortingInIventor];
                InventoryManager.Instance.InventoryData.inventoryItemList[sortingInIventor] = InventoryManager.Instance.InventoryData.inventoryItemList[targetSort];
                InventoryManager.Instance.InventoryData.inventoryItemList[targetSort] = temp;
                InventoryManager.Instance.UpdateInventory();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(itemHolder.active)
            {

                float canvasScalerIndex = InventoryManager.Instance.GetComponent<CanvasScaler>().scaleFactor;
                Vector3 tipPos = new Vector3(eventData.position.x - 120, eventData.position.y, 0f);

                tip = Instantiate(InventoryManager.Instance.tipPrefab, tipPos, Quaternion.identity, InventoryManager.Instance.InventoryPanel.transform);
                tip.transform.SetSiblingIndex(tip.transform.parent.childCount - 1);

                // 更新面板信息
                UpdateTipsInfo(tip);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (tip != null)
            {
                Destroy(tip);
            }
        }

        private void UpdateTipsInfo(GameObject tip)
        {
            TextMeshProUGUI name = tip.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI desc = tip.transform.Find("Description").GetComponent<TextMeshProUGUI>();

            name.text = InventoryManager.Instance.InventoryData.inventoryItemList[sortingInIventor].name;
            desc.text = InventoryManager.Instance.InventoryData.inventoryItemList[sortingInIventor].itemDesciption;
        }
    }
}
