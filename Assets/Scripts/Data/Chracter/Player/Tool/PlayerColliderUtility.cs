using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class ColliderUtility : PlayerColliderUtility
    {
        [field: SerializeField] public PlayerTriggerData PlayerTriggerData { get; private set; }

        public override void OnInitialize()
        {
            base.OnInitialize();

            PlayerTriggerData.Initialize();

        }
    }
}
