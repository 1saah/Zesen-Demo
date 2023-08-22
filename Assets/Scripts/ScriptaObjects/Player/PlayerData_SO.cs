using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [CreateAssetMenu(fileName = "PlayerMovementData", menuName = "Data/Character/Player")]
    public class PlayerData_SO : ScriptableObject
    {
        // 这里的数据是不可以再游戏中修改的 仅仅可以在inspector中修改
        [field: SerializeField] public GroundedData PlayerGroundedData { get; private set; }
        [field: SerializeField] public AirborneData AirborneData { get; private set;}
    }
}
