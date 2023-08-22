using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class QuestRequest
    {
        [field: SerializeField] public Sprite requestSprite { get; private set; }
        [field: SerializeField][field: Range(1, 999)] public int requestCurrentAmount;
        [field: SerializeField][field: Range(1, 999)] public int requestMaxAmount;
    }
}
