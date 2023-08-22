using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovement
{
    public class JumpingState : AirborneState
    {
        protected JumpingData jumpingData;
        public bool shouldRotate;
        private bool canFallingDown;

        public JumpingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            jumpingData = airborneData.JumpingData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            // 首先跳跃的时候不能移动
            StateMachine.ReusableData.speedMultiplier = 0f;

            StateMachine.ReusableData.speedDecelerateMultiplier = jumpingData.VeticalDecellerateModifier;
            shouldRotate = StateMachine.ReusableData.input != Vector2.zero;

            StateMachine.ReusableData.RotationData = jumpingData.RotationData;
            StateMachine.ReusableData.TimeToReachTargetRotation.y = StateMachine.ReusableData.RotationData.TimeToReachTargetRotation.y;

            Jump();
        }

        public override void Update()
        {
            base.Update();
            
            // 下面是跳跃过程中切换下落的逻辑
            // 首先排除因为floating capsule以影响
            if (!canFallingDown && isUpMove())
            {
                canFallingDown = true;
            }

            if(canFallingDown && isDownMove()) 
            {
                StateMachine.ChangeState(StateMachine.FallingState);
            }
            
        }

        public override void PhysicalUpdate()
        {
            base.PhysicalUpdate();

            // 如果跳跃的时候点了方向键 那么要移动到这个方向
            if(shouldRotate)
            {
                SmoothDampRotate();
            }

            // 给上升一个减速度
            if(isUpMove())
            {
                VerticalDecellerate();
            }

        }

        public override void Exit()
        {
            base.Exit();
            SetBaseRotaionData();
            canFallingDown = false;
        }

        #endregion

        #region Main Methods
        private void Jump()
        {
            // 首先需要重置速度
            ResetVerticalVelocity();

            // 找到目标移动方向
            Vector3 playerFoward = StateMachine.Controller.transform.forward;

            // 得到目前跳跃的力度
            Vector3 jumpForce = StateMachine.ReusableData.JumpingFoece;

            // 如果跳跃的时候点击了方向键 那么需要向这个方向移动
            if(shouldRotate)
            {
                ResetEulerSmoothDampData(GetTartYEulerAngleBasedOnCamera());

                playerFoward =  GetMovementDirection(GetTartYEulerAngleBasedOnCamera());

            }

            Ray capsuleCenterDownRay = new Ray(StateMachine.Controller.PlayerFloatUtility.CapsuleColliderData.ColliderCenter, Vector3.down);
            if(Physics.Raycast(capsuleCenterDownRay, out RaycastHit hitInfo, jumpingData.GroundDetactedRayDistance, jumpingData.JumpingLayer, QueryTriggerInteraction.Ignore))
            {
                float slopeAngle = Vector3.Angle(hitInfo.normal, -capsuleCenterDownRay.direction);
                // 上坡
                if (isUpMove())
                {
                    float upSlopeJumpingSpeedModifier = jumpingData.UpSlopeSpeedCurve.Evaluate(slopeAngle);
                    jumpForce.x *= upSlopeJumpingSpeedModifier;
                    jumpForce.z *= upSlopeJumpingSpeedModifier;
                }

                // 下坡
                if (isDownMove())
                {
                    float downSlopeJumpingSpeedModifier = jumpingData.UpSlopeSpeedCurve.Evaluate(slopeAngle);
                    jumpForce.x *= downSlopeJumpingSpeedModifier;
                    jumpForce.z *= downSlopeJumpingSpeedModifier;
                }
            }

            // 得到最终跳跃的量
            jumpForce.x *= playerFoward.x;
            jumpForce.z *= playerFoward.z;

            // 让人物跳跃
            StateMachine.Controller.Rigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
            
        }
        #endregion

        #region Reusable Methods
        // 打断冲刺状态(除了跳跃)，所以跳跃需要重写
        protected override void ResetSprintingState()
        {
            
        }

        protected override void MovementCanceled(InputAction.CallbackContext obj)
        {
            
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
