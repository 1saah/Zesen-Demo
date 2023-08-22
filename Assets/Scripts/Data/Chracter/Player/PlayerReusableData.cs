using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    // ��Ϊ����Ҫ��inspector���޸� ���ǲ���Ҫ[Serializable]��[field:SerialzebleField]
    public class PlayerReusableData
    {
        public float speedMultiplier { get; set; }
        public float speedSlopeMultiplier { get; set; } = 1f;
        public float speedDecelerateMultiplier { get; set; } = 5f;
        public List<CameraRecenteringData> BackwardsRecenteringData;
        public List<CameraRecenteringData> SidewayRecenteringData;
        public Vector2 input { get; set; }
        public bool isToggle { get; set; }
        public bool isSprinting { get; set; }
        private Vector3 recordEulerRoration;
        private Vector3 timeToReachTargetRotation;
        private Vector3 timerToReachTargetRotation;
        private Vector3 eulerVelocity;

        public RotationData RotationData { get; set; }

        public Vector3 JumpingFoece { get; set; }
        // �����������޸�Vector3��x,y,z����ô��Ҫ��������Ϊref
        // ��ΪVector3��struct���� ֵ����
        public ref Vector3 RecordEulerRoration
        {
            get
            {
                return ref recordEulerRoration;
            }
        }

        public ref Vector3 TimeToReachTargetRotation
        {
            get
            {
                return ref timeToReachTargetRotation;
            }
        }

        public ref Vector3 TimerToReachTargetRotation
        {
            get
            {
                return ref timerToReachTargetRotation;
            }
        }

        public ref Vector3 EulerVelocity
        {
            get
            {
                return ref eulerVelocity;
            }
        }
    }
}
