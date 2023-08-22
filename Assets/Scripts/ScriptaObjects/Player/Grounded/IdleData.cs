using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class IdleData
    {
        [field: SerializeField][field: Range(0f, 5f)] public float SpeedMultiplier { get; private set; } = 0f;

        [field: SerializeField] public List<CameraRecenteringData> BackwardsRecenteringData;
    }
}
