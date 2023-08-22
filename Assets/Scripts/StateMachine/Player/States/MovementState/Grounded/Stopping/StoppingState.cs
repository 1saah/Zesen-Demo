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
        // ���Ƚ������״̬����Ҫ�ڿ�ʼֹͣ�ƶ�
        public override void Enter()
        {
            ResetDefaultRecenteringData();
            StateMachine.ReusableData.speedMultiplier = 0f;
            base.Enter();
            StartAnimation(StateMachine.Controller.animatorDataUtility.stoppingHash);
        }
        // ֮��������Ҫ��ʼ���ٵ��ӽ�0�ķ�Χ(PhysicsUpdate)
        // ��Ӧ���߼�д��MovementState�� ���������ط�����
        public override void PhysicalUpdate()
        {
            base.PhysicalUpdate();
            // �Ƿ��Ѿ��ٶȽӽ�0
            if(IsHorizontalStopping())
            {
                return;
            }

            // ������ת�򵽼�¼�ĽǶ�
            SmoothDampRotate();

            // ���û����ô��ʼ�����߼�
            HorizontalDecellerate();

        }

        // �Ҿ�����������ԭ�����������������������˼
        // ������Ӧ��������Ƕ��������޸ĵĲ���
        //public override void OnAnimationExitEvent()
        //{
        //    StateMachine.ChangeState(StateMachine.IdelingState);
        //}

        // ������������л������Ĳ���
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
