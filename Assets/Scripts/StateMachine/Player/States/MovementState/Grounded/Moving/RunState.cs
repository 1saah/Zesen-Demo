using GenshinImpactMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RunState : MovingState
{
    private RunData runData;
    private SprintData sprintData;
    private float startedRunTime;
    public RunState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        runData = groundedData.PlayerRunData;
        sprintData = groundedData.SprintData;
    }

    #region IState Methods
    public override void Enter()
    {
        StateMachine.ReusableData.speedMultiplier = runData.SpeedMultiplier;
        base.Enter();
        startedRunTime = Time.time;
    }

    public override void Update()
    {
        base.Update();

        // ������߼��������жϴ�ʱ�ı���״̬�Ƿ��ǳ��֮�������״̬��Toggle��trueΪ��·״̬��
        if(StateMachine.ReusableData.isToggle == false ) 
        {
            return;
        }

        if(startedRunTime + sprintData.RunToWalkTime > Time.time)
        {
            return;
        }

        StateMachine.ChangeState(StateMachine.WalkingState);
    }
    #endregion

    #region Reusable Methods
    protected override void JumpStartedReaction(InputAction.CallbackContext obj)
    {
        StateMachine.ReusableData.JumpingFoece = jumpingData.MediumJumpingFoece;
        StateMachine.ChangeState(StateMachine.JumpingState);
    }
    #endregion

    #region Input Methods
    protected override void ToggleReaction(InputAction.CallbackContext obj)
    {
        base.ToggleReaction(obj);
        StateMachine.ChangeState(StateMachine.WalkingState);
    }

    protected override void MovementCanceled(InputAction.CallbackContext obj)
    {
        StateMachine.ChangeState(StateMachine.MediumStoppingState);

        base.MovementCanceled(obj);
    }
    #endregion
}
