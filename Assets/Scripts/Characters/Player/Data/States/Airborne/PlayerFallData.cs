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
    }
}
