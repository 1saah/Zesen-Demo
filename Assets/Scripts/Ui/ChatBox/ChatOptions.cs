using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    public class ChatOptions : MonoBehaviour
    {
        public GameObject optionPrefab;

        public void UpdateOptions(ChatPiece chatPiece)
        {
            ClearAllOptions();
            CreateNewOptions(chatPiece);
        }

        private void ClearAllOptions()
        {
            int optionNum = transform.childCount;

            // É¾³ý×ÓÑ¡Ïî
            if (transform.childCount > 0)
            {
                for (int i = optionNum - 1; i >= 0; i--)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
            }
        }

        private void CreateNewOptions(ChatPiece chatPiece)
        {
            if(chatPiece.optionPieceList.Count > 0)
            {
                foreach (var i in chatPiece.optionPieceList)
                {
                    ChatOption option = Instantiate(optionPrefab, transform).GetComponent<ChatOption>();
                    option.UpdateOption(i);
                }
            }
        }
    }
}
