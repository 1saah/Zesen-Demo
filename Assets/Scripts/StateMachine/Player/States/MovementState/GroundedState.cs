using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovement
{
    public class GroundedState : MovementState
    {
        protected JumpingData jumpingData;
        public GroundedState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            jumpingData = StateMachine.Controller.playerData_SO.AirborneData.JumpingData;
        }

        #region Istate Methods
        public override void Enter()
        {
            base.Enter();
            StartAnimation(StateMachine.Controller.animatorDataUtility.groundedHash);
        }

        public override void Update()
        {
            base.Update();
            UpdateSprintingState();
        }

        public override void PhysicalUpdate()
        {
            base.PhysicalUpdate();
            CapsuleColliderFloating();
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(StateMachine.Controller.animatorDataUtility.groundedHash);
        }
        #endregion

        #region Main Methods

        public void CapsuleColliderFloating()
        {
            // ������Capsule Collider���������
            Ray rayFromCerterToGround = new Ray(StateMachine.Controller.PlayerFloatUtility.CapsuleColliderData.CapsuleColliderRef.bounds.center, Vector3.down);
            if (Physics.Raycast(rayFromCerterToGround, out RaycastHit hit, groundedData.DefaultVerticalRaycastDistance, groundedData.floatLayerMask, QueryTriggerInteraction.Ignore))
            {
                float playerVerticalSpeed = GetVerticalSpeed();
                float diatanceToFloatPoint = StateMachine.Controller.PlayerFloatUtility.CapsuleColliderData.ColliderCenter.y * StateMachine.Controller.transform.localScale.y - hit.distance;
                // �ж����߾��� �Ƿ������ײ�����ĸ߶�
                if (diatanceToFloatPoint == 0f)
                {
                    return;
                }

                // �ᴥ����ʱ��ֻ����������ʱ��
                // ��ʱ����ȥԭ����ֱ�����ٶȵ�Ӱ��
                float amountToLift = diatanceToFloatPoint * groundedData.DefaultFloatVerticalForceMultiplier - playerVerticalSpeed;

                StateMachine.Controller.Rigidbody.AddForce(new Vector3(0f, amountToLift, 0f), ForceMode.VelocityChange);

                // �����¶Ⱥ����߼н�
                float slopeAngle = Vector3.Angle(-rayFromCerterToGround.direction, hit.normal);
                // �����¶��޸��ƶ��ٶ�
                StateMachine.ReusableData.speedSlopeMultiplier = GetSlopeSpeedModifier(slopeAngle);

            }
        }

        #endregion


        #region ReUsable Methods
        protected override void ExamineLeavingGround()
        {
            // �ų��ѷ��С��ɧ��
            if(TouchTriggerGround())
            {
                return;
            }

            // �����������Ƿ��㹻
            CapsuleCollider capsuleCollider = StateMachine.Controller.PlayerFloatUtility.CapsuleColliderData.CapsuleColliderRef;
            Vector3 capsuleColliderCenter = capsuleCollider.bounds.center;
            Ray ray = new Ray(capsuleColliderCenter - StateMachine.Controller.PlayerFloatUtility.CapsuleColliderData.ColliderExtent, Vector3.down);
            if(!Physics.Raycast(ray, out _, groundedData.GroundCheckLayerDistance, groundedData.floatLayerMask, QueryTriggerInteraction.Ignore))
            {
                Falling();
            }
        }

        protected virtual void Falling()
        {
            StateMachine.ChangeState(StateMachine.FallingState);
        }

        // ���ڱ���Sprint״̬���������û��movement���룬��ô���ñ���
        protected void UpdateSprintingState()
        {
            if(!StateMachine.ReusableData.isSprinting)
            {
                return;
            }

            if(StateMachine.ReusableData.input != Vector2.zero)
            {
                return;
            }

            StateMachine.ReusableData.isSprinting = false;
        }

        protected virtual void OnMove()
        {
            if(StateMachine.ReusableData.isSprinting)
            {
                StateMachine.ChangeState(StateMachine.SprintingState);
                return;
            }

            if (StateMachine.ReusableData.isToggle)
            {
                StateMachine.ChangeState(StateMachine.WalkingState);
                return;
            }
            else
            {
                StateMachine.ChangeState(StateMachine.RunningState);
                return;
            }
        }
        protected float GetSlopeSpeedModifier(float slopeRadius)
        {
            return groundedData.SlopeSpeedCurve.Evaluate(slopeRadius);
        }

        protected override void AddInputReaction()
        {
            base.AddInputReaction();
            StateMachine.Controller.Input.PlayerActions.Dash.started += DashStartedReaction;
            StateMachine.Controller.Input.PlayerActions.Jump.started += JumpStartedReaction;
        }


        protected override void RemoveInputReaction()
        {
            base.RemoveInputReaction();
            StateMachine.Controller.Input.PlayerActions.Dash.started -= DashStartedReaction;
            StateMachine.Controller.Input.PlayerActions.Jump.started -= JumpStartedReaction;
        }

        protected virtual void DashStartedReaction(InputAction.CallbackContext obj)
        {
            StateMachine.ChangeState(StateMachine.DashingState);
        }

        // ����һЩ�����Խ��еĲ������� �ڵ��ϵ�����״̬�Ŀ�����Ծ
        // ���ǿ�����дһ����Ծ�����Ļ����ί�к���
        // ֮����ݼ̳л���״̬�Ĳ�ͬ ��д���ί�к���
        protected virtual void JumpStartedReaction(InputAction.CallbackContext obj)
        {
            StateMachine.ReusableData.JumpingFoece = jumpingData.StationaryJumpingFoece;        
            StateMachine.ChangeState(StateMachine.JumpingState);
        }
        #endregion
    }
}
