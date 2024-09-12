using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YuanShenImpactMovementSystem
{
    public class PlayerHardLandingState : PlayerLandingState
    {
        public PlayerHardLandingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {

        }

        #region IState Methods
        public override void Enter()
        {
            playerMovementStateMachine.playerStateReusableData.movementSpeedModifier = 0f;

            base.Enter();

            //关闭移动按键输入
            playerMovementStateMachine.player.input.playerActions.Movement.Disable();

            ResetVelocity();
        }

        public override void Exit()
        {
            base.Exit();

            playerMovementStateMachine.player.input.playerActions.Movement.Enable();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!IsMovingHorizontally())
            {
                return;
            }

            //垂直移动则重置速度
            ResetVelocity();
        }

        public override void OnAnimationExitEvent()
        {
            playerMovementStateMachine.player.input.playerActions.Movement.Enable();
        }

        public override void OnAnimationTransitionEvent()
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.idlingState);
        }
        #endregion

        #region Reusable Methods
        protected override void AddInputActionsCallback()
        {
            base.AddInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Movement.started += OnMovementStarted;
        }

        protected override void RemoveInputActionsCallback()
        {
            base.RemoveInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Movement.started -= OnMovementStarted;
        }

        protected override void OnMove()
        {
            if (playerMovementStateMachine.playerStateReusableData.shouldWalk)
            {
                return;
            }

            playerMovementStateMachine.ChangeState(playerMovementStateMachine.runningState);
        }
        #endregion

        #region Input Methods
        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
            
        }

        private void OnMovementStarted(InputAction.CallbackContext context)
        {
            OnMove();
        }
        #endregion
    }
}
