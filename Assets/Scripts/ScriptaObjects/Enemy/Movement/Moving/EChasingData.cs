using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class EChasingData 
    {
        [field: SerializeField][field: Range(0f, 10f)] public float ChasingSpeedMutifier { get; private set; } = 5f;
        [field: SerializeField][field: Range(0f, 2f)] public float StoppingDistance { get; private set; } = 1f;
    }
}
