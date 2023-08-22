using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void Enter();

    public void Exit();

    public void GetInput();

    public void Update();

    public void PhysicalUpdate();

    public void OnAnimationEnterEvent();
    public void OnAnimationExitEvent();
    public void OnAnimationTransactionEvent();

    public void OnTriggerEnter(Collider collider);

    public void OnTriggerExit(Collider collider);

}
