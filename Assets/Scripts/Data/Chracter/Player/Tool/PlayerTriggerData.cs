using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class PlayerTriggerData
    {
        // ��˼���϶��Ļ�ı� ����ҲҪcash��
        [field:SerializeField]public BoxCollider GroundCheckCollider { get; private set; }
        [field:SerializeField]public Vector3 GroundCheckColliderExtents { get; private set; }

        // ��Ϊ�����й�����GroundCheckCollider��.bounds.extents;�����Ż����仯����������Ҫ��¼��ʼֵ�����������
        public void Initialize()
        {
            GroundCheckColliderExtents = GroundCheckCollider.bounds.extents;
        }
    }
}
