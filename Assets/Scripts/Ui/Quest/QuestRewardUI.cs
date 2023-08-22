using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenshinImpactMovement
{
    public class QuestRewardUI : MonoBehaviour
    {
        private Image rewardImage;
        private TextMeshProUGUI amount;

        private void Awake()
        {
            rewardImage = transform.GetChild(0).GetComponent<Image>();
            amount = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        public void UpdateInfo(QuestReward questReward)
        {
            rewardImage.sprite = questReward.rewardSprite;
            amount.text = questReward.rewardAmount.ToString();
        }
    }
}
