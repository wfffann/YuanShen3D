using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [Serializable]
    public class PlayerRunningData
    {
        [field: SerializeField][field: Range(1f, 2f)] public float speedModifier { get; private set; } = 1f;
    }
}
