using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovement
{
    // ���״̬�����ǿ����ƶ� �ǵ���û�������ʱ���Զ�ת��Ŀ�귽��
    // ����̳г��״̬
    // ���ǿ���ת���� Waking Running Dashing Medium Stopping״̬
    public class RollingState : LandingState
    {
        protected RollingData rollingData;
        public RollingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            rollingData = groundedData.RollingData;
        }

        #region IState Methods
        public override void Enter()
        {
            StateMachine.ReusableData.speedMultiplier = rollingData.RollingSpeedModifier;
            base.Enter();
            StartAnimation(StateMachine.Controller.animatorDataUtility.isRollingHash);

            StateMachine.ReusableData.isSprinting = false;
        }

        public override void PhysicalUpdate()
        {
            base.PhysicalUpdate();

            if(StateMachine.ReusableData.input != Vector2.zero)
            {
                return;
            }

            // û�������������Զ�ת�򵽼�¼���� ��Ϊֻ���ƶ���ʱ�����ת���߼�
            SmoothDampRotate();
        }

        public override void OnAnimationTransactionEvent()
        {
            if (StateMachine.ReusableData.input == Vector2.zero)
            {
                StateMachine.ChangeState(StateMachine.MediumStoppingState);

                return;
            }

            OnMove();
        }
        

        public override void Exit()
        {
            base.Exit();
            StopAnimation(StateMachine.Controller.animatorDataUtility.isRollingHash);
        }
        #endregion

        #region Input Methods
        protected override void JumpStartedReaction(InputAction.CallbackContext obj)
        {

        }
        #endregion
    }
}
