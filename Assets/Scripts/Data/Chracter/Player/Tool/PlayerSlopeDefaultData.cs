using System;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class PlayerSlopeDefaultData
    {
        // 允许通过的最大坡度百分比（按照人物的高度）
        [field: SerializeField][field: Range(0f, 1f)] public float Slope { get; private set; } = 0.25f;
    }
}