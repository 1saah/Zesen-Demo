using GenshinImpactMovement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO:我觉得没有写没有按下冲刺建将isSprinting设置为false的逻辑

public class SprintState : MovingState
{
    private SprintData sprintData;
    private bool isSprinting;
    // 因为跳跃延续冲刺状态的原因 所以在Exit()的时候有两种状态：
    // 1. 跳跃结束之后保持冲刺状态
    // 2. 跳跃结束之后不保持冲刺状态
    // 所以这里我们需要一个标志位来判断我们是否需要保持冲刺状态
    private bool shouldResetSprint;
    private float sprintStartedTime;

    public SprintState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        sprintData = groundedData.SprintData;
    }

    #region IState Methods
    public override void Enter()
    {
        StateMachine.ReusableData.speedMultiplier = sprintData.SprintModifier;
        base.Enter();
        StartAnimation(StateMachine.Controller.animatorDataUtility.isSprintingHash);
        sprintStartedTime = Time.time;
        shouldResetSprint = true;
    }

    public override void Update()
    {
        base.Update();

        // 持续按住那么就奔跑
        if (isSprinting)
        {
            return;
        }

        // 为持续按住那么判断是否过了短暂奔跑时间
        if (sprintStartedTime + sprintData.SprintTime > Time.time)
        {
            return;
        }

        // 如果没有按下奔跑键 并且 过了短暂奔跑时间 那么就停止奔跑
        StopSprint();

    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(StateMachine.Controller.animatorDataUtility.isSprintingHash);
        // 如果是正常退出冲刺状态 那么需要在之后保持这个状态 所以重置它
        if (shouldResetSprint)
        {
            isSprinting = false;
            StateMachine.ReusableData.isSprinting = false;
        }
    }

    #endregion
    #region Main Methods
    private void StopSprint()
    {
        // 如果有移动输入那么返回WalkState
        if (StateMachine.ReusableData.input != Vector2.zero)
        {
            StateMachine.ChangeState(StateMachine.RunningState);
            return;
        }
        // 没有移动输入那么返回IdleState
        StateMachine.ChangeState(StateMachine.IdelingState);
    }
    #endregion

    #region Reusable Methods

    // 下落之后也需要保持冲刺
    protected override void Falling()
    {
        shouldResetSprint = false;
        base.Falling();
    }

    protected override void AddInputReaction()
    {
        base.AddInputReaction();
        StateMachine.Controller.Input.PlayerActions.Sprint.performed += SprintPerformedReaction;
        StateMachine.Controller.Input.PlayerActions.Sprint.canceled += SprintCanceledReaction;

    }

    protected override void RemoveInputReaction()
    {
        base.RemoveInputReaction();
        StateMachine.Controller.Input.PlayerActions.Sprint.performed -= SprintPerformedReaction;
        StateMachine.Controller.Input.PlayerActions.Sprint.canceled -= SprintCanceledReaction;
    }

    protected override void JumpStartedReaction(InputAction.CallbackContext obj)
    {
        StateMachine.ReusableData.JumpingFoece = jumpingData.HardJumpingFoece;

        // 进入跳跃状态的时候如果是持续按下了冲刺键 那么结束的时候才保持冲刺状态
        if (isSprinting)
        {
            StateMachine.ReusableData.isSprinting = true;
        }

        // 进入跳跃状态 那么保持冲刺本身的状态 不需要重置
        shouldResetSprint = false;

        StateMachine.ChangeState(StateMachine.JumpingState);
    }

    #endregion

    #region Input Methods
    private void SprintCanceledReaction(InputAction.CallbackContext obj)
    {
        isSprinting = false;
    }

    private void SprintPerformedReaction(InputAction.CallbackContext obj)
    {
        isSprinting = true;
        // spint状态进入跳跃 跳跃结束还是sprint状态
        // 不放在JumpStartedReaction中的原因是因为只有持续按下了才能让isSprinting为true
        // 如果想放在JumpStartedReaction的话，那么就判断isSprinting为true的时候才有下面的逻辑
        //StateMachine.ReusableData.isSprinting = true;
    }

    protected override void MovementCanceled(InputAction.CallbackContext obj)
    {
        StateMachine.ChangeState(StateMachine.HardStoppingState);

        base.MovementCanceled(obj);
    }
    #endregion
}
