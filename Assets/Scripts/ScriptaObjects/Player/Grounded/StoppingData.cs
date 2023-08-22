using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class StoppingData
    {
        [field: SerializeField][field: Range(0f, 15f)] public float LightStoppingModifier { get; private set; } = 5f;
        [field: SerializeField][field: Range(0f, 15f)] public float MediumStoppingModifier { get; private set; } = 6.5f;
        [field: SerializeField][field: Range(0f, 15f)] public float HardStoppingModifier { get; private set; } = 8f;
    }
}
