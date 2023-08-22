using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenshinImpactMovement
{
    public class QuestButton : MonoBehaviour
    {
        private Button questButton;
        public TextMeshProUGUI questName;
        public Quest_SO questData_SO;

        private void Awake()
        {
            // 手动拖绑定可以避免一些顺序问题
            questButton = GetComponent<Button>();
        }

        private void Start()
        {
            questButton.onClick.AddListener(RefreshQuestContent);
        }

        // 根据任务按键保存的任务SO来刷新任务详情面板
        private void RefreshQuestContent()
        {
            if(questData_SO != null)
            {
                QuestManager.Instance.UpdateQuestContent(questData_SO);
            }
        }
    }
}
