using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [Serializable]
    public class DefaultColliderData
    {
        [field: SerializeField] public float height { get; private set; } = 1.8f;//��ײ��ĳ���
        [field: SerializeField] public float centerY { get; private set; } = 0.9f;//��ײ��߶ȵ�һ��
        [field: SerializeField] public float radius { get; private set; } = 0.2f;//��ײ��İ뾶
    }
}
