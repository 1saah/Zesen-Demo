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
            // �ﵽ�������� ��ʼ����
            if (dis <= eMovementData.fightDis + eChasingData.StoppingDistance)
            {
                // ����û������ȴ
                if(enemyStatemachine.reusableData.enemyFightWaitingTimer <= 0f)
                {
                    enemyStatemachine.ChangeState(new EFightState(enemyStatemachine));
                }
            }
            else if(dis > eMovementData.chasingDis + eChasingData.StoppingDistance)// ���Ѿ��뷵��ԭ��
            {
                enemyStatemachine.ChangeState(new EBackState(enemyStatemachine));
            }// ����׷�� ������Ҿ���
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
