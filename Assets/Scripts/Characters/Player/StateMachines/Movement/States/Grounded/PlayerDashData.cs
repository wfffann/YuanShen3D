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
        [field: SerializeField][field: Range(0f, 2f)] public float timeToBeConsideredConsecutive { get; private set; } = 1f;//������̵ļ��ʱ��
        [field: SerializeField][field: Range(1, 10)] public int consecutiveDashesLimitAmount { get; private set; } = 2;//���ʱ�Ĵ�������
        [field: SerializeField][field: Range(0f, 5f)] public float dashLimitReachedCooldown { get; private set; } = 1.75f;//��̵���ȴʱ��
    }
}
