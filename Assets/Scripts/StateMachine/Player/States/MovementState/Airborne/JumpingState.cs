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

            // ������Ծ��ʱ�����ƶ�
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
            
            // ��������Ծ�������л�������߼�
            // �����ų���Ϊfloating capsule��Ӱ��
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

            // �����Ծ��ʱ����˷���� ��ôҪ�ƶ����������
            if(shouldRotate)
            {
                SmoothDampRotate();
            }

            // ������һ�����ٶ�
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
            // ������Ҫ�����ٶ�
            ResetVerticalVelocity();

            // �ҵ�Ŀ���ƶ�����
            Vector3 playerFoward = StateMachine.Controller.transform.forward;

            // �õ�Ŀǰ��Ծ������
            Vector3 jumpForce = StateMachine.ReusableData.JumpingFoece;

            // �����Ծ��ʱ�����˷���� ��ô��Ҫ����������ƶ�
            if(shouldRotate)
            {
                ResetEulerSmoothDampData(GetTartYEulerAngleBasedOnCamera());

                playerFoward =  GetMovementDirection(GetTartYEulerAngleBasedOnCamera());

            }

            Ray capsuleCenterDownRay = new Ray(StateMachine.Controller.PlayerFloatUtility.CapsuleColliderData.ColliderCenter, Vector3.down);
            if(Physics.Raycast(capsuleCenterDownRay, out RaycastHit hitInfo, jumpingData.GroundDetactedRayDistance, jumpingData.JumpingLayer, QueryTriggerInteraction.Ignore))
            {
                float slopeAngle = Vector3.Angle(hitInfo.normal, -capsuleCenterDownRay.direction);
                // ����
                if (isUpMove())
                {
                    float upSlopeJumpingSpeedModifier = jumpingData.UpSlopeSpeedCurve.Evaluate(slopeAngle);
                    jumpForce.x *= upSlopeJumpingSpeedModifier;
                    jumpForce.z *= upSlopeJumpingSpeedModifier;
                }

                // ����
                if (isDownMove())
                {
                    float downSlopeJumpingSpeedModifier = jumpingData.UpSlopeSpeedCurve.Evaluate(slopeAngle);
                    jumpForce.x *= downSlopeJumpingSpeedModifier;
                    jumpForce.z *= downSlopeJumpingSpeedModifier;
                }
            }

            // �õ�������Ծ����
            jumpForce.x *= playerFoward.x;
            jumpForce.z *= playerFoward.z;

            // ��������Ծ
            StateMachine.Controller.Rigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
            
        }
        #endregion

        #region Reusable Methods
        // ��ϳ��״̬(������Ծ)��������Ծ��Ҫ��д
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
