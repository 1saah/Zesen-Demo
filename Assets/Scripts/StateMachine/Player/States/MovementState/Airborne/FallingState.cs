using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovement
{
    public class FallingState : AirborneState
    {
        protected FallingData fallingData;
        protected Vector3 fallHighestPoint;

        public FallingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            fallingData = airborneData.FallingData;
        }

        #region Istate Methods
        public override void Enter()
        {
            base.Enter();
            StartAnimation(StateMachine.Controller.animatorDataUtility.isFallingHash);
            fallHighestPoint = StateMachine.Controller.transform.position;
            ResetVerticalVelocity();
        }

        public override void PhysicalUpdate()
        {
            base.PhysicalUpdate();

            // 保持下落速度恒定
            LimitFallingSpeed();
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(StateMachine.Controller.animatorDataUtility.isFallingHash);
        }

        #endregion

        #region Main Methods
        private void LimitFallingSpeed()
        {
            float verticalSpeed = GetVerticalSpeed();
            if (-verticalSpeed < fallingData.FallingSpeedLimit)
            {
                return;
            }

            Vector3 speedDifference = new Vector3(0f, -verticalSpeed - fallingData.FallingSpeedLimit, 0f);

            StateMachine.Controller.Rigidbody.AddForce(speedDifference, ForceMode.VelocityChange);
        }
        #endregion

        #region Reusable Methods
        protected override void ResetSprintingState()
        {
            
        }

        protected override void OnTouchedGround()
        {
            // 这里不用Mathf.Abs()是因为像上平台跳跃也会使用lightlanding
            float dis = fallHighestPoint.y - StateMachine.Controller.transform.position.y;

            if(dis < fallingData.MinimalFallingDistance) 
            {
                StateMachine.ChangeState(StateMachine.LightLandingState);

                return;
            }

            if(StateMachine.ReusableData.input == Vector2.zero|| (StateMachine.ReusableData.isSprinting == false&& StateMachine.ReusableData.isToggle == true))
            {
                StateMachine.ChangeState(StateMachine.HardLandingState);

                return;
            }

            StateMachine.ChangeState(StateMachine.RollingState);
        }
        #endregion

        #region Input Methods
        protected override void AddInputReaction()
        {
            base.AddInputReaction();
            StateMachine.Controller.Input.PlayerActions.Jump.started += FlyingReact;
        }

        protected override void RemoveInputReaction()
        {
            base.RemoveInputReaction();
            StateMachine.Controller.Input.PlayerActions.Jump.started -= FlyingReact;
        }


        private void FlyingReact(InputAction.CallbackContext context)
        {
            if (CanTransformToFlyingState())
            {
                StateMachine.ChangeState(StateMachine.FlyingState);
            }
        }
        #endregion
    }
}
