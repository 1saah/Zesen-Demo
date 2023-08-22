using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class HorizontalRecenteringUtility
    {
        [field: SerializeField][field: Range(-1f, 20f)] public float DefaultRecenteringWaitingTime { get; private set; } = 0f;
        [field: SerializeField][field: Range(-1f, 20f)] public float DefaultRecenteringTime { get; private set; } = 4f;
        [field: SerializeField] public CinemachineVirtualCamera VirtualCamera { get; private set; }
        private CinemachinePOV cinemechinePOV;

        public void Initialize()
        {
            cinemechinePOV = VirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        }

        public void EnableRecentering(float waitingTime = -1.0f, float recenteringTime = -1.0f)
        {
            if(waitingTime == -1f)
            {
                cinemechinePOV.m_HorizontalRecentering.m_WaitTime = DefaultRecenteringWaitingTime;
            }
            else
            {
                cinemechinePOV.m_HorizontalRecentering.m_WaitTime = waitingTime;
            }

            if (recenteringTime == -1f)
            {
                cinemechinePOV.m_HorizontalRecentering.m_RecenteringTime = DefaultRecenteringTime;
            }
            else
            {
                cinemechinePOV.m_HorizontalRecentering.m_RecenteringTime = recenteringTime;
            }

            // 清空当前的Recentering
            ResetRecentering();
            cinemechinePOV.m_HorizontalRecentering.m_enabled = true;
        }

        public void DisableRecentering()
        {
            cinemechinePOV.m_HorizontalRecentering.m_enabled = false;
        }

        public void ResetRecentering()
        {
            cinemechinePOV.m_HorizontalRecentering.CancelRecentering();
        }
    }
}
