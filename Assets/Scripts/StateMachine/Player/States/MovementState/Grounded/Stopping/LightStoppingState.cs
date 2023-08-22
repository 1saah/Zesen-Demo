using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovement
{
    public class LightStoppingState : StoppingState
    {
        public LightStoppingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region Istate Methods
        public override void Enter()
        {
            StateMachine.ReusableData.speedDecelerateMultiplier = stoppingData.LightStoppingModifier;
            base.Enter();
        }
        #endregion

        #region Reusable Methods
        protected override void JumpStartedReaction(InputAction.CallbackContext obj)
        {
            StateMachine.ReusableData.JumpingFoece = jumpingData.LightJumpingFoece;
            StateMachine.ChangeState(StateMachine.JumpingState);
        }
        #endregion
    }


}
