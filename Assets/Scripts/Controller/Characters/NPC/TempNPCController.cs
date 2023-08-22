using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GenshinImpactMovement
{
    public class TempNPCController : MonoBehaviour
    {
        public GameObject stateIconUIPrefab;
        private NPCStateIconUI stateIconUI;
        public Transform NPCCanvas;
        public ChatData_SO chatData_SO;

        private string NPCName = "Zesen";

        // Start is called before the first frame update
        void Start()
        {
            stateIconUI = Instantiate(stateIconUIPrefab, NPCCanvas).GetComponent<NPCStateIconUI>();
            stateIconUI.SetStateIcon(transform.GetChild(1), false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player"&& chatData_SO != null)
            {
                // �򿪶Ի���ť
                ChatBoxManager.Instance.CreateNameChatButton(NPCName, chatData_SO);

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player" && chatData_SO != null)
            {
                // �رնԻ���ť
                ChatBoxManager.Instance.DestroyNameChatButtons();
            }
        }
    }
}
