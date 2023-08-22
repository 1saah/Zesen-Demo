using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovement
{
    // ���뷽ʽ: �� ���� ���� �����е���ո�
    // �뿪��ʽ:����������ok
    public class FlyingState : AirborneState
    {
        protected FlyingData flyingData;
        private Vector3 originalGravity;

        public FlyingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            flyingData = airborneData.FlyingData;
        }

        public override void Enter()
        {
            base.Enter();
            StartAnimation(StateMachine.Controller.animatorDataUtility.isFlyingHash);
            StateMachine.ReusableData.speedMultiplier = flyingData.FlySpeedModified;
            originalGravity = Physics.gravity;
            Physics.gravity = new Vector3 (0, -flyingData.GravityModified, 0);
            StateMachine.ReusableData.RotationData = flyingData.RotationData;
            StateMachine.ReusableData.TimeToReachTargetRotation.y = StateMachine.ReusableData.RotationData.TimeToReachTargetRotation.y;
        }

        public override void Exit() { 
            base.Exit();
            Physics.gravity = originalGravity;
            SetBaseRotaionData();
            StopAnimation(StateMachine.Controller.animatorDataUtility.isFlyingHash);
        }

        #region Input Methods

        protected override void AddInputReaction()
        {
            base.AddInputReaction();
            StateMachine.Controller.Input.PlayerActions.Jump.started += CancelFlying;
        }


        protected override void RemoveInputReaction()
        {
            base.RemoveInputReaction();
            StateMachine.Controller.Input.PlayerActions.Jump.started += CancelFlying;
        }
        private void CancelFlying(InputAction.CallbackContext context)
        {
            StateMachine.ChangeState(StateMachine.FallingState);
        }

        #endregion

        // ��Ĭ�ϵĽӴ������л�״̬��Airbrone��
    }
}
