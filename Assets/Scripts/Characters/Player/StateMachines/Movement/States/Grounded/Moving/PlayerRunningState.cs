using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YuanShenImpactMovementSystem
{
    public class PlayerRunningState : PlayerMovingState
    {
        public PlayerRunningState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {

        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.playerStateReusableData.movementSpeedModifier = playerGroundedMovementData.playerRunningData.speedModifier;
        }
        #endregion


        #region Input Methods
        /// <summary>
        /// Running״̬��Walk״̬�Ĺ���
        /// </summary>
        /// <param name="context"></param>
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            //��·����
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.walkingState);
        }
        #endregion
    }
}
