using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GenshinImpactMovement
{
    public class EMovementState : IState
    {
        protected EMovementData eMovementData;
        protected EnemyStatemachine enemyStatemachine;
        protected NavMeshAgent agent;

        public EMovementState(EnemyStatemachine statemachine)
        {
            enemyStatemachine = statemachine;
            eMovementData = enemyStatemachine.controller.enemyData_SO.EMovementData;
            agent = enemyStatemachine.controller.navMeshAgent;
        }

        #region Istate Methods
        public virtual void Enter()
        {
            Debug.Log("Enter" + this.ToString());
        }

        public virtual void Exit()
        {
            
        }

        public virtual void GetInput()
        {
            
        }

        public virtual void OnAnimationEnterEvent()
        {
            
        }

        public virtual void OnAnimationExitEvent()
        {
            
        }

        public virtual void OnAnimationTransactionEvent()
        {
            
        }

        public virtual void OnTriggerEnter(Collider collider)
        {
            
        }

        public virtual void OnTriggerExit(Collider collider)
        {
            
        }

        public virtual void PhysicalUpdate()
        {
            
        }

        public virtual void Update()
        {
            if(enemyStatemachine.reusableData.enemyFightWaitingTimer > 0)
            {
                enemyStatemachine.reusableData.enemyFightWaitingTimer -= Time.deltaTime;
            }
        }
        #endregion
        #region Reusable Methods

        #endregion
    }
}
