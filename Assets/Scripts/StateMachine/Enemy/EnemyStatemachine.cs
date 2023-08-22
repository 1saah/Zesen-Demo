using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    public class EnemyStatemachine : StateMachine
    {
        // ��Ϊ���ﲢ������ʱ����Ҫ����״̬�� �������ǿ������л���ʱ���ڴ��� ���û���״̬
        // Idle
        public EnemyController controller;
        public EnemyReusableData reusableData;

        public EnemyStatemachine(EnemyController enemyController)
        {
            controller = enemyController;
            reusableData = new EnemyReusableData();
        }
    }
}
