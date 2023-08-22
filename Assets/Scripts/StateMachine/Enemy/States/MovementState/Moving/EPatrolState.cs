using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    public class EPatrolState : EMovementState
    {
        protected EPatrolData ePatrolData;
        protected Vector3 targetDes;

        public EPatrolState(EnemyStatemachine statemachine) : base(statemachine)
        {
            ePatrolData = eMovementData.EPatrolData;
        }

        #region IstateMethods
        public override void Enter()
        {
            SetNewAgentDestination();
            enemyStatemachine.controller.animator.SetBool("isPatrol", true);
        }

        // ���̳м�ʱ�� ����û��base.Update()
        public override void Update()
        {
            if (Vector3.Distance(enemyStatemachine.controller.transform.position, targetDes) <= ePatrolData.StoppingDistance)
            {
                enemyStatemachine.ChangeState(new EIdleState(enemyStatemachine));
            }
        }

        public override void Exit() 
        { 
            agent.isStopped = true;
            enemyStatemachine.controller.animator.SetBool("isPatrol", false);
        }
        #endregion

        #region Main Methods
        // ��⵽���
        public override void OnTriggerEnter(Collider collider)
        {
            base.OnTriggerEnter(collider);
            if (collider.gameObject.tag == "Player")
            {
                enemyStatemachine.controller.player = collider.gameObject;
                float dis = Vector3.Distance(collider.transform.position, enemyStatemachine.controller.transform.position);
                // ���빻�˾�Fight
                if (dis <= eMovementData.fightDis + eMovementData.stoppingDis)
                {
                    // �жϹ����Ƿ�����ȴ��
                    if (enemyStatemachine.reusableData.enemyFightWaitingTimer <= 0f)
                    {
                        enemyStatemachine.ChangeState(new EFightState(enemyStatemachine));
                    }
                }
                else// ���벻����Chase
                {
                    enemyStatemachine.ChangeState(new EChasingState(enemyStatemachine, collider.transform));
                }
            }
        }
        #endregion

        #region ReusableMethods
        protected void SetNewAgentDestination()
        {
            agent.isStopped = false;
            agent.speed = ePatrolData.PatrolSpeedMutifier;
            targetDes = GetRandomPatrolPosition();
            agent.destination = targetDes;
            agent.stoppingDistance = ePatrolData.StoppingDistance;
        }

        protected Vector3 GetRandomPatrolPosition()
        {
            float xBias = Random.Range(0f, ePatrolData.PatrolRadius);
            float zBias = Random.Range(0f, ePatrolData.PatrolRadius);
            Vector3 originalPos = enemyStatemachine.reusableData.originalPos;
            return new Vector3(originalPos.x + xBias, originalPos.y,originalPos.z + zBias);
        }
        #endregion
    }
}
