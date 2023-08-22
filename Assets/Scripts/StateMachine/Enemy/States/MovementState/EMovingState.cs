using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GenshinImpactMovement
{
    public class EMovingState : EMovementState
    {
        protected EMovingData eMovingData;
        protected Transform playerTrans;
        protected Vector3 targetPos;
        protected float dis;

        public EMovingState(EnemyStatemachine statemachine) : base(statemachine)
        {
            eMovingData = eMovementData.EMovingData;
        }

        public EMovingState(EnemyStatemachine statemachine, Transform player) : base(statemachine)
        {
            eMovingData = eMovementData.EMovingData;
            playerTrans = player;
            agent = enemyStatemachine.controller.navMeshAgent;
        }

        #region IstateMethods
        public override void Enter()
        {
            base.Enter();
            enemyStatemachine.controller.animator.SetBool("Moving", true);
        }
        public override void Update()
        {
            base.Update();
            if (playerTrans != null)
            {
                dis = Vector3.Distance(playerTrans.position, enemyStatemachine.controller.transform.position);
            }
        }

        public override void Exit()
        {
            base.Exit();
            enemyStatemachine.controller.animator.SetBool("Moving", false);
        }
        #endregion
    }
}
