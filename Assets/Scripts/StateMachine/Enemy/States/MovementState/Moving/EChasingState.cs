using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GenshinImpactMovement
{
    public class EChasingState : EMovingState
    {
        protected EChasingData eChasingData;
        private bool isChasing;

        public EChasingState(EnemyStatemachine statemachine, Transform player) : base(statemachine, player)
        {
            eChasingData = eMovingData.EChasingData;
        }

        #region IstateMethods
        public override void Enter()
        {
            base.Enter();
            agent = enemyStatemachine.controller.navMeshAgent;
            agent.isStopped = false;
            agent.speed = eChasingData.ChasingSpeedMutifier;
            agent.destination = playerTrans.position;
            agent.stoppingDistance = eChasingData.StoppingDistance;
            enemyStatemachine.controller.animator.SetBool("isChasing", true);
        }

        public override void Update()
        {
            base.Update();
            // 达到攻击距离 开始攻击
            if (dis <= eMovementData.fightDis + eChasingData.StoppingDistance)
            {
                // 攻击没有在冷却
                if(enemyStatemachine.reusableData.enemyFightWaitingTimer <= 0f)
                {
                    enemyStatemachine.ChangeState(new EFightState(enemyStatemachine));
                }
            }
            else if(dis > eMovementData.chasingDis + eChasingData.StoppingDistance)// 拉脱距离返回原点
            {
                enemyStatemachine.ChangeState(new EBackState(enemyStatemachine));
            }// 继续追逐 更新玩家距离
            else
            {
                agent.destination = playerTrans.position;
            }

        }

        public override void Exit()
        { 
            base.Exit();
            agent.isStopped = true;
            enemyStatemachine.controller.animator.SetBool("isChasing", false);
        }
        #endregion

    }
}
