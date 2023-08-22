using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovement
{
    // 这个状态中我们可以移动 记得在没有输入的时候自动转向到目标方向
    // 不会继承冲刺状态
    // 我们可以转换到 Waking Running Dashing Medium Stopping状态
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

            // 没有输入的情况下自动转向到记录方向 因为只有移动的时候才有转向逻辑
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
