using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [Serializable]
    public class AnimatorDataUtility
    {
        [Header("SubStateMachine Parameter")]
        [field: SerializeField] private string airbroneHashParameter = "Airbrone";
        [field: SerializeField] private string groundedHashParameter = "Grounded";
        [field: SerializeField] private string movingHashParameter = "Moving";
        [field: SerializeField] private string stoppingHashParameter = "Stopping";
        [field: SerializeField] private string landingHashParameter = "Landing";

        [Header("Animation Parameter")]
        [field: SerializeField] private string isFallingHashParameter = "isFalling";
        [field: SerializeField] private string isIdlingHashParameter = "isIdling";
        [field: SerializeField] private string isSprintingHashParameter = "isSprinting";
        [field: SerializeField] private string isWalkingHashParameter = "isWalking";
        [field: SerializeField] private string isHardStoppingHashParameter = "isHardStopping";
        [field: SerializeField] private string isMediumStoppingHashParameter = "isMediumStopping";
        [field: SerializeField] private string isHardLandingHashParameter = "isHardLanding";
        [field: SerializeField] private string isRollingHashParameter = "isRolling";
        [field: SerializeField] private string isDashingHashParameter = "isDashing";
        [field: SerializeField] private string isFlyingHashParameter = "isFlying";

        // Cashing SubStateMachine Hashing
        public int airbroneHash { get; private set; }
        public int groundedHash { get; private set; }
        public int movingHash { get; private set; }
        public int stoppingHash { get; private set; }
        public int landingHash { get; private set; }
        // Cashing Animation Hashing
        public int isFallingHash { get; private set; }
        public int isIdlingHash { get; private set; }
        public int isSprintingHash { get; private set; }
        public int isWalkingHash { get; private set; }
        public int isHardStoppingHash { get; private set; }
        public int isMediumStoppingHash { get; private set; }
        public int isHardLandingHash { get; private set; }
        public int isRollingHash { get; private set; }
        public int isDashingHash { get; private set; }
        public int isFlyingHash { get; private set; }

        public void Initialize()
        {
            airbroneHash = Animator.StringToHash(airbroneHashParameter);
            groundedHash = Animator.StringToHash(groundedHashParameter);
            movingHash = Animator.StringToHash(movingHashParameter);
            stoppingHash = Animator.StringToHash(stoppingHashParameter);
            landingHash = Animator.StringToHash(landingHashParameter);

            isFallingHash = Animator.StringToHash(isFallingHashParameter);
            isIdlingHash = Animator.StringToHash(isIdlingHashParameter);
            isSprintingHash = Animator.StringToHash(isSprintingHashParameter);
            isWalkingHash = Animator.StringToHash(isWalkingHashParameter);
            isHardStoppingHash = Animator.StringToHash(isHardStoppingHashParameter);
            isMediumStoppingHash = Animator.StringToHash(isMediumStoppingHashParameter);
            isHardLandingHash = Animator.StringToHash(isHardLandingHashParameter);
            isRollingHash = Animator.StringToHash(isRollingHashParameter);
            isDashingHash = Animator.StringToHash(isDashingHashParameter);
            isFlyingHash = Animator.StringToHash(isFlyingHashParameter);
        }
    }
}
