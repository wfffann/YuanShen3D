using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YuanShenImpactMovementSystem
{
    public class PlayerGroundedState : PlayerMovementState
    {
        private SlopeData slopeData;

        public PlayerGroundedState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            slopeData = playerMovementStateMachine.player.colliderUtility.slopeData;
        }

        #region IState Methods
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            Float();
        }

        #endregion

        #region Main Methods
        /// <summary>
        /// 浮动胶囊体
        /// </summary>
        private void Float()
        {
            //返回碰撞体世界坐标的位置
            Vector3 capsuleColliderCenterInWorldSpace = 
                playerMovementStateMachine.player.colliderUtility.capsulColliderData.collider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

            //在胶囊碰撞体的Center向下放出射线
            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, slopeData.FloatRayDistance,
                playerMovementStateMachine.player.layerData.groundLayer,
                QueryTriggerInteraction.Ignore))//忽略是groundLayer层的Trigger对象
            {
                //计算法线与射线之间的角度
                float groundAngle = Vector3.Angle(hit.normal, - downwardsRayFromCapsuleCenter.direction);

                //获取当前坡度的速度修改器
                float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

                //这样可以使玩家不会悬浮在坡度很大的坡面
                if(slopeSpeedModifier == 0)
                {
                    return;
                }

                //获取碰撞体相对于世界坐标的center
                float distanceToFloatingPoint = 
                    playerMovementStateMachine.player.colliderUtility.capsulColliderData.colliderCenterInLocalSpace.y *
                    playerMovementStateMachine.player.transform.localScale.y -  //如果缩放玩家比例
                    hit.distance;

                if(distanceToFloatingPoint == 0f)
                {
                    return;
                }

                //计算Y轴的速度变化差值（都看成速度（因为AddForceMode仅考虑速度(给的力 - 重力的下坠速度
                float amountToLift = distanceToFloatingPoint * slopeData.stepReachForce - GetPlayerVerticalVelocity().y;

                //指定方向
                Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

                //添加浮动力
                playerMovementStateMachine.player.rb.AddForce(liftForce, ForceMode.VelocityChange);
            }
        }

        /// <summary>
        /// 根据曲线求取当前坡度的速度修改器
        /// </summary>
        /// <param name="groundAngle"></param>
        private float SetSlopeSpeedModifierOnAngle(float groundAngle)
        {
            float slopeSpeedModifier = playerGroundedMovementData.slopeSpeedAngleCurve.Evaluate(groundAngle);

            playerMovementStateMachine.playerStateReusableData.movementOnSlopeSpeedModifier = slopeSpeedModifier;

            return slopeSpeedModifier;
        }
        #endregion

        #region  Reusable Methods
        /// <summary>
        /// 玩家在移动时（有移动向量输入
        /// </summary>
        protected virtual void OnMove()
        {
            //如果按下walk键
            if (playerMovementStateMachine.playerStateReusableData.shouldWalk)
            {
                //切换为行走状态
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.walkingState);
                return;
            }

            //否则是奔跑状态
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.runningState);
        }

        protected override void AddInputActionsCallback()
        {
            base.AddInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Movement.canceled += OnMovementCanceled;
        }

        protected override void RemoveInputActionsCallback()
        {
            base.RemoveInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Movement.canceled -= OnMovementCanceled;
        }
        #endregion

        #region Input Methods
        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.idlingState);
        }
        #endregion
    }
}
