using GenshinImpactMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ʵ�����״̬��
/// Cashing vs. Instantial
/// set vs. private set
/// ����һ����Ҫ��֪ʶ�㣬��������������ֱ���������࣬��������ڴ���ʱҲ���ݸ�������
/// �����������Ǹ�״̬����Ӹ�������(PlayerStateMachine)
/// </summary>
public class PlayerStateMachine : StateMachine
{
    // ������һЩ�޸� �Ҿ�������ط�ʹ��Istate����һЩ ��Ϊ������ʱ�滻�޸�
    // ������ݲ�ͬ��״̬������cashing �� instantiatial������
    // ������ѡ���cashing
    public IdleState IdelingState { get; }
    public WalkState WalkingState { get; }
    public RunState RunningState { get; }
    public SprintState SprintingState { get; }
    public DashState DashingState { get; }
    public LightStoppingState LightStoppingState { get; }
    public MediumStoppingState MediumStoppingState { get; }
    public HardStoppingState HardStoppingState { get; }
    public JumpingState JumpingState { get; }
    public FallingState FallingState { get; }
    public PlayerController Controller { get; }
    public PlayerReusableData ReusableData { get; } // ������get��setֻ����������޶��������޶�����������
    public LightLandingState LightLandingState { get; }
    public RollingState RollingState { get; }
    public FlyingState FlyingState { get; }
    public HardLandingState HardLandingState { get; }

    public PlayerStateMachine(GenshinImpactMovement.PlayerController playerController)
    {
        Controller = playerController;
        ReusableData = new PlayerReusableData();
        IdelingState = new IdleState(this);
        WalkingState = new WalkState(this);
        RunningState = new RunState(this);
        SprintingState = new SprintState(this);
        DashingState = new DashState(this);
        LightStoppingState = new LightStoppingState(this);
        MediumStoppingState = new MediumStoppingState(this);
        HardStoppingState = new HardStoppingState(this);
        JumpingState = new JumpingState(this);
        FallingState = new FallingState(this);
        LightLandingState = new LightLandingState(this);
        RollingState = new RollingState(this);
        HardLandingState = new HardLandingState(this);
        FlyingState = new FlyingState(this);    
    }
}
