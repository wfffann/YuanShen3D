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
        [field: SerializeField] [field: Range(0f, 5f)]  public float groundToFallRayDistance { get; private set; } = 0.5f;
        [field: SerializeField] public AnimationCurve slopeSpeedAngleCurve { get; private set; }
        [field: SerializeField] public PlayerRotationData baseRotationData { get; private set; }
        [field: SerializeField] public PlayerWalkData playerWalkData { get; private set; }
        [field: SerializeField] public PlayerRunningData playerRunningData { get; private set; }
        [field: SerializeField] public PlayerDashData playerDashData { get; private set; }
        [field: SerializeField] public PlayerSprintData playerSprintData { get; private set; }
        [field: SerializeField] public PlayerStoppingData playerStoppingData { get; private set; }
        [field: SerializeField] public PlayerRollData playerRollData { get; private set; }
    }
}
