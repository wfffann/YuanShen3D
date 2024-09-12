using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [Serializable]
    public class PlayerCapsuleColliderUtility : CapsulColliderUtility
    {
        [field:SerializeField] public PlayerTriggerColliderData triggerColliderData {  get; private set; }
        //[field:SerializeField] public PlayerTriggerColliderData triggerColliderData {  get; private set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            triggerColliderData.Initialize();
        }
    }
}
