using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [Serializable]
    public class PlayerAirborneData
    {
        [field:SerializeField] public PlayerJumpData playerJumpData {  get; private set; } 
    }
}
