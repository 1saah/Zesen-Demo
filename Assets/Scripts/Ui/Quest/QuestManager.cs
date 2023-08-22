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

        // �����б�
        public List<Quest_SO> questList;
        public Transform questListTrans;
        public GameObject questButtonPrefab;

        // ��������
        public TextMeshProUGUI questName;
        public TextMeshProUGUI questDescription;

        public Transform questRewardTrans;
        public GameObject questRewardsPrefab;

        public Transform questRequestTrans;
        public GameObject questRequestPrefab;

        // ��ť
        public Button closeButton;
        public GameObject questPanel;

        // ״̬
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
            // ɾ�����е�����Button
            DeleteAllQuestsButton();
            // �����б��������Button
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

            // ˢ�������б�UI
            UpdateQuestListUI();
        }

        public void DeleteQuestToList(Quest_SO quest)
        {
            if (questList.Contains(quest))
            {
                questList.Remove(quest);
            }
            // ˢ�������б�UI
            UpdateQuestListUI();
        }

        // ����ˢ�������������(��ť)
        public void UpdateQuestContent(Quest_SO quest)
        {
            DeleteAllQuestRequests();
            DeleteAllQuestRewards();
            questName.text = quest.questName;
            questDescription.text = quest.questDes;
            // ��������Ҫ��ͽ����б�
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
                    // ˢ������
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
                    // ˢ������
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
