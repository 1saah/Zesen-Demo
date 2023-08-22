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

        // ����Update ���ù�����ȴʱ�����
        public override void Update()
        {
            SmoothDampRotate();
        }

        public override void Exit()
        {
            base.Exit();
            enemyStatemachine.controller.animator.SetBool("isAttacking", false);
        }

        // �ڳ������������ʱ����� �л���׷��״̬
        public override void OnAnimationTransactionEvent()
        {
            enemyStatemachine.ChangeState(new EChasingState(enemyStatemachine, enemyStatemachine.controller.player.transform));
        }

        #endregion

        #region Reusable Methods
        protected void SmoothDampRotate()
        {
            // ��ȡ��ҵ����˵�����
            Vector3 directionToEnemy = enemyStatemachine.controller.player.transform.position - enemyStatemachine.controller.transform.position;

            // �������������˵���ת�Ƕ�
            Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);

            // Mathf�е�ר���ڽǶȵĲ�ֵ���� ������Lerp��ר������ֵͨ�Ĳ�ֵ���� ֮ǰ��������Camera��Zoom
            float frameEulerAngle = Mathf.SmoothDampAngle(enemyStatemachine.controller.transform.eulerAngles.y, targetRotation.eulerAngles.y, ref enemyStatemachine.reusableData.EulerVelocity.y, enemyStatemachine.reusableData.TimeToReachTargetRotation.y - enemyStatemachine.reusableData.TimerToReachTargetRotation.y); 
            enemyStatemachine.reusableData.TimerToReachTargetRotation.y += Time.deltaTime;

            // ��������ת�Ƕ�ת��ΪQuaternion
            Quaternion targetotation = Quaternion.Euler(0f, frameEulerAngle, 0f);

            enemyStatemachine.controller.rb.MoveRotation(targetotation);
        }
        #endregion

    }
}
