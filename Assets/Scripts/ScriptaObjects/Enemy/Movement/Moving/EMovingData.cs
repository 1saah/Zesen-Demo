using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class EMovingData
    {
        [field: SerializeField] public EBackData EBackData { get; private set; }
        [field: SerializeField] public EChasingData EChasingData { get; private set; }
    }
}
