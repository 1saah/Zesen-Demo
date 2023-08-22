using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class DashData
    {
        [field: SerializeField][field: Range(0f, 2f)] public float DashSpeedModifier { get; private set; } = 2f;
        [field: SerializeField][field: Range(0f, 2f)] public float DashConductiveTime { get; private set; } = 1f;
        [field: SerializeField][field: Range(0, 2)] public int DashConductiveCount { get; private set; } = 2;
        [field: SerializeField][field: Range(0f, 2f)] public float DashConductiveCoolDown { get; private set; } = 2f;
        [field:SerializeField] public RotationData DashRotationData { get; private set; }
    }
}
