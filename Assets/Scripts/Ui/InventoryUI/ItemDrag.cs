using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GenshinImpactMovement
{
    public class ItemDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Transform originalParent;
        private Vector3 originalPos;

        public void OnBeginDrag(PointerEventData eventData)
        {
            // ÅÅ³ýtipsÎ´É¾³ýµÄbug
            if (transform.parent.GetComponent<GridUIController>().tip != null)
            {
                Destroy(transform.parent.GetComponent<GridUIController>().tip);
            }

            originalParent = transform.parent;
            originalPos = transform.localPosition;
            transform.SetParent(transform.parent.parent.parent.parent.Find("DragCanvas"));
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;    
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                // ÅÅ³ýtipsÎ´É¾³ýµÄbug
                if (originalParent.GetComponent<GridUIController>().tip != null)
                {
                    Destroy(originalParent.GetComponent<GridUIController>().tip);
                }             

                int sort = originalParent.GetComponent<GridUIController>().sortingInIventor;
                if (eventData.pointerEnter.tag == "Drag"&& eventData.pointerEnter.GetComponent<GridUIController>())
                {
                    eventData.pointerEnter.gameObject.GetComponent<GridUIController>().SwapItem(sort);
                }
                else if(eventData.pointerEnter.tag == "Drag" && eventData.pointerEnter.transform.parent.parent.parent.gameObject.GetComponent<GridUIController>())
                {
                    eventData.pointerEnter.transform.parent.parent.parent.gameObject.GetComponent<GridUIController>().SwapItem(sort);
                }
            }

            transform.SetParent(originalParent);
            transform.localPosition = originalPos;
        }

    }
}
