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
        [field: SerializeField] public Vector3 stationaryForce {  get; private set; }
        [field: SerializeField] public Vector3 weakForce {  get; private set; }
        [field: SerializeField] public Vector3 mediumForce {  get; private set; }
        [field: SerializeField] public Vector3 strongForce {  get; private set; }

    }
}
