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

    // �������Idle����ͨ����Movement������Startedί�����ת����WalkState����RunState״̬���¼�
    // ��ô���ܻ����Jump֮ǰ�Ұ�סW����JumpState��Jump����֮������Ȼû���ɿ�W�����ǻص���IdleState
    // ��ô�Ҳ��ᴥ��Movement������Startedί�н���WalkState����RunState,��ʱ�Ұ�ס��W����

    // ���������ڰ������º��ɿ���ί������¼��Ļ��ƻ�Ӱ�����ֳ����԰�������������ƶ�����������������
    // ��Ҫ�����뵥��д��Update()������֡�жϣ������ǽ����ڿ�ʼ���߽�����ί�����жϡ�

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
        //// ��Ҫ��һ�������в�ͬ���߼����ֹ�Ϊ�����ӷ���
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
