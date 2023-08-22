using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GenshinImpactMovement
{
    public class ChatBox : MonoBehaviour
    {
        public TextMeshProUGUI chatTargetName;
        public TextMeshProUGUI chatContent;

        private void Awake()
        {
            chatTargetName = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            chatContent = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        }

        public void UpdateChatBox(ChatPiece piece)
        {
            chatTargetName.text = piece.chatName;
            chatContent.text = piece.chatContent;
        }
    }
}
