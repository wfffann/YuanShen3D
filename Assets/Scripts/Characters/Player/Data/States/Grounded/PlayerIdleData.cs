using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [Serializable]
    public class PlayerIdleData
    {
        [field: SerializeField] public List<PlayerCameraRecenteringData> backwardsCameraRecenteringData { get; private set; }
    }
}
