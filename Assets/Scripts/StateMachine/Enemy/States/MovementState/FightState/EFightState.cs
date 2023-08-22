using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    public class EFightState : EMovementState
    {
        protected EFightData eFightData;
        public EFightState(EnemyStatemachine statemachine) : base(statemachine)
        {
            eFightData = eMovementData.EFightData;
        }
        #region IstateMethods
        public override void Enter()
        {
            base.Enter();
            enemyStatemachine.controller.animator.SetBool("isAttacking", true);
            enemyStatemachine.reusableData.enemyFightWaitingTimer = eFightData.fightInterval;
            enemyStatemachine.reusableData.TimerToReachTargetRotation.y = 0f;
        }

        // 空置Update 不让攻击冷却时间减少
        public override void Update()
        {
            SmoothDampRotate();
        }

        public override void Exit()
        {
            base.Exit();
            enemyStatemachine.controller.animator.SetBool("isAttacking", false);
        }

        // 在超出攻击距离的时候调用 切换回追逐状态
        public override void OnAnimationTransactionEvent()
        {
            enemyStatemachine.ChangeState(new EChasingState(enemyStatemachine, enemyStatemachine.controller.player.transform));
        }

        #endregion

        #region Reusable Methods
        protected void SmoothDampRotate()
        {
            // 获取玩家到敌人的向量
            Vector3 directionToEnemy = enemyStatemachine.controller.player.transform.position - enemyStatemachine.controller.transform.position;

            // 计算玩家面向敌人的旋转角度
            Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);

            // Mathf中的专用于角度的插值函数 区别于Lerp是专用于普通值的插值函数 之前我们用于Camera的Zoom
            float frameEulerAngle = Mathf.SmoothDampAngle(enemyStatemachine.controller.transform.eulerAngles.y, targetRotation.eulerAngles.y, ref enemyStatemachine.reusableData.EulerVelocity.y, enemyStatemachine.reusableData.TimeToReachTargetRotation.y - enemyStatemachine.reusableData.TimerToReachTargetRotation.y); 
            enemyStatemachine.reusableData.TimerToReachTargetRotation.y += Time.deltaTime;

            // 将最终旋转角度转化为Quaternion
            Quaternion targetotation = Quaternion.Euler(0f, frameEulerAngle, 0f);

            enemyStatemachine.controller.rb.MoveRotation(targetotation);
        }
        #endregion

    }
}
