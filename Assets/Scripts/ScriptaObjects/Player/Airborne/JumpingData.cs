using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class JumpingData
    {
        [field: SerializeField] public Vector3 StationaryJumpingFoece { get; private set; }
        [field: SerializeField] public Vector3 LightJumpingFoece { get; private set; }
        [field: SerializeField] public Vector3 MediumJumpingFoece { get; private set; }
        [field: SerializeField] public Vector3 HardJumpingFoece { get; private set; }
        [field:SerializeField] public RotationData RotationData { get; private set; }
        [field:SerializeField] public LayerMask JumpingLayer { get; private set; }
        [field:SerializeField] public AnimationCurve UpSlopeSpeedCurve { get; private set; }
        [field: SerializeField] public AnimationCurve DownSlopeSpeedCurve { get; private set; }
        [field: SerializeField][field: Range(0f, 2f)] public float GroundDetactedRayDistance { get; private set; } = 2f;
        [field: SerializeField][field: Range(0f, 10f)] public float VeticalDecellerateModifier { get; private set; } = 1.5f;
    }
}
