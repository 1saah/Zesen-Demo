using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenshinImpactMovement
{
    public class QuestRequestUI : MonoBehaviour
    {
        private Image requestImage;
        private TextMeshProUGUI amount;

        private void Awake()
        {
            requestImage = transform.GetChild(0).GetComponent<Image>();
            amount = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        public void UpdateInfo(QuestRequest questRequest)
        {
            requestImage.sprite = questRequest.requestSprite;
            amount.text = questRequest.requestCurrentAmount + "/" + questRequest.requestMaxAmount;
        }
    }
}
