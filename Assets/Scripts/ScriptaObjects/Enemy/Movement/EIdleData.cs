using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class EIdleData
    {
        [field: SerializeField][field: Range(0f, 20f)] public float IdlePatrolWaittingTime { get; private set; } = 5f;
    }
}
