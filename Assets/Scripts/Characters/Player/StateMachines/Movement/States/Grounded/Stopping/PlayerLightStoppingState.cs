using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public class PlayerLightStoppingState : PlayerStoppingState
    {
        public PlayerLightStoppingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {

        }

        #region
        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.playerStateReusableData.movementDecelerationForce = playerGroundedMovementData.playerStoppingData.lightDecelerationForce;
        }
        #endregion
    }
}
