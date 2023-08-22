using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class EFightData
    {
        [field: SerializeField][field: Range(0f, 20f)] public float fightInterval { get; private set; } = 1f;
    }
}
