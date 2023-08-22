using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class ChatPiece
    {
        [field: SerializeField] public string chatName { get; private set; } = "Name";
        [field:TextArea]
        [field: SerializeField] public string chatContent { get; private set; } = "Here is ChatContent...";
        [field: SerializeField] public int chatNumber { get; private set; } = -1;
        [field: SerializeField] public List<OptionPiece> optionPieceList { get; private set; }

    }
}
