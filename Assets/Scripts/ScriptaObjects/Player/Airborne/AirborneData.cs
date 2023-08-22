using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class AirborneData
    {
        [field:SerializeField] public JumpingData JumpingData { get; private set; }
        [field:SerializeField] public FallingData FallingData { get; private set; }
        [field:SerializeField] public FlyingData FlyingData { get; private set; }

        [field: SerializeField][field: Range(0f, 10f)] public float LeastFlyingHeight { get; private set; } = 2f;
    }
}
