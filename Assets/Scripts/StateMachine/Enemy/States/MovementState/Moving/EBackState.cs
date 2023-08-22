using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    public class EBackState : EMovingState
    {
        protected EBackData eBackData;
        public EBackState(EnemyStatemachine statemachine) : base(statemachine)
        {
            eBackData = eMovingData.EBackData;
        }

        #region IstateMethods
        public override void Enter()
        {
            base.Enter();
            agent = enemyStatemachine.controller.navMeshAgent;
            agent.isStopped = false;
            agent.speed = eBackData.BackSpeedMutifier;
            agent.stoppingDistance = eBackData.StoppingDistance;
            agent.destination = enemyStatemachine.reusableData.originalPos;
            enemyStatemachine.controller.animator.SetBool("isBacking", true);
        }

        public override void Update()
        {
            base.Update();
            float disToOriginalPos = Vector3.Distance(enemyStatemachine.controller.transform.position, enemyStatemachine.reusableData.originalPos);
            // 到达原点
            if (disToOriginalPos <= eBackData.StoppingDistance)
            {
                enemyStatemachine.ChangeState(new EIdleState(enemyStatemachine));
            }           
        }

        public override void Exit()
        {
            base.Exit();
            agent.isStopped = true;
            enemyStatemachine.controller.animator.SetBool("isBacking", false);
        }
        #endregion
    }
}
