using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace GenshinImpactMovement
{
    public class AirborneState : MovementState
    {
        public AirborneState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
           
        }

        #region Istate Methods
        public override void Enter()
        {
            base.Enter();
            StartAnimation(StateMachine.Controller.animatorDataUtility.airbroneHash);
            ResetSprintingState();
        }

        public override void OnTriggerEnter(Collider collider)
        {
            if (groundedData.isTouchGround(collider.gameObject.layer))
            {
                OnTouchedGround();
            }
        }

        protected virtual void OnTouchedGround()
        {
            StateMachine.ChangeState(StateMachine.LightLandingState);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(StateMachine.Controller.animatorDataUtility.airbroneHash);
        }

        #endregion

        #region Reusable Methods
        // 打断冲刺状态(除了跳跃)，所以跳跃需要重写
        protected virtual void ResetSprintingState()
        {
            StateMachine.ReusableData.isSprinting = false;
        }
        #endregion
    }
}
