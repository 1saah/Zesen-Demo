using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class OptionPiece
    {
        [field: SerializeField] public string optionContent { get; private set; } = "Option Content";
        [field: SerializeField] public int optionTarget { get; private set; } = -1;
        [field: SerializeField] public bool isTakingTask { get; private set; }
        [field: SerializeField] public Quest_SO Quest_SO { get; private set; }
    }
}
