using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [Serializable]
    public class PlayerGroundedData
    {
        [field: SerializeField] [field: Range(0f, 25f)]  public float BaseSpeed { get; private set; } = 5f;
        [field: SerializeField] public AnimationCurve slopeSpeedAngleCurve { get; private set; }
        [field: SerializeField] public PlayerRotationData baseRotationData { get; private set; }
        [field: SerializeField] public PlayerWalkData playerWalkData { get; private set; }
        [field: SerializeField] public PlayerRunningData playerRunningData { get; private set; }

        [field: SerializeField] public PlayerDashData playerDashData { get; private set; }
    }
}
