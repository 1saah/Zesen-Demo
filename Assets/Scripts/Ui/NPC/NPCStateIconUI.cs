using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GenshinImpactMovement
{
    public class NPCStateIconUI : MonoBehaviour
    {
        public Transform mainCam;
        public Transform npcStateIconTrans;
        public Sprite freeStateIcon;
        public Sprite busyStateIcon;
        public Image stateIcon;
        public bool isBusy;

        private void Awake()
        {
            stateIcon = GetComponent<Image>();
            mainCam = Camera.main.transform;
        }

        private void LateUpdate()
        {
            UpdateIconState();
        }

        public void SetStateIcon(Transform stateIconTrans, bool busy)
        {
            npcStateIconTrans = stateIconTrans;
            isBusy = busy;
        }

        // 根据任务状态修改状态图标
        private void UpdateIconState()
        {
            if(isBusy)
            {
                stateIcon.sprite = busyStateIcon;
            }
            else
            {
                stateIcon.sprite = freeStateIcon;
            }

            // 根据NPC坐标和摄像机角度调整UI
            transform.position = npcStateIconTrans.position;
            transform.forward = -mainCam.transform.forward;
        }

    }
}
