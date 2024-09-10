using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [Serializable]
    public class PlayerFallData
    {
        [field: SerializeField][field: Range(1f, 15f)] public float fallSpeedLimit { get; private set; } = 15f;
        [field: SerializeField][field: Range(0f, 100f)] public float minimumDistanceToBeConsideredHardFall { get; private set; } = 3f;
    }
}
