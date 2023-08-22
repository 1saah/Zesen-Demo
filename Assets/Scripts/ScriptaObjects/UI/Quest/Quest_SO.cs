using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    [CreateAssetMenu(fileName = "QuestData_SO", menuName = "Data/UI/QuestData_SO")]
    public class Quest_SO : ScriptableObject
    {
        public string questName;
        [field:TextArea]
        public string questDes;
        public List<QuestReward> rewardsList;
        public List<QuestRequest> requestsList;
    }
}
