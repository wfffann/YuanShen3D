using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [Serializable]
    public class PlayerJumpData
    {
        [field: SerializeField] public PlayerRotationData playerRotationData { get; private set; }
        [field: SerializeField][field: Range(0f, 5f)] public float jumpToGroundRayDistance { get; private set; } = 2f;
        [field: SerializeField] public AnimationCurve jumpForceModifierOnSlopeUpwards { get; private set; }
        [field: SerializeField] public AnimationCurve jumpForceModifierOnSlopeDownwards { get; private set; }
        [field: SerializeField] public Vector3 stationaryForce {  get; private set; }
        [field: SerializeField] public Vector3 weakForce {  get; private set; }
        [field: SerializeField] public Vector3 mediumForce {  get; private set; }
        [field: SerializeField] public Vector3 strongForce {  get; private set; }

        [field: SerializeField][field: Range(0f, 10f)] public float decelerationForce { get; private set; } = 1.5f;

    }
}
