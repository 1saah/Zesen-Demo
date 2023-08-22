using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    // Camera Recentering Data的模板，可以为不同的状态创建属于他们的Camera Recentering Data
    [Serializable]
    public class CameraRecenteringData
    {
        [field: SerializeField][field: Range(0f, 90f)] public float MaximumAngle { get; private set; }
        [field: SerializeField][field: Range(0f, 90f)] public float MinimumAngle { get; private set; }
        [field: SerializeField][field: Range(-1f, 20f)] public float WaitingTime { get; private set; }
        [field: SerializeField][field: Range(-1f, 20f)] public float RecenteringTime { get; private set; }

        // 判断输入优菈角度是否在这个Recentering角度范围内
        public bool IsWithingLimimation(float eulerAngle)
        {
            return eulerAngle <= MaximumAngle && eulerAngle >= MinimumAngle;
        }

    }
}
