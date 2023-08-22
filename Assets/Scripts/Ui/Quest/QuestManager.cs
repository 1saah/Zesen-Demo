using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GenshinImpactMovement
{
    public class QuestManager : Singleton<QuestManager>
    {
        public PlayerController controller;

        // 任务列表
        public List<Quest_SO> questList;
        public Transform questListTrans;
        public GameObject questButtonPrefab;

        // 任务详情
        public TextMeshProUGUI questName;
        public TextMeshProUGUI questDescription;

        public Transform questRewardTrans;
        public GameObject questRewardsPrefab;

        public Transform questRequestTrans;
        public GameObject questRequestPrefab;

        // 按钮
        public Button closeButton;
        public GameObject questPanel;

        // 状态
        private bool isOpen = false;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            closeButton.onClick.AddListener(CloseWindow);
            UpdateQuestListUI();

            if (!isOpen)
            {
                questPanel.SetActive(false);
                return;
            }
        }

        public void UpdateQuestPanel(InputAction.CallbackContext context)
        {
            if (questPanel.activeSelf)
            {
                questPanel.SetActive(false);
                isOpen = false;
            }
            else
            {
                questPanel.SetActive(true);
                isOpen = true;
            }
        }


        private void CloseWindow()
        {
            questPanel.SetActive(false);
            isOpen = false;
        }

        private void UpdateQuestListUI()
        {
            // 删除已有的任务Button
            DeleteAllQuestsButton();
            // 根据列表添加任务Button
            AddAllQuestsButton();
        }

        private void AddAllQuestsButton()
        {
            if (questList.Count > 0)
            {
                foreach (var currentQuest in questList)
                {
                    QuestButton button = Instantiate(questButtonPrefab, questListTrans).GetComponent<QuestButton>();
                    button.questName.text = currentQuest.questName;
                    button.questData_SO = currentQuest;
                }
            }
        }

        private void DeleteAllQuestsButton()
        {
            int questAmount = questListTrans.childCount;
            if (questAmount > 0)
            {
                for (int i = questAmount - 1; i >= 0; i--)
                {
                    Destroy(questListTrans.GetChild(i).gameObject);
                }
            }
        }

        public void AddQuestToList(Quest_SO quest)
        {
            if (!questList.Contains(quest))
            {
                questList.Add(quest);
            }

            // 刷新任务列表UI
            UpdateQuestListUI();
        }

        public void DeleteQuestToList(Quest_SO quest)
        {
            if (questList.Contains(quest))
            {
                questList.Remove(quest);
            }
            // 刷新任务列表UI
            UpdateQuestListUI();
        }

        // 用于刷新任务详情界面(按钮)
        public void UpdateQuestContent(Quest_SO quest)
        {
            DeleteAllQuestRequests();
            DeleteAllQuestRewards();
            questName.text = quest.questName;
            questDescription.text = quest.questDes;
            // 更新任务要求和奖励列表
            AddNewQuestRequests(quest);
            AddNewQuestRewards(quest);
        }

        private void AddNewQuestRewards(Quest_SO quest)
        {
            if (quest.rewardsList.Count > 0)
            {
                foreach (var reward in quest.rewardsList)
                {
                    QuestRewardUI questRewardUI = Instantiate(questRewardsPrefab, questRewardTrans).GetComponent<QuestRewardUI>();
                    // 刷新内容
                    questRewardUI.UpdateInfo(reward);
                }
            }
        }

        private void DeleteAllQuestRewards()
        {
            int childCount = questRewardTrans.childCount;
            if (childCount > 0)
            {
                for (int i = childCount - 1; i >= 0; --i)
                {
                    Destroy(questRewardTrans.GetChild(i).gameObject);
                }
            }
        }

        private void AddNewQuestRequests(Quest_SO quest)
        {
            if (quest.requestsList.Count > 0)
            {
                foreach (var request in quest.requestsList)
                {
                    QuestRequestUI questRequestUI = Instantiate(questRequestPrefab, questRequestTrans).GetComponent<QuestRequestUI>();
                    // 刷新内容
                    questRequestUI.UpdateInfo(request);
                }
            }
        }

        private void DeleteAllQuestRequests()
        {
            int childCount = questRequestTrans.childCount;
            if (childCount > 0)
            {
                for (int i = childCount - 1; i >= 0; --i)
                {
                    Destroy(questRequestTrans.GetChild(i).gameObject);
                }
            }
        }
    }
}
