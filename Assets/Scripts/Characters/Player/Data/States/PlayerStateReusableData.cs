using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [Serializable]
    public class PlayerStateReusableData
    {
        public PlayerRotationData RotationData { get; set; }
        public Vector2 movementInput { get; set; }
        public float movementSpeedModifier { get; set; } = 1f;
        public float movementOnSlopeSpeedModifier { get; set; } = 1f;
        public float movementDecelerationForce { get; set; } = 1f;

        public List<PlayerCameraRecenteringData> sidewaysCameraRecenteringData { get; set; }
        public List<PlayerCameraRecenteringData> backwardsCameraRecenteringData { get; set; }

        public bool shouldWalk {  get; set; }
        public bool shouldSprint {  get; set; }

        private Vector3 currentTargetRotation;//目标旋转角度
        private Vector3 timeToReachTargetRotation;//角度平滑方法调用需要的时间
        private Vector3 dampedTargetRotationCurrentVelocity;
        private Vector3 dampedTargetRotationPassedTime;

        public ref Vector3 CurrentTargetRotation
        {
            get
            {
                return ref currentTargetRotation;
            }
        }
        
        public ref Vector3 TimeToReachTargetRotation
        {
            get
            {
                return ref timeToReachTargetRotation;
            }
        }
        
        public ref Vector3 DampedTargetRotationCurrentVelocity
        {
            get
            {
                return ref dampedTargetRotationCurrentVelocity;
            }
        }
        
        public ref Vector3 DampedTargetRotationPassedTime
        {
            get
            {
                return ref dampedTargetRotationPassedTime;
            }
        }

        public Vector3 currentJumpForce {  get; set; }
    }
}
