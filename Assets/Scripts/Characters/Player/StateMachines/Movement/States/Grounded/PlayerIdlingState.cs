using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        private PlayerIdleData idleData;
        public PlayerIdlingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            idleData = playerGroundedMovementData.playerIdleData;
        }

        #region IState 方法
        public override void Enter()
        {
            playerMovementStateMachine.playerStateReusableData.movementSpeedModifier = 0f;

            playerMovementStateMachine.playerStateReusableData.backwardsCameraRecenteringData = idleData.backwardsCameraRecenteringData;

            base.Enter();

            playerMovementStateMachine.playerStateReusableData.currentJumpForce = playerAirborneData.playerJumpData.stationaryForce;

            ResetVelocity();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!IsMovingHorizontally())
            {
                return;
            }

            //垂直移动则重置速度
            ResetVelocity();
        }

        public override void Update()
        {
            base.Update();

            //如果没有移动输入
            if(playerMovementStateMachine.playerStateReusableData.movementInput == Vector2.zero)
            {
                return;
            }

            OnMove();
        }
        #endregion
    }
}
