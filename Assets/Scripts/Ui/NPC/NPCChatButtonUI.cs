using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenshinImpactMovement
{
    public class NPCChatButtonUI : MonoBehaviour
    {
        private TextMeshProUGUI chaterName;
        private Button chaterButton;
        public ChatData_SO chatData_SO;

        private void Awake()
        {
            chaterName = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            chaterButton = GetComponent<Button>();
        }

        private void Start()
        {
            chaterButton.onClick.AddListener(OpenChatBox);
        }

        private void OpenChatBox()
        {
           ChatBoxManager.Instance.chatData = chatData_SO;
           ChatBoxManager.Instance.CheckChatBoxUI();
           Destroy(transform.gameObject);
        }

        public void SetChaterName(string name, ChatData_SO chatData)
        {
            chaterName.text = name;
            chatData_SO = chatData;
        }
    }
}
