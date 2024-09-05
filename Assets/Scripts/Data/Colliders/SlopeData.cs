using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [Serializable]
    public class SlopeData
    {
        [field: SerializeField][field: Range(0f, 1f)] public float stepHeightPercentage { get; private set; } = 0.25f;
        [field: SerializeField][field: Range(0f, 5f)] public float FloatRayDistance { get; private set; } = 2f;
        [field: SerializeField][field: Range(0f, 50f)] public float stepReachForce { get; private set; } = 25f;
    }
}
