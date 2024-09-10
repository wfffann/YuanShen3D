using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public class PlayerFallingState : PlayerAirborneState
    {
        private PlayerFallData playerFallData;
        public PlayerFallingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            playerFallData = playerAirborneData.playerFallData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.playerStateReusableData.movementSpeedModifier = 0f;

            ResetVerticalVelocity();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            LimitVerticalVelocity();
        }
        #endregion

        #region Reusable Methods
        protected override void ResetSprintState()
        {
            base.ResetSprintState();

        }
        #endregion

        #region Main Methods
        /// <summary>
        /// 限制垂直的速度
        /// </summary>
        private void LimitVerticalVelocity()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

            if(playerVerticalVelocity.y >= -playerFallData.fallSpeedLimit)
            {
                return;
            }

            //需要限制的速度差值
            Vector3 limitVelocity = new Vector3(0f, -playerFallData.fallSpeedLimit - playerVerticalVelocity.y, 0f);

            playerMovementStateMachine.player.rb.AddForce(limitVelocity, ForceMode.VelocityChange);
        }
        #endregion
    }
}
