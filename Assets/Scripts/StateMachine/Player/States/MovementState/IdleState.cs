using GenshinImpactMovement;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : GroundedState
{
    protected IdleData idleData;

    public IdleState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        idleData = StateMachine.Controller.playerData_SO.PlayerGroundedData.PlayerIDleData;
    }

    // 如果我在Idle里面通过给Movement按键的Started委托添加转换到WalkState或者RunState状态的事件
    // 那么可能会出现Jump之前我按住W进入JumpState，Jump结束之后我依然没有松开W，但是回到了IdleState
    // 那么我不会触发Movement按键的Started委托进入WalkState或者RunState,及时我按住了W按键

    // 所以这种在按键按下和松开的委托添加事件的机制会影响那种持续性按键的输入比如移动，因此针对这种我们
    // 需要将输入单独写在Update()中来逐帧判断，而不是仅仅在开始或者结束的委托中判断。

    #region Istate Methods
    public override void Enter()
    {
        StateMachine.ReusableData.speedMultiplier = idleData.SpeedMultiplier;
        StateMachine.ReusableData.BackwardsRecenteringData = idleData.BackwardsRecenteringData;
        base.Enter();
        ResetVerticalVelocity();     
    }


    public override void Update()
    {
        base.Update();

        if(StateMachine.ReusableData.input == Vector2.zero)
        {
            return;
        }

        OnMove();
        //// 需要将一个方法中不同的逻辑部分归为它的子方法
        //if(isToggle)
        //{
        //    StateMachine.ChangeState(StateMachine.IdelingState);
        //}
        //else 
        //{
        //    StateMachine.ChangeState(StateMachine.RunningState);
        //}
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
    #endregion
}
