using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class EPatrolData
    {
        [field: SerializeField][field: Range(0f, 10f)] public float PatrolSpeedMutifier { get; private set; } = 2f;
        [field: SerializeField][field: Range(0f, 10f)] public float PatrolRadius { get; private set; } = 10f;
        [field: SerializeField][field: Range(0f, 2f)] public float StoppingDistance { get; private set; } = 0.1f;
    }
}
