using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class PlayerTriggerData
    {
        // 意思是拖动的会改变 所以也要cash？
        [field:SerializeField]public BoxCollider GroundCheckCollider { get; private set; }
        [field:SerializeField]public Vector3 GroundCheckColliderExtents { get; private set; }

        // 因为在运行过程中GroundCheckCollider的.bounds.extents;会随着环境变化，所以我们要记录初始值方便后续计算
        public void Initialize()
        {
            GroundCheckColliderExtents = GroundCheckCollider.bounds.extents;
        }
    }
}
