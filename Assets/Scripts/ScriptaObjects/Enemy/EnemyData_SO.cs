using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [CreateAssetMenu (fileName = "EnemyData", menuName = "Data/Character/Enemy")]
    public class EnemyData_SO : ScriptableObject
    {
        [field: SerializeField] public EMovementData EMovementData { get; private set; }
    }
}
