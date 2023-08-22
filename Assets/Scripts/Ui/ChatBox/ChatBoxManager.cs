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
                // ��ȡ��һ���Ի�����
                currentChatDataID = chatData.chatPieces[0].chatNumber;

                UpdateChatPenel();
            }
        }

        // ����Ŀǰ�Ի����ˢ�������Ի�UI
        public void UpdateChatPenel()
        {
            // ����Ի�������Χ ��ô�رնԻ�����
            if (currentChatDataID >= chatData.chatPieces.Count)
            {
                chatData = null;
                chatBoxPanel.SetActive(false);
                return;
            }

            // ���¶Ի�����
            chatBox.UpdateChatBox(chatData.chatPieces[currentChatDataID]);

            // ����ѡ������
            chatOptions.UpdateOptions(chatData.chatPieces[currentChatDataID]);
        }

        private void ContinueNextChat()
        {
            currentChatDataID += 1;

            UpdateChatPenel();
        }

        public void CreateNameChatButton(string name, ChatData_SO chatData_SO)
        {
            // ɾ�����еĶԻ�ѡ��
            DestroyNameChatButtons();

            // �����µĶԻ�ѡ��
            NPCChatButtonUI chatButton = Instantiate(nameChatButtonPrefab, NPCNamePanel.transform).GetComponent< NPCChatButtonUI>();
            // ��������
            chatButton.SetChaterName(name, chatData_SO);

        }

        public void DestroyNameChatButtons()
        {
            // ɾ�����еĶԻ�ѡ��
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
