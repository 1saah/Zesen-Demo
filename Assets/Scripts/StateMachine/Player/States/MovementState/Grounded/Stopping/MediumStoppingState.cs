using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovement
{
    public class MediumStoppingState : StoppingState
    {
        public MediumStoppingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }
        #region Istate Methods
        public override void Enter()
        {
            StateMachine.ReusableData.speedDecelerateMultiplier = stoppingData.MediumStoppingModifier;
            base.Enter();
            StartAnimation(StateMachine.Controller.animatorDataUtility.isMediumStoppingHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(StateMachine.Controller.animatorDataUtility.isMediumStoppingHash);
        }
        #endregion

        #region Reusable Methods
        protected override void JumpStartedReaction(InputAction.CallbackContext obj)
        {
            StateMachine.ReusableData.JumpingFoece = jumpingData.MediumJumpingFoece;
            StateMachine.ChangeState(StateMachine.JumpingState);
        }
        #endregion
    }
}
