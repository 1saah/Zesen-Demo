using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ״̬����abstract��ԭ����Ϊ������ֻ�ܱ��̳У����ܱ�ʵ����
/// ͬʱΪ״̬��������Ӹ��õĺ���
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
