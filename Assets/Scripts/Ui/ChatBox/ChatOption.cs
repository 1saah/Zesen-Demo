using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GenshinImpactMovement
{
    public class ChatOption : MonoBehaviour
    {
        public Button optionButton;
        public TextMeshProUGUI optionContent;
        public bool isTakingTask;
        public int targetID;
        public Quest_SO quest;

        private void Awake()
        {
            optionButton = GetComponent<Button>();
            optionContent = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            optionButton.onClick.AddListener(optionButtonListener);
        }

        private void optionButtonListener()
        {
            ChatBoxManager.Instance.currentChatDataID = targetID;
            ChatBoxManager.Instance.UpdateChatPenel();
            if (isTakingTask )
            {
                // TODO: 刷新加入任务
                QuestManager.Instance.AddQuestToList(quest);
            }
        }

        public void UpdateOption(OptionPiece optionPiece)
        {
            optionContent.text = optionPiece.optionContent;
            isTakingTask = optionPiece.isTakingTask;
            targetID = optionPiece.optionTarget;
            quest = optionPiece.Quest_SO;
        }
    }
}
