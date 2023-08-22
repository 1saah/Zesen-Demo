using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovement
{
    public class StoppingState : GroundedState
    {
        protected StoppingData stoppingData;
        public StoppingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            stoppingData = groundedData.StoppingData;
        }

        #region Istate Methods
        // 首先进入这个状态我们要在开始停止移动
        public override void Enter()
        {
            ResetDefaultRecenteringData();
            StateMachine.ReusableData.speedMultiplier = 0f;
            base.Enter();
            StartAnimation(StateMachine.Controller.animatorDataUtility.stoppingHash);
        }
        // 之后我们需要开始减速到接近0的范围(PhysicsUpdate)
        // 相应的逻辑写在MovementState中 用于其它地方复用
        public override void PhysicalUpdate()
        {
            base.PhysicalUpdate();
            // 是否已经速度接近0
            if(IsHorizontalStopping())
            {
                return;
            }

            // 将人物转向到记录的角度
            SmoothDampRotate();

            // 如果没有那么开始减速逻辑
            HorizontalDecellerate();

        }

        // 我觉得这里错误的原因是我理解错了这个函数的意思
        // 它这里应该想表达的是动画结束修改的参数
        //public override void OnAnimationExitEvent()
        //{
        //    StateMachine.ChangeState(StateMachine.IdelingState);
        //}

        // 这里想表达的是切换动画的操作
        public override void OnAnimationTransactionEvent()
        {
            StateMachine.ChangeState(StateMachine.IdelingState);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(StateMachine.Controller.animatorDataUtility.stoppingHash);
        }
        #endregion

        #region Input Methods
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

        protected void MovementStartedReaction(InputAction.CallbackContext obj)
        {
            OnMove();
        }
        #endregion
    }
}
