using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [CreateAssetMenu(fileName = "PlayerMovementData", menuName = "Data/Character/Player")]
    public class PlayerData_SO : ScriptableObject
    {
        // ����������ǲ���������Ϸ���޸ĵ� ����������inspector���޸�
        [field: SerializeField] public GroundedData PlayerGroundedData { get; private set; }
        [field: SerializeField] public AirborneData AirborneData { get; private set;}
    }
}
