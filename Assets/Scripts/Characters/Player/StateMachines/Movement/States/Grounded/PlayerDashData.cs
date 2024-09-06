using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [Serializable]
    public class PlayerDashData
    {
        [field: SerializeField][field: Range(1f, 3f)] public float speedModifier { get; private set; } = 2f;
        [field: SerializeField][field: Range(0f, 2f)] public float timeToBeConsideredConsecutive { get; private set; } = 1f;//连续冲刺的检测时间
        [field: SerializeField][field: Range(1, 10)] public int consecutiveDashesLimitAmount { get; private set; } = 2;//冲刺时的次数限制
        [field: SerializeField][field: Range(0f, 5f)] public float dashLimitReachedCooldown { get; private set; } = 1.75f;//冲刺的冷却时间
    }
}
