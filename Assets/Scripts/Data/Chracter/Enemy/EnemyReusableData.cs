using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    public class EnemyReusableData
    {
        public float enemyIdleWaitingTimer { get; set; }
        public float enemyFightWaitingTimer { get; set; }
        public Vector3 speedMdifier { get; set; }
        public Vector3 originalPos { get; set; }

        private Vector3 timeToReachTargetRotation = new Vector3(0f, 0.14f, 0f);
        private Vector3 timerToReachTargetRotation = Vector3.zero;
        private Vector3 eulerVelocity;

        public ref Vector3 TimeToReachTargetRotation
        {
            get { return ref timeToReachTargetRotation; }

        }

        public ref Vector3 TimerToReachTargetRotation
        {
            get { return ref timerToReachTargetRotation; }
        }

        public ref Vector3 EulerVelocity
        {
            get { return ref eulerVelocity; }
        }
    }
}
