using System;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class PlayerColliderDefaultData
    {
        // 模型默认高度
        [field: SerializeField] [field:Range(0f, 5f)]public float ColliderDefaultHeight { get; private set; } = 1.8f;
        // 默认半径
        [field: SerializeField][field: Range(0f, 5f)] public float ColliderDefaultRadius { get; private set; } = 0.2f;

        [field: SerializeField][field: Range(0f, 5f)] public float ColliderDefaultYCenter { get; private set; } = 0.9f;
    }
}