using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovement
{
    public class HardStoppingState : StoppingState
    {
        public HardStoppingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region Istate Methods
        public override void Enter()
        {
            StateMachine.ReusableData.speedDecelerateMultiplier = stoppingData.HardStoppingModifier;
            base.Enter();
            StartAnimation(StateMachine.Controller.animatorDataUtility.isHardStoppingHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(StateMachine.Controller.animatorDataUtility.isHardStoppingHash);
        }
        #endregion

        #region Reusable Methods
        protected override void JumpStartedReaction(InputAction.CallbackContext obj)
        {
            StateMachine.ReusableData.JumpingFoece = jumpingData.HardJumpingFoece;
            StateMachine.ChangeState(StateMachine.JumpingState);
        }
        #endregion

        #region Input Method
        protected override void OnMove()
        {
            if(StateMachine.ReusableData.isToggle)
            {
                return;
            }

            StateMachine.ChangeState(StateMachine.RunningState);

        }

        #endregion
    }
}
