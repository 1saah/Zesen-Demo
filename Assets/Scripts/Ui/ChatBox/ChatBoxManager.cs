using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GenshinImpactMovement
{
    public class ChatBoxManager : Singleton<ChatBoxManager>
    {
        public ChatBox chatBox;
        public ChatOptions chatOptions;
        public Button continueBotton;

        public ChatData_SO chatData;
        public int currentChatDataID;

        public GameObject chatBoxPanel;
        public GameObject NPCNamePanel;

        public GameObject nameChatButtonPrefab;

        protected override void Awake()
        {
            base.Awake();
            chatBoxPanel = transform.GetChild(2).gameObject;
            NPCNamePanel = transform.GetChild(1).gameObject;
            chatBox = chatBoxPanel.transform.GetChild(0).GetComponent<ChatBox>();
            continueBotton = chatBoxPanel.transform.GetChild(1).GetComponent<Button>();
            continueBotton.onClick.AddListener(ContinueNextChat);
            chatOptions = chatBoxPanel.transform.GetChild(2).GetComponent<ChatOptions>();
        }

        private void Start()
        {
            CheckChatBoxUI();
        }

        public void CheckChatBoxUI()
        {
            if (chatData == null)
            {
                chatBoxPanel.SetActive(false);
            }
            else
            {
                chatBoxPanel.SetActive(true);
                // 读取第一个对话内容
                currentChatDataID = chatData.chatPieces[0].chatNumber;

                UpdateChatPenel();
            }
        }

        // 根据目前对话编号刷新整个对话UI
        public void UpdateChatPenel()
        {
            // 如果对话超出范围 那么关闭对话窗口
            if (currentChatDataID >= chatData.chatPieces.Count)
            {
                chatData = null;
                chatBoxPanel.SetActive(false);
                return;
            }

            // 更新对话内容
            chatBox.UpdateChatBox(chatData.chatPieces[currentChatDataID]);

            // 更新选项内容
            chatOptions.UpdateOptions(chatData.chatPieces[currentChatDataID]);
        }

        private void ContinueNextChat()
        {
            currentChatDataID += 1;

            UpdateChatPenel();
        }

        public void CreateNameChatButton(string name, ChatData_SO chatData_SO)
        {
            // 删除已有的对话选项
            DestroyNameChatButtons();

            // 创建新的对话选项
            NPCChatButtonUI chatButton = Instantiate(nameChatButtonPrefab, NPCNamePanel.transform).GetComponent< NPCChatButtonUI>();
            // 更新名字
            chatButton.SetChaterName(name, chatData_SO);

        }

        public void DestroyNameChatButtons()
        {
            // 删除已有的对话选项
            if (NPCNamePanel.transform.childCount != 0)
            {
                for (int i = NPCNamePanel.transform.childCount - 1; i >= 0; --i)
                {
                    Destroy(NPCNamePanel.transform.GetChild(i).gameObject);
                }
            }
        }
    }
}
