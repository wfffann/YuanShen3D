using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace YuanShenImpactMovementSystem
{
    public class PlayerWalkingState : PlayerMovingState
    {
        private PlayerWalkData walkData;
        public PlayerWalkingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            walkData = playerGroundedMovementData.playerWalkData;
        }

        #region IState Methods
        public override void Enter()
        {
            playerMovementStateMachine.playerStateReusableData.movementSpeedModifier = playerGroundedMovementData.playerWalkData.speedModifier;

            playerMovementStateMachine.playerStateReusableData.backwardsCameraRecenteringData = walkData.backwardsCameraRecenteringData;

            base.Enter();

            playerMovementStateMachine.playerStateReusableData.currentJumpForce = playerAirborneData.playerJumpData.weakForce;
        }

        public override void Exit()
        {
            base.Exit();

            SetBaseCameraRecenteringData();
        }
        #endregion

        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerLightStoppingState);

            base.OnMovementCanceled(context);
        }

        /// <summary>
        /// ����Walk״̬��Running״̬�Ĺ���
        /// </summary>
        /// <param name="context"></param>
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            //�ܲ����ɣ�
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.runningState);
        }
        #endregion
    }
}
