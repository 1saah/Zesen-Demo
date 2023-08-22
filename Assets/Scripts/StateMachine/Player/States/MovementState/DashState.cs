using GenshinImpactMovement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashState : GroundedState
{
    protected DashData dashData;
    private float startTime;
    private float dashCount;
    private bool isAutomaticalRotation;

    public DashState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        dashData = groundedData.DashData;
    }

    #region Istate Methods
    public override void Enter()
    {
        base.Enter();
        StartAnimation(StateMachine.Controller.animatorDataUtility.isDashingHash);

        StateMachine.ReusableData.RotationData = dashData.DashRotationData;
        StateMachine.ReusableData.TimeToReachTargetRotation.y = StateMachine.ReusableData.RotationData.TimeToReachTargetRotation.y;

        // 需要添加冲刺的逻辑
        AddDashForce();

        // 联系冲刺逻辑判断
        UpdateDashLimit();
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();

        // 如果在冲刺时或者冲刺途中点按了方向 那么可能就需要在冲刺中转向
        if(!isAutomaticalRotation)
        {
            return;
        }

        SmoothDampRotate();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(StateMachine.Controller.animatorDataUtility.isDashingHash);
        SetBaseRotaionData();
    }

    #endregion

    #region Main Methods
    private void UpdateDashLimit()
    {
        // 判断是否为连续冲刺
        if (!IsConductiveDash())
        {
            dashCount += 1;
        }
        else
        {
            dashCount = 0;
        }

        // 记录此次冲刺的开始时间
        startTime = Time.time;

        // 如果已经连续冲刺 那么设置按键Disable冷却时间
        if(dashCount == dashData.DashConductiveCount)
        {
            // 冷却次数归零
            dashCount = 0;
            // 冲刺按键短暂不可用
            StateMachine.Controller.Input.DiaableAction(StateMachine.Controller.Input.PlayerActions.Dash, dashData.DashConductiveCoolDown);
        }
    }



    private void AddDashForce()
    {
        StateMachine.ReusableData.speedMultiplier = dashData.DashSpeedModifier;

        // 此时我们还得将目标旋转方向更新为面朝方向
        StateMachine.ReusableData.RecordEulerRoration.y = GetEulerRotation(StateMachine.Controller.transform.forward);

        // 如果没有输入 那么让人物沿着面朝方向冲刺
        Vector3 playerDirection = StateMachine.Controller.transform.forward;

        if (StateMachine.ReusableData.input != Vector2.zero)
        {
            StateMachine.ReusableData.RecordEulerRoration.y = GetTartYEulerAngleBasedOnCamera();

            playerDirection = GetMovementDirection(StateMachine.ReusableData.RecordEulerRoration.y);
        }

        playerDirection.y = 0;

        // 根据资料显示这里使用AddForce会出问题 TODO:为什么？
        StateMachine.Controller.Rigidbody.velocity = playerDirection * GetMovementSpeed();

    }
    #endregion

    #region Reusable Methods
    private bool IsConductiveDash()
    {
        if (Time.time > startTime + dashData.DashConductiveTime)
        {
            return false;
        }

        return true;
    }

    // 使用按键判断在进入时或者途中是否点按了移动按键
    protected override void AddInputReaction()
    {
        base.AddInputReaction();
        StateMachine.Controller.Input.PlayerActions.Movement.performed += InputPerformedReaction;
    }

    protected override void RemoveInputReaction()
    {
        base.RemoveInputReaction();
        StateMachine.Controller.Input.PlayerActions.Movement.performed -= InputPerformedReaction;
    }

    public override void OnAnimationTransactionEvent()
    {
        if(StateMachine.ReusableData.input == Vector2.zero)
        {
            StateMachine.ChangeState(StateMachine.HardStoppingState);
        }else
        {
            StateMachine.ChangeState(StateMachine.SprintingState);
        }
    }

    // 不能自身调用
    protected override void DashStartedReaction(InputAction.CallbackContext obj)
    {
        
    }

    protected override void JumpStartedReaction(InputAction.CallbackContext obj)
    {
        StateMachine.ReusableData.JumpingFoece = jumpingData.HardJumpingFoece;
        StateMachine.ChangeState(StateMachine.JumpingState);
    }
    #endregion

    #region Input Methods
    protected virtual void InputPerformedReaction(InputAction.CallbackContext obj)
    {
        isAutomaticalRotation = true;
    }
    #endregion

}
