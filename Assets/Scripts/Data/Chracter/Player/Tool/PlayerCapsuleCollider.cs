using System;
using Unity.VisualScripting;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class PlayerCapsuleCollider
    {
        public CapsuleCollider CapsuleColliderRef { get; private set; }
        public Vector3 ColliderCenter { get; private set; }
        public Vector3 ColliderExtent { get; private set; }

        public void Initialize(GameObject player)
        {
            if (CapsuleColliderRef != null)
            {
                return;
            }

            CapsuleColliderRef = player.GetComponent<CapsuleCollider>();
        }

        public void UpdateColliderCenter()
        {
            ColliderCenter = CapsuleColliderRef.center;
            // always half the size
            ColliderExtent = new Vector3(0f, CapsuleColliderRef.bounds.extents.y, 0f);
        }
    }
}