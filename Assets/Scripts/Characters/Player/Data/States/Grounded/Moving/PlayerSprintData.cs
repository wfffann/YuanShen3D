using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [Serializable]
    public class PlayerSprintData
    {
        [field: SerializeField][field: Range(1f, 3f)] public float speedModifier { get; private set; } = 1.7f;//�����ٶȵ�����
        [field: SerializeField][field: Range(0f, 5f)] public float sprintToRunTime { get; private set; } = 1f;//���ܵ����ܵ��ж�ʱ��
        [field: SerializeField][field: Range(0f, 2f)] public float runToWalkTime { get; private set; } = 0.5f;//���ܵ����ܵ��ж�ʱ��
    }
}
