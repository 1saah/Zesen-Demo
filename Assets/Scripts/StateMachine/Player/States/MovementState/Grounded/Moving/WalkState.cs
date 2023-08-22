using GenshinImpactMovement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WalkState : MovingState
{
    protected WalkData walkData;
    public WalkState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        walkData = StateMachine.Controller.playerData_SO.PlayerGroundedData.PlayerWalkData;
    }

    #region IState Methods
    public override void Enter()
    {
        StateMachine.ReusableData.BackwardsRecenteringData = walkData.BackwardsRecenteringData;
        StateMachine.ReusableData.speedMultiplier = walkData.SpeedMultiplier;
        base.Enter();
        StartAnimation(StateMachine.Controller.animatorDataUtility.isWalkingHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(StateMachine.Controller.animatorDataUtility.isWalkingHash);
        ResetDefaultRecenteringData();
    }
    #endregion

    #region Reusable Methods
    protected override void JumpStartedReaction(InputAction.CallbackContext obj)
    {
        StateMachine.ReusableData.JumpingFoece = jumpingData.LightJumpingFoece;
        StateMachine.ChangeState(StateMachine.JumpingState);
    }
    #endregion

    #region Input Methods
    protected override void ToggleReaction(InputAction.CallbackContext obj)
    {
        base.ToggleReaction(obj);
        StateMachine.ChangeState(StateMachine.RunningState);
    }

    protected override void MovementCanceled(InputAction.CallbackContext obj)
    {
        StateMachine.ChangeState(StateMachine.LightStoppingState);

        base.MovementCanceled(obj);
    }
    #endregion
}
