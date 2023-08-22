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
            // �ֶ��ϰ󶨿��Ա���һЩ˳������
            questButton = GetComponent<Button>();
        }

        private void Start()
        {
            questButton.onClick.AddListener(RefreshQuestContent);
        }

        // �������񰴼����������SO��ˢ�������������
        private void RefreshQuestContent()
        {
            if(questData_SO != null)
            {
                QuestManager.Instance.UpdateQuestContent(questData_SO);
            }
        }
    }
}
