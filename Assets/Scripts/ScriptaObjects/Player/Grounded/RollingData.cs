using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class RollingData
    {
        [field: SerializeField][field: Range(0f, 10f)] public float RollingSpeedModifier { get; private set; } = 1f;
    }
}
