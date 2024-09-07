using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YuanShenImpactMovementSystem
{
    public class PlayerDashingState : PlayerGroundedState
    {
        private PlayerDashData playerDashData;

        private float startTime;

        private int consecutiveDashesUsed;//连续按下次数

        private bool shouldKeepRotating;

        public PlayerDashingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            playerDashData = playerGroundedMovementData.playerDashData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            //速度修改器
            playerMovementStateMachine.playerStateReusableData.movementSpeedModifier = 
                playerGroundedMovementData.playerDashData.speedModifier;

            playerMovementStateMachine.playerStateReusableData.currentJumpForce = 
                playerAirborneData.playerJumpData.strongForce;

            playerMovementStateMachine.playerStateReusableData.RotationData = playerDashData.rotationData;

            AddForceOnTransitionFromStationaryState();

            //进入Dashing冲刺状态时是否有移动输入
            shouldKeepRotating = playerMovementStateMachine.playerStateReusableData.movementInput != Vector2.zero;
            
            //进入这个状态就会增加按下次数
            UpdateConsecutiveDashes();

            //记录第一次按下的时间
            startTime = Time.time;
        }

        public override void Exit()
        {
            base.Exit();

            SetBaseRotationData();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!shouldKeepRotating)
            {
                return;
            }

            //人物旋转
            RotateTowardsTagetRotation();
        }

        /// <summary>
        /// 冲刺状态的动画过渡(绑定在动画的关键帧执行
        /// </summary>
        public override void OnAnimationTransitionEvent()
        {
            //如果没有移动输入
            if(playerMovementStateMachine.playerStateReusableData.movementInput == Vector2.zero)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerHardStoppingState);

                return;
            }

            //有移动输入则进入疾跑状态
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.sprintingState);
        }
        #endregion

        #region Main Methods
        /// <summary>
        /// 冲刺状态增加的速度
        /// </summary>
        private void AddForceOnTransitionFromStationaryState()
        {
            //如果此时有移动的输入
            if(playerMovementStateMachine.playerStateReusableData.movementInput != Vector2.zero)
            {
                return;
            }

            Vector3 characterRotationDirection = playerMovementStateMachine.player.transform.forward;

            characterRotationDirection.y = 0f;

            //在旋转的时候冲刺，那么会在进入的时候（没有移动输入）旋转到其他正前方的朝向(不考虑相机的移动角度
            UpdateTargetRotation(characterRotationDirection, false);

            //冲刺时的移动速度
            playerMovementStateMachine.player.rb.velocity = characterRotationDirection * GetMovementSpeed();
        }

        /// <summary>
        /// 更新按下的冲刺次数
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void UpdateConsecutiveDashes()
        {
            //判断是否连续按下冲刺键
            if (!IsConsecutive())
            {
                consecutiveDashesUsed = 0;
            }

            ++consecutiveDashesUsed;

            //如果连续按下则清空次数
            if(consecutiveDashesUsed == playerDashData.consecutiveDashesLimitAmount)
            {
                consecutiveDashesUsed = 0;

                //执行禁用冲刺的延时协程
                playerMovementStateMachine.player.input.DishingActionFor(playerMovementStateMachine.player.input.playerActions.Dash, 
                    playerGroundedMovementData.playerDashData.dashLimitReachedCooldown);
            }
        }

        /// <summary>
        /// 判断是否连续按下冲刺键
        /// </summary>
        /// <returns></returns>
        private bool IsConsecutive()
        {
            //如果在一秒之内连续按下
            return Time.time < startTime + playerDashData.timeToBeConsideredConsecutive;
        }
        #endregion

        #region Reusable Methods
        protected override void AddInputActionsCallback()
        {
            base.AddInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Movement.performed += OnMovementPerformed;
        }

        protected override void RemoveInputActionsCallback()
        {
            base.RemoveInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Movement.performed -= OnMovementPerformed;
        }
        #endregion

        #region Input Methods
        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            shouldKeepRotating = true;
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            //此函数已被清空，目的是防止在冲刺状态下没有移动速度立马变为IdleState
            //此函数的作用被冲刺状态的动画帧Event同理替换
        }

        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
            //base.OnDashStarted(context);
        }
        #endregion
    }
}
