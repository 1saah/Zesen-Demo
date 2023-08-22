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
            // 从人物Capsule Collider发射的射线
            Ray rayFromCerterToGround = new Ray(StateMachine.Controller.PlayerFloatUtility.CapsuleColliderData.CapsuleColliderRef.bounds.center, Vector3.down);
            if (Physics.Raycast(rayFromCerterToGround, out RaycastHit hit, groundedData.DefaultVerticalRaycastDistance, groundedData.floatLayerMask, QueryTriggerInteraction.Ignore))
            {
                float playerVerticalSpeed = GetVerticalSpeed();
                float diatanceToFloatPoint = StateMachine.Controller.PlayerFloatUtility.CapsuleColliderData.ColliderCenter.y * StateMachine.Controller.transform.localScale.y - hit.distance;
                // 判断射线距离 是否大于碰撞体中心高度
                if (diatanceToFloatPoint == 0f)
                {
                    return;
                }

                // 会触发的时候只有陷入地面的时候
                // 此时将减去原本垂直方向速度的影响
                float amountToLift = diatanceToFloatPoint * groundedData.DefaultFloatVerticalForceMultiplier - playerVerticalSpeed;

                StateMachine.Controller.Rigidbody.AddForce(new Vector3(0f, amountToLift, 0f), ForceMode.VelocityChange);

                // 计算坡度和射线夹角
                float slopeAngle = Vector3.Angle(-rayFromCerterToGround.direction, hit.normal);
                // 根据坡度修改移动速度
                StateMachine.ReusableData.speedSlopeMultiplier = GetSlopeSpeedModifier(slopeAngle);

            }
        }

        #endregion


        #region ReUsable Methods
        protected override void ExamineLeavingGround()
        {
            // 排除裂缝和小洞骚扰
            if(TouchTriggerGround())
            {
                return;
            }

            // 检测下落距离是否足够
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

        // 用于保持Sprint状态，但是如果没有movement输入，那么不用保持
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

        // 对于一些都可以进行的操作比如 在地上的所有状态的可以跳跃
        // 我们可以在写一个跳跃按键的基层的委托函数
        // 之后根据继承基层状态的不同 重写这个委托函数
        protected virtual void JumpStartedReaction(InputAction.CallbackContext obj)
        {
            StateMachine.ReusableData.JumpingFoece = jumpingData.StationaryJumpingFoece;        
            StateMachine.ChangeState(StateMachine.JumpingState);
        }
        #endregion
    }
}
