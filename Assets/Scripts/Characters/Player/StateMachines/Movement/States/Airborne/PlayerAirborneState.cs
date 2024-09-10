using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public class PlayerAirborneState : PlayerMovementState
    {
        public PlayerAirborneState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {

        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            ResetSprintState();
        }
        #endregion


        #region Reusable Methods
        /// <summary>
        /// 接触到地面后
        /// </summary>
        /// <param name="collider"></param>
        protected override void OnContactWithGround(Collider collider)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerLightLandingState);
        }


        protected virtual void ResetSprintState()
        {
            playerMovementStateMachine.playerStateReusableData.shouldSprint = false;
        }
        #endregion
    }
}
