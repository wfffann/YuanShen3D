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
        /// �Ӵ���������з���
        /// </summary>
        /// <param name="collider"></param>
        protected override void OnContactWithGround(Collider collider)
        {
            float fallDistance = Mathf.Abs(playerPositionOnEnter.y - playerMovementStateMachine.player.transform.position.y);

            //����½
            if (fallDistance < playerFallData.minimumDistanceToBeConsideredHardFall)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerLightLandingState);

                return;
            }

            //����½
            if(playerMovementStateMachine.playerStateReusableData.shouldWalk && 
                !playerMovementStateMachine.playerStateReusableData.shouldSprint || 
                playerMovementStateMachine.playerStateReusableData.movementInput == Vector2.zero)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerHardLandingState);

                return;
            }

            //����
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerRollingState);
        }
        #endregion

        #region Main Methods
        /// <summary>
        /// ���ƴ�ֱ���ٶ�
        /// </summary>
        private void LimitVerticalVelocity()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

            if(playerVerticalVelocity.y >= -playerFallData.fallSpeedLimit)
            {
                return;
            }

            //��Ҫ���Ƶ��ٶȲ�ֵ
            Vector3 limitVelocity = new Vector3(0f, -playerFallData.fallSpeedLimit - playerVerticalVelocity.y, 0f);

            playerMovementStateMachine.player.rb.AddForce(limitVelocity, ForceMode.VelocityChange);
        }
        #endregion
    }
}
