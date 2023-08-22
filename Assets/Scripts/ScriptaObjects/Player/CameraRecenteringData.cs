using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    // Camera Recentering Data��ģ�壬����Ϊ��ͬ��״̬�����������ǵ�Camera Recentering Data
    [Serializable]
    public class CameraRecenteringData
    {
        [field: SerializeField][field: Range(0f, 90f)] public float MaximumAngle { get; private set; }
        [field: SerializeField][field: Range(0f, 90f)] public float MinimumAngle { get; private set; }
        [field: SerializeField][field: Range(-1f, 20f)] public float WaitingTime { get; private set; }
        [field: SerializeField][field: Range(-1f, 20f)] public float RecenteringTime { get; private set; }

        // �ж�������ǉ�Ƕ��Ƿ������Recentering�Ƕȷ�Χ��
        public bool IsWithingLimimation(float eulerAngle)
        {
            return eulerAngle <= MaximumAngle && eulerAngle >= MinimumAngle;
        }

    }
}
