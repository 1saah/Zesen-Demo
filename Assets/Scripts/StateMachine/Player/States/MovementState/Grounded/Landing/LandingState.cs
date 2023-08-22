using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovement
{
    public class LandingState : GroundedState
    {
        public LandingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {

        }

        #region Istate Methods
        public override void Enter()
        {
            base.Enter();
            StartAnimation(StateMachine.Controller.animatorDataUtility.landingHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(StateMachine.Controller.animatorDataUtility.landingHash);
        }

        #endregion

    }
}
