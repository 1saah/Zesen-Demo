using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 状态机用abstract的原因是为了让它只能被继承，不能被实例化
/// 同时为状态机本身添加复用的函数
/// </summary>
public abstract class StateMachine
{
    protected IState currentState;

    public void ChangeState(IState state)
    {
        currentState?.Exit();

        currentState = state;

        currentState?.Enter();
    }

    public void Update()
    {
        currentState?.GetInput();

        currentState?.Update();
    }

    public void PhysicalUpdate()
    {
        currentState?.PhysicalUpdate();
    }

    public void OnAnimationEnterEvent()
    {
        currentState?.OnAnimationEnterEvent();
    }

    public void OnAnimationExitEvent()
    {
        currentState?.OnAnimationExitEvent();
    }

    public void OnAnimationTransactionEvent()
    {
        currentState?.OnAnimationTransactionEvent();
    }

    public void OnTriggerEnter(Collider collider)
    {
        currentState?.OnTriggerEnter(collider);
    }

    public void OnTriggerExit(Collider collider)
    {
        currentState?.OnTriggerExit(collider);
    }
}
