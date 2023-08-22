using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovement
{
    // 可以切换到 所有MovingState Dashing Idling
    // 这个状态中我们不能移动
    // 我们只能通过移动进入RuningState而不能进入WalkingState
    // 在初始的一段时间内我们不能通过移动按键切换到其它状态
    // 所以我们不能和LightLandingState一样在Update中进行状态转换 而是应该在按键时间的委托函数中启用
    public class HardLandingState : LandingState
    {
        public HardLandingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region Istate Methdos
        public override void Enter()
        {
            StateMachine.ReusableData.speedMultiplier = 0f;
            base.Enter();
            StartAnimation(StateMachine.Controller.animatorDataUtility.isHardLandingHash);

            ResetVelocity();

            StateMachine.Controller.Input.PlayerActions.Movement.Disable();
        }

        public override void Exit() { 
            base.Exit();
            StopAnimation(StateMachine.Controller.animatorDataUtility.isHardLandingHash);
            StateMachine.Controller.Input.PlayerActions.Movement.Enable();
        }

        public override void OnAnimationExitEvent()
        {
            StateMachine.Controller.Input.PlayerActions.Movement.Enable();
        }

        public override void OnAnimationTransactionEvent()
        {
            StateMachine.ChangeState(StateMachine.IdelingState);
        }

        public override void PhysicalUpdate()
        {
            base.PhysicalUpdate();

            if (IsHorizontalStopping())
            {
                return;
            }

            ResetVelocity();
        }
        #endregion

        #region Reusable Methods
        protected override void AddInputReaction()
        {
            base.AddInputReaction();
            StateMachine.Controller.Input.PlayerActions.Movement.started += MovementStartedReaction;
        }

        protected override void RemoveInputReaction()
        {
            base.RemoveInputReaction();
            StateMachine.Controller.Input.PlayerActions.Movement.started -= MovementStartedReaction;
        }

        protected override void OnMove()
        {
            if (StateMachine.ReusableData.isToggle)
            {
                return;
            }

            StateMachine.ChangeState(StateMachine.RunningState);
        }

        // 在HardLightingState中我们不能跳跃
        protected override void JumpStartedReaction(InputAction.CallbackContext obj)
        {
            
        }

        #endregion

        #region Input Methods
        protected virtual void MovementStartedReaction(InputAction.CallbackContext obj)
        {
            OnMove();
        }

        #endregion
    }
}
