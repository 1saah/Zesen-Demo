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

        // ��Ҫ��ӳ�̵��߼�
        AddDashForce();

        // ��ϵ����߼��ж�
        UpdateDashLimit();
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();

        // ����ڳ��ʱ���߳��;�е㰴�˷��� ��ô���ܾ���Ҫ�ڳ����ת��
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
        // �ж��Ƿ�Ϊ�������
        if (!IsConductiveDash())
        {
            dashCount += 1;
        }
        else
        {
            dashCount = 0;
        }

        // ��¼�˴γ�̵Ŀ�ʼʱ��
        startTime = Time.time;

        // ����Ѿ�������� ��ô���ð���Disable��ȴʱ��
        if(dashCount == dashData.DashConductiveCount)
        {
            // ��ȴ��������
            dashCount = 0;
            // ��̰������ݲ�����
            StateMachine.Controller.Input.DiaableAction(StateMachine.Controller.Input.PlayerActions.Dash, dashData.DashConductiveCoolDown);
        }
    }



    private void AddDashForce()
    {
        StateMachine.ReusableData.speedMultiplier = dashData.DashSpeedModifier;

        // ��ʱ���ǻ��ý�Ŀ����ת�������Ϊ�泯����
        StateMachine.ReusableData.RecordEulerRoration.y = GetEulerRotation(StateMachine.Controller.transform.forward);

        // ���û������ ��ô�����������泯������
        Vector3 playerDirection = StateMachine.Controller.transform.forward;

        if (StateMachine.ReusableData.input != Vector2.zero)
        {
            StateMachine.ReusableData.RecordEulerRoration.y = GetTartYEulerAngleBasedOnCamera();

            playerDirection = GetMovementDirection(StateMachine.ReusableData.RecordEulerRoration.y);
        }

        playerDirection.y = 0;

        // ����������ʾ����ʹ��AddForce������� TODO:Ϊʲô��
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

    // ʹ�ð����ж��ڽ���ʱ����;���Ƿ�㰴���ƶ�����
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

    // �����������
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
