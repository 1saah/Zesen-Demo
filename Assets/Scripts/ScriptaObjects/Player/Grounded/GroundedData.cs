using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class GroundedData
    {
        [field: SerializeField] [field: Range(0f, 10f)]public float DefaultSpeed { get; private set; } = 5f;
        [field: SerializeField][field: Range(0f, 10f)] public float GroundCheckLayerDistance { get; private set; } = 1f;
        [field: SerializeField][field: Range(0f, 5f)] public float DefaultSpeedMultiplier { get; private set; } = 1f;
        [field: SerializeField][field: Range(0f, 5f)] public float DefaultSlopeSpeedMultiplier { get; private set; } = 1f;
        [field: SerializeField][field: Range(0f, 100f)] public float DefaultFloatVerticalForceMultiplier { get; private set; } = 25f;
        [field: SerializeField][field: Range(0f, 100f)] public float DefaultVerticalRaycastDistance { get; private set; } = 2f;
        [field: SerializeField] public AnimationCurve SlopeSpeedCurve { get; private set; }
        [field: SerializeField] public LayerMask floatLayerMask { get; private set; }
        [field: SerializeField] public RunData PlayerRunData { get; private set; }
        [field: SerializeField] public WalkData PlayerWalkData { get; private set; }
        [field: SerializeField] public RotationData PlayerRotationData { get; private set; }
        [field: SerializeField] public IdleData PlayerIDleData { get; private set; }
        [field: SerializeField] public DashData DashData { get; private set; }
        [field: SerializeField] public SprintData SprintData { get; private set; }
        [field: SerializeField] public StoppingData StoppingData { get; private set; }
        [field: SerializeField] public RollingData RollingData { get; private set; }

        public bool isTouchGround(int layer)
        {
            return checkTouchLayer(floatLayerMask, layer);
        }

        public bool checkTouchLayer(LayerMask layerMask, int layer)
        {
            return (1 << layer & layerMask) != 0;
        }
    }
}
