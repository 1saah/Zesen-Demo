using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class FlyingData
    {
        [field: SerializeField][field: Range(0f, 10f)] public float FlySpeedModified { get; private set;} = 1.3f;
        [field: SerializeField] public RotationData RotationData { get; private set; }
        [field: SerializeField][field: Range(0f, 10f)] public float GravityModified { get; private set; } = 0.5f;
    }
}
