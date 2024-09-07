using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public class PlayerHardStoppingState : PlayerStoppingState
    {
        public PlayerHardStoppingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {

        }

        #region
        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.playerStateReusableData.movementDecelerationForce = playerGroundedMovementData.playerStoppingData.hardDecelerationForce;
        }
        #endregion

        #region Resable Methods
        protected override void OnMove()
        {
            if (playerMovementStateMachine.playerStateReusableData.shouldWalk)
            {
                return;
            }

            playerMovementStateMachine.ChangeState(playerMovementStateMachine.runningState);
        }
        #endregion
    }
}
