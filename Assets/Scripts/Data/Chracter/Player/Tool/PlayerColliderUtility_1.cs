using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    // Collider.center�Ǿֲ����������
    // Collider.bounds.center���������������
    [Serializable]
    public class PlayerColliderUtility
    {
        // ��ײ���� ��ϣ����������Լ��ڲ�Ѱ������ ���Բ���[field:SerializeField]
        [field: SerializeField] public PlayerCapsuleCollider CapsuleColliderData { get; private set; }
        // ��ײ����Ϣ��
        [field:SerializeField] public PlayerColliderDefaultData PlayerColliderDefaultData { get; private set;}
        // �¶���
        [field: SerializeField] public PlayerSlopeDefaultData PlayerSlopeDefaultData { get; private set; }

        public void Initialize(GameObject player)
        {
            OnInitialize();

            CapsuleColliderData.Initialize(player);
            // ���¼���colliderλ����Ϣ
            CalculateFloatDemension();
        }

        public virtual void OnInitialize()
        {
            
        }

        public void CalculateFloatDemension()
        {
            // ����Ĭ�ϴ洢��Ϣ����Collider�߶� 
            CalculateColliderHeight();
            // ����Ĭ�ϴ洢��Ϣ����Collider�뾶
            CalculateColliderRadius();
            // ����Ĭ�ϴ洢��Ϣ���¼���Collider������ 
            CalculateColliderCenter();
            // ����CapsuleColliderData��������Ϣ
            CapsuleColliderData.UpdateColliderCenter();
        }


        private void CalculateColliderHeight()
        {
            CapsuleColliderData.CapsuleColliderRef.height = PlayerColliderDefaultData.ColliderDefaultHeight * (1f - PlayerSlopeDefaultData.Slope);
        }

        private void CalculateColliderRadius()
        {
            // ���1/2�߶�С�ڰ뾶����ô��Ҫ��С�뾶
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
