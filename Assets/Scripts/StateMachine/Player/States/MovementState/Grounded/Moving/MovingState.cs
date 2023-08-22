using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    public class MovingState : GroundedState
    {
        public MovingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region Istate Methods
        public override void Enter()
        {
            base.Enter();
            StartAnimation(StateMachine.Controller.animatorDataUtility.movingHash);
        }

        public override void Exit() { 
            base.Exit();
            StopAnimation(StateMachine.Controller.animatorDataUtility.movingHash);
        }

        #endregion

    }
}
