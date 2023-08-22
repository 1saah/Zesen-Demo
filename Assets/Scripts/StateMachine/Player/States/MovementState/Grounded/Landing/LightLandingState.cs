using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    // 可以切换到 所有MovingState Jumping Dashing Idling
    // 这个状态中我们不能移动
    public class LightLandingState : LandingState
    {
        public LightLandingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {

        }

        #region IState Methods
        public override void Enter()
        {

            StateMachine.ReusableData.speedMultiplier = 0f;
            base.Enter();

            StateMachine.ReusableData.JumpingFoece = airborneData.JumpingData.StationaryJumpingFoece;

            ResetVelocity();
        }

        public override void Update()
        {
            base.Update();

            if(StateMachine.ReusableData.input == Vector2.zero)
            {
                return;
            }

            OnMove();
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

        public override void OnAnimationEnterEvent()
        {
            StateMachine.ChangeState(StateMachine.IdelingState);
        }
        #endregion
    }
}
