using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [Serializable]
    public class PlayerTriggerColliderData
    {
        [field:SerializeField] public BoxCollider groundCheckCollider {  get; private set; }

        public Vector3 groundCheckColliderExtents { get; private set; }

        public void Initialize()
        {
            groundCheckColliderExtents = groundCheckCollider.bounds.extents;
        }
    }
}
