using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public class PlayerJumpState : PlayerAirborneState
    {
        public PlayerJumpState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {

        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.playerStateReusableData.movementSpeedModifier = 0f;

            Jump();
        }
        #endregion

        #region Main Methods
        private void Jump()
        {
            Vector3 jumpForce = playerMovementStateMachine.playerStateReusableData.currentJumpForce;

            Vector3 playerForward = playerMovementStateMachine.player.transform.forward;

            //这个力也会给 其他 轴
            jumpForce.x *= playerForward.x;
            jumpForce.z *= playerForward.z;

            //跳跃前重置速度
            ResetVelocity();

            playerMovementStateMachine.player.rb.AddForce(jumpForce, ForceMode.VelocityChange);
        }
        #endregion
    }
}
