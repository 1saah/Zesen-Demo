using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    public class EIdleState : EMovementState
    {
        protected EIdleData eIdleData;
        public EIdleState(EnemyStatemachine statemachine) : base(statemachine)
        {
            eIdleData = eMovementData.EIdleData;
        }

        #region Istate Methods
        public override void Enter()
        {
            base.Enter();
            enemyStatemachine.controller.animator.SetBool("isIdling", true);
        }
        public override void Update()
        {
            base.Update();
            enemyStatemachine.reusableData.enemyIdleWaitingTimer += Time.deltaTime;
            // Idle×´Ì¬³¬¹ý3Ãë¿ªÊ¼Ô­µØÎ§ÈÆÔ²ÐÄÑ²Âß
            if(enemyStatemachine.reusableData.enemyIdleWaitingTimer > eIdleData.IdlePatrolWaittingTime)
            {
                enemyStatemachine.reusableData.enemyIdleWaitingTimer = 0f;
                enemyStatemachine.ChangeState(new EPatrolState(enemyStatemachine));
            }
        }

        public override void Exit()
        {
            base.Exit();
            enemyStatemachine.controller.animator.SetBool("isIdling", false);
        }

        // ¼ì²âµ½Íæ¼Ò
        public override void OnTriggerEnter(Collider collider)
        {
            base.OnTriggerEnter(collider);
            if(collider.gameObject.tag == "Player")
            {
                enemyStatemachine.controller.player = collider.gameObject;
                float dis = Vector3.Distance(collider.transform.position, enemyStatemachine.controller.transform.position);
                // ¾àÀë¹»ÁË¾ÍFight
                if (dis <= eMovementData.fightDis + eMovementData.stoppingDis)
                {
                    // ÅÐ¶Ï¹¥»÷ÊÇ·ñÔÚÀäÈ´ÖÐ
                    if(enemyStatemachine.reusableData.enemyFightWaitingTimer <= 0f) 
                    {                      
                        enemyStatemachine.ChangeState(new EFightState(enemyStatemachine));
                    }                  
                }
                else// ¾àÀë²»¹»¾ÍChase
                {
                    enemyStatemachine.ChangeState(new EChasingState(enemyStatemachine, collider.transform));
                }                                                
            }
        }
        #endregion
    }
}
