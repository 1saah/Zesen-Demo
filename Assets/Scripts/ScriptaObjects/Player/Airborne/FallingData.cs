using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class FallingData
    {
        [field: SerializeField][field: Range(0f, 20f)] public float FallingSpeedLimit { get; private set; } = 15f;
        [field: SerializeField][field: Range(0f, 20f)] public float MinimalFallingDistance { get; private set; } = 3f;
    }
}
