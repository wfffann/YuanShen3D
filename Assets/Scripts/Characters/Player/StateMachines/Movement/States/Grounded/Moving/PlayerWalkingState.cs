using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YuanShenImpactMovementSystem
{
    public class PlayerWalkingState : PlayerMovingState
    {
        public PlayerWalkingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.playerStateReusableData.movementSpeedModifier = playerGroundedMovementData.playerWalkData.speedModifier;
        }
        #endregion

        #region Input Methods
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
