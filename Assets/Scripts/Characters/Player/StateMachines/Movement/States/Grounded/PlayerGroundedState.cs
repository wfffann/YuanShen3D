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
        public override void Enter()
        {
            base.Enter();

            UpdateShouldSpringState();
        }

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
                playerMovementStateMachine.player.playerLayerData.groundLayer,
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

                //计算出悬浮的距离
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

        /// <summary>
        /// 使Player进入其他地面状态时，更新是否保持疾跑的状态
        /// </summary>
        private void UpdateShouldSpringState()
        {
            //是疾跑状态
            if (!playerMovementStateMachine.playerStateReusableData.shouldSprint)
            {
                return;
            }

            //没有移动输入
            if (playerMovementStateMachine.playerStateReusableData.movementInput != Vector2.zero)
            {
                return;
            }

            playerMovementStateMachine.playerStateReusableData.shouldSprint = false;
        }

        /// <summary>
        /// 在碰撞体内检测是否有物体
        /// </summary>
        /// <returns></returns>
        private bool IsThereGroundUnderneath()
        {
            //碰撞体
            BoxCollider groundCheckCollider = playerMovementStateMachine.player.colliderUtility.triggerColliderData.groundCheckCollider;

            //碰撞体中心点
            Vector3 groundColliderCenterInWorldSpace = groundCheckCollider.bounds.center;

            //返回碰撞体碰撞的物体
            Collider[] overlappedGroundColliders = Physics.OverlapBox(groundColliderCenterInWorldSpace,
                groundCheckCollider.bounds.center, 
                groundCheckCollider.transform.rotation, 
                playerMovementStateMachine.player.playerLayerData.groundLayer,
                QueryTriggerInteraction.Ignore);

            return overlappedGroundColliders.Length > 0;
        }
        #endregion

        #region  Reusable Methods
        /// <summary>
        /// 玩家在移动时（有移动向量输入(进入其他的移动状态
        /// </summary>
        protected virtual void OnMove()
        {
            //如果是疾跑状态
            if (playerMovementStateMachine.playerStateReusableData.shouldSprint)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.sprintingState);

                return;
            }

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

        //添加按键的回调
        protected override void AddInputActionsCallback()
        {
            base.AddInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Movement.canceled += OnMovementCanceled;

            playerMovementStateMachine.player.input.playerActions.Dash.started += OnDashStarted;

            playerMovementStateMachine.player.input.playerActions.Jump.started += OnJumpStarted;
        }

        //移除按键的回调
        protected override void RemoveInputActionsCallback()
        {
            base.RemoveInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Movement.canceled -= OnMovementCanceled;

            playerMovementStateMachine.player.input.playerActions.Dash.started -= OnDashStarted;

            playerMovementStateMachine.player.input.playerActions.Jump.started -= OnJumpStarted;
        }

        /// <summary>
        /// 下方碰撞体离开准备下落状态
        /// </summary>
        /// <param name="collider"></param>
        protected override void OnContactWithGroundExited(Collider collider)
        {
            base.OnContactWithGroundExited(collider);

            //下方groundCheck碰撞体内是否有物体
            if (IsThereGroundUnderneath())
            {
                return;
            }

            Vector3 capsuleColliderCenterInWorldSpace = 
                playerMovementStateMachine.player.colliderUtility.capsulColliderData.collider.bounds.center;

            //胶囊碰撞体的底部位置向下发射出射线
            Ray downwardsRayFromCapsuleBottom = new Ray(capsuleColliderCenterInWorldSpace - 
                playerMovementStateMachine.player.colliderUtility.capsulColliderData.colliderVerticalExtents, 
                Vector3.down);

            //在短距离的射线检测下没有物体，那么执行下落的状态
            if(!Physics.Raycast(downwardsRayFromCapsuleBottom,
                out _, 
                playerGroundedMovementData.groundToFallRayDistance, 
                playerMovementStateMachine.player.playerLayerData.groundLayer, 
                QueryTriggerInteraction.Ignore))
            {
                //下落状态
                OnFall();
            }
        }

        /// <summary>
        /// 下落
        /// </summary>
        protected virtual void OnFall()
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerFallingState);
        }

        #endregion

        #region Input Methods
        /// <summary>
        /// 没有移动向量的输入
        /// </summary>
        /// <param name="context"></param>
        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.idlingState);
        }

        /// <summary>
        /// 触发一次冲刺
        /// </summary>
        /// <param name="context"></param>
        protected virtual void OnDashStarted(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.dashState);
        }

        /// <summary>
        /// 触发一次跳跃
        /// </summary>
        /// <param name="context"></param>
        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerJumpState);
        }
        #endregion
    }
}
