using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public class PlayerJumpState : PlayerAirborneState
    {
        private PlayerJumpData playerJumpData;
        private bool shouldKeepRotating;
        private bool canStartFalling;

        public PlayerJumpState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            playerJumpData = playerAirborneData.playerJumpData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            //限制所有移动速度
            playerMovementStateMachine.playerStateReusableData.movementSpeedModifier = 0f;

            playerMovementStateMachine.playerStateReusableData.movementDecelerationForce = playerJumpData.decelerationForce;

            shouldKeepRotating = playerMovementStateMachine.playerStateReusableData.movementInput != Vector2.zero;

            //只有跳跃速度
            Jump();
        }

        public override void Exit()
        {
            base.Exit();

            SetBaseRotationData();

            canStartFalling = false;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (shouldKeepRotating)
            {
                RotateTowardsTagetRotation();
            }

            if (IsMovingUp())
            {
                DecelerateVerticalVelocity();
            }
        }

        public override void Update()
        {
            base.Update();

            //上升状态(前置条件
            if(!canStartFalling && IsMovingUp(0f))
            {
                canStartFalling = true;
            }

            //等待速度 < 0
            if(!canStartFalling || GetPlayerVerticalVelocity().y > 0)
            {
                return;
            }

            //Y轴速度 < 0
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerFallingState);
        }
        #endregion

        #region Reusable Methods
        protected override void ResetSprintState()
        {
           
        }
        #endregion

        #region Main Methods
        /// <summary>
        /// 跳跃添加的力
        /// </summary>
        private void Jump()
        {
            Vector3 jumpForce = playerMovementStateMachine.playerStateReusableData.currentJumpForce;

            Vector3 jumpDirection = playerMovementStateMachine.player.transform.forward;

            //如果在进入Jump跳跃状态前有移动输入那么就朝这个进入时的向量跳跃，否则直接向前面跳跃
            if (shouldKeepRotating)
            {
                //获取旋转之后的向量
                jumpDirection = GetTargetRotationDirection(playerMovementStateMachine.playerStateReusableData.CurrentTargetRotation.y);
            }

            //这个力也会给 其他 轴
            jumpForce.x *= jumpDirection.x;
            jumpForce.z *= jumpDirection.z;

            //获取中心点
            Vector3 capsualColliderCenterInWorldSpace = playerMovementStateMachine.player.colliderUtility.capsulColliderData.collider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsualColliderCenterInWorldSpace, Vector3.down);

            if(Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, playerJumpData.jumpToGroundRayDistance,  
               playerMovementStateMachine.player.playerLayerData.groundLayer,
               QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

                //如果是上升状态
                if (IsMovingUp())
                {
                    float forceModifier = playerJumpData.jumpForceModifierOnSlopeUpwards.Evaluate(groundAngle);

                    jumpForce.x *= forceModifier;
                    jumpForce.z *= forceModifier;
                }

                //如果是下降状态
                if (IsMovingDown())
                {
                    float forceModifier = playerJumpData.jumpForceModifierOnSlopeDownwards.Evaluate(groundAngle);

                    jumpForce.y *= forceModifier;
                }
            }

            //跳跃前重置速度
            ResetVelocity();

            playerMovementStateMachine.player.rb.AddForce(jumpForce, ForceMode.VelocityChange);
        }
        #endregion
    }
}
