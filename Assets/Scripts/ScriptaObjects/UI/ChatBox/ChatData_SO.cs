using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [CreateAssetMenu(fileName = "ChatData_SO", menuName = "Data/UI/ChatData_SO")]
    public class ChatData_SO : ScriptableObject
    {
        // 添加功能： 有内容改变的时候自动重新排序编号

        [field: SerializeField] public List<ChatPiece> chatPieces { get; private set; }

        #region Default Methods
        private void OnValidate()
        {
            
        }
        #endregion

        #region Reusable Methods
        private void ReOrderChatPiecesNumber()
        {

        }
        #endregion
    }
}
