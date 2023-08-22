using GenshinImpactMovement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO:�Ҿ���û��дû�а��³�̽���isSprinting����Ϊfalse���߼�

public class SprintState : MovingState
{
    private SprintData sprintData;
    private bool isSprinting;
    // ��Ϊ��Ծ�������״̬��ԭ�� ������Exit()��ʱ��������״̬��
    // 1. ��Ծ����֮�󱣳ֳ��״̬
    // 2. ��Ծ����֮�󲻱��ֳ��״̬
    // ��������������Ҫһ����־λ���ж������Ƿ���Ҫ���ֳ��״̬
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

        // ������ס��ô�ͱ���
        if (isSprinting)
        {
            return;
        }

        // Ϊ������ס��ô�ж��Ƿ���˶��ݱ���ʱ��
        if (sprintStartedTime + sprintData.SprintTime > Time.time)
        {
            return;
        }

        // ���û�а��±��ܼ� ���� ���˶��ݱ���ʱ�� ��ô��ֹͣ����
        StopSprint();

    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(StateMachine.Controller.animatorDataUtility.isSprintingHash);
        // ����������˳����״̬ ��ô��Ҫ��֮�󱣳����״̬ ����������
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
        // ������ƶ�������ô����WalkState
        if (StateMachine.ReusableData.input != Vector2.zero)
        {
            StateMachine.ChangeState(StateMachine.RunningState);
            return;
        }
        // û���ƶ�������ô����IdleState
        StateMachine.ChangeState(StateMachine.IdelingState);
    }
    #endregion

    #region Reusable Methods

    // ����֮��Ҳ��Ҫ���ֳ��
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

        // ������Ծ״̬��ʱ������ǳ��������˳�̼� ��ô������ʱ��ű��ֳ��״̬
        if (isSprinting)
        {
            StateMachine.ReusableData.isSprinting = true;
        }

        // ������Ծ״̬ ��ô���ֳ�̱����״̬ ����Ҫ����
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
        // spint״̬������Ծ ��Ծ��������sprint״̬
        // ������JumpStartedReaction�е�ԭ������Ϊֻ�г��������˲�����isSprintingΪtrue
        // ��������JumpStartedReaction�Ļ�����ô���ж�isSprintingΪtrue��ʱ�����������߼�
        //StateMachine.ReusableData.isSprinting = true;
    }

    protected override void MovementCanceled(InputAction.CallbackContext obj)
    {
        StateMachine.ChangeState(StateMachine.HardStoppingState);

        base.MovementCanceled(obj);
    }
    #endregion
}
