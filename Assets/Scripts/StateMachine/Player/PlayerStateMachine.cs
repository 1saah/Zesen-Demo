using GenshinImpactMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实现玩家状态机
/// Cashing vs. Instantial
/// set vs. private set
/// 补充一个重要的知识点，父类包括子类可以直接引用子类，最好子类在创建时也传递父类引用
/// 所以这里我们给状态机添加父类引用(PlayerStateMachine)
/// </summary>
public class PlayerStateMachine : StateMachine
{
    // 我做了一些修改 我觉得这个地方使用Istate更好一些 因为可以随时替换修改
    // 这里根据不同的状态机会有cashing 和 instantiatial的区别
    // 这里是选择的cashing
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
    public PlayerReusableData ReusableData { get; } // 变量的get和set只是他本身的限定，并不限定他的子属性
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
