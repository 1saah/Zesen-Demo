using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    public class EnemyStatemachine : StateMachine
    {
        // 因为怪物并不是随时都需要更新状态的 所以我们可以在切换的时候在创建 不用缓存状态
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
