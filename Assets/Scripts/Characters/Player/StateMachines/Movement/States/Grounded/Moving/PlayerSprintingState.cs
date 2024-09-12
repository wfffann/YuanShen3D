using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YuanShenImpactMovementSystem
{
    public class PlayerSprintingState : PlayerMovingState
    {
        private PlayerSprintData playerSprintData;

        private float startTime;

        private bool keepSprinting;//是否保持疾跑
        private bool shouldResetSpringState;

        public PlayerSprintingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            playerSprintData = playerGroundedMovementData.playerSprintData;
        }

        #region IState Methods
        public override void Enter()
        {
            //修改速度调节器
            playerMovementStateMachine.playerStateReusableData.movementSpeedModifier = playerSprintData.speedModifier;

            base.Enter();

            playerMovementStateMachine.playerStateReusableData.currentJumpForce = playerAirborneData.playerJumpData.strongForce;

            shouldResetSpringState = true;
           
            //记录当前时间
            startTime = Time.time;
        }

        public override void Exit()
        {
            base.Exit();

            if (shouldResetSpringState)
            {
                keepSprinting = false;
                playerMovementStateMachine.playerStateReusableData.shouldSprint = false;
            }
        }

        public override void Update()
        {
            base.Update();

            //在1s判定前一直都是为false
            if (keepSprinting)
            {
                return;
            }

            //1s的时间判定是否改变疾跑状态
            if(Time.time < startTime + playerSprintData.sprintToRunTime)
            {
                return;
            }

            //停止疾跑状态（判定时间大于1s时
            StopSprinting();
        }
        #endregion

        #region Main Methods
        /// <summary>
        /// 停止疾跑状态
        /// </summary>
        private void StopSprinting()
        {
            if(playerMovementStateMachine.playerStateReusableData.movementInput == Vector2.zero)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.idlingState);

                return;
            }

            playerMovementStateMachine.ChangeState(playerMovementStateMachine.runningState);
        }
        #endregion

        #region Reusable Methods
        protected override void AddInputActionsCallback()
        {
            base.AddInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Sprint.performed += OnSprintPerformed;
        }

        protected override void RemoveInputActionsCallback()
        {
            base.RemoveInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Sprint.performed -= OnSprintPerformed;
        }

        protected override void OnFall()
        {
            shouldResetSpringState = false;
            base.OnFall();
        }
        #endregion

        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerHardStoppingState);

            base.OnMovementCanceled(context);
        }

        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
            shouldResetSpringState = false;

            base.OnJumpStarted(context);
        }

        private void OnSprintPerformed(InputAction.CallbackContext context)
        {
            keepSprinting = true;

            playerMovementStateMachine.playerStateReusableData.shouldSprint = true;
        }
        #endregion
    }
}
