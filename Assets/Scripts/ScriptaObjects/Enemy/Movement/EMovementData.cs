using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class EMovementData
    {
        [field: SerializeField] public EIdleData EIdleData { get; private set; }
        [field: SerializeField] public EPatrolData EPatrolData { get; private set; }
        [field: SerializeField] public EMovingData EMovingData { get; private set; }
        [field: SerializeField] public EFightData EFightData { get; private set; }

        [field: SerializeField][field: Range(0f, 10f)] public float chasingDis { get; private set; } = 10f;
        [field: SerializeField][field: Range(0f, 10f)] public float fightDis { get; private set; } = 3f;
        [field: SerializeField][field: Range(0f, 10f)] public float stoppingDis { get; private set; } = 1f;
    }
}
