using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public class PlayerFallingState : PlayerAirborneState
    {
        private PlayerFallData playerFallData;

        private Vector3 playerPositionOnEnter;

        public PlayerFallingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            playerFallData = playerAirborneData.playerFallData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            playerPositionOnEnter = playerMovementStateMachine.player.transform.position;

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

        /// <summary>
        /// 接触到地面进行翻滚
        /// </summary>
        /// <param name="collider"></param>
        protected override void OnContactWithGround(Collider collider)
        {
            float fallDistance = Mathf.Abs(playerPositionOnEnter.y - playerMovementStateMachine.player.transform.position.y);

            //轻着陆
            if (fallDistance < playerFallData.minimumDistanceToBeConsideredHardFall)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerLightLandingState);

                return;
            }

            //重着陆
            if(playerMovementStateMachine.playerStateReusableData.shouldWalk && 
                !playerMovementStateMachine.playerStateReusableData.shouldSprint || 
                playerMovementStateMachine.playerStateReusableData.movementInput == Vector2.zero)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerHardLandingState);

                return;
            }

            //翻滚
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerRollingState);
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
