using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class RotationData
    {
        [field: SerializeField] public Vector3 TimeToReachTargetRotation { get; private set; } = new Vector3(0f, 0.14f, 0f);
    }
}
