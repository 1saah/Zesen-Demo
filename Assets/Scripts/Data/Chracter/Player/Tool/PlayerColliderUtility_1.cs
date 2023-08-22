using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    // Collider.center是局部坐标的中心
    // Collider.bounds.center是世界坐标的中心
    [Serializable]
    public class PlayerColliderUtility
    {
        // 碰撞体类 我希望这个类是自己内部寻找引用 所以不用[field:SerializeField]
        [field: SerializeField] public PlayerCapsuleCollider CapsuleColliderData { get; private set; }
        // 碰撞体信息类
        [field:SerializeField] public PlayerColliderDefaultData PlayerColliderDefaultData { get; private set;}
        // 坡度类
        [field: SerializeField] public PlayerSlopeDefaultData PlayerSlopeDefaultData { get; private set; }

        public void Initialize(GameObject player)
        {
            OnInitialize();

            CapsuleColliderData.Initialize(player);
            // 从新计算collider位置信息
            CalculateFloatDemension();
        }

        public virtual void OnInitialize()
        {
            
        }

        public void CalculateFloatDemension()
        {
            // 根据默认存储信息设置Collider高度 
            CalculateColliderHeight();
            // 根据默认存储信息设置Collider半径
            CalculateColliderRadius();
            // 根据默认存储信息从新计算Collider的中心 
            CalculateColliderCenter();
            // 更新CapsuleColliderData的中心信息
            CapsuleColliderData.UpdateColliderCenter();
        }


        private void CalculateColliderHeight()
        {
            CapsuleColliderData.CapsuleColliderRef.height = PlayerColliderDefaultData.ColliderDefaultHeight * (1f - PlayerSlopeDefaultData.Slope);
        }

        private void CalculateColliderRadius()
        {
            // 如果1/2高度小于半径，那么需要缩小半径
            if(PlayerColliderDefaultData.ColliderDefaultRadius > CapsuleColliderData.CapsuleColliderRef.height / 2)
            {
                CapsuleColliderData.CapsuleColliderRef.radius = CapsuleColliderData.CapsuleColliderRef.height / 2;
            }
            else
            {
                CapsuleColliderData.CapsuleColliderRef.radius = PlayerColliderDefaultData.ColliderDefaultRadius;
            }
        }

        private void CalculateColliderCenter()
        {
            float heightDifference = PlayerColliderDefaultData.ColliderDefaultHeight - CapsuleColliderData.CapsuleColliderRef.height;
            float centerY = PlayerColliderDefaultData.ColliderDefaultYCenter + heightDifference / 2;
            CapsuleColliderData.CapsuleColliderRef.center = new Vector3(0f, centerY, 0f);
        }

    }
}
