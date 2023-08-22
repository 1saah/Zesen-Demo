using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class QuestReward
    {
        [field:SerializeField] public Sprite rewardSprite { get; private set; }
        [field: SerializeField][field: Range(1, 999)] public int rewardAmount;
    }
}
