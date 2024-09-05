using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [Serializable]
    public class DefaultColliderData
    {
        [field: SerializeField] public float height { get; private set; } = 1.8f;//碰撞体的长度
        [field: SerializeField] public float centerY { get; private set; } = 0.9f;//碰撞体高度的一半
        [field: SerializeField] public float radius { get; private set; } = 0.2f;//碰撞体的半径
    }
}
