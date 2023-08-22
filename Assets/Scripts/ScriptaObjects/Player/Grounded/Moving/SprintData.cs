using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class SprintData
    {
        [field: SerializeField] [field:Range(0f,2f)]public float SprintModifier { get; private set; } = 1.7f;
        [field: SerializeField][field: Range(0f, 2f)] public float SprintTime { get; private set; } = 1.0f;
        [field: SerializeField][field: Range(0f, 2f)] public float RunToWalkTime { get; private set; } = 1.0f;
    }
}
