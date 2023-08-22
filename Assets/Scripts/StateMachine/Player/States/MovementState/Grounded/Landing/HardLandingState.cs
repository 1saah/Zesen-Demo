using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovement
{
    // �����л��� ����MovingState Dashing Idling
    // ���״̬�����ǲ����ƶ�
    // ����ֻ��ͨ���ƶ�����RuningState�����ܽ���WalkingState
    // �ڳ�ʼ��һ��ʱ�������ǲ���ͨ���ƶ������л�������״̬
    // �������ǲ��ܺ�LightLandingStateһ����Update�н���״̬ת�� ����Ӧ���ڰ���ʱ���ί�к���������
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

        // ��HardLightingState�����ǲ�����Ծ
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
