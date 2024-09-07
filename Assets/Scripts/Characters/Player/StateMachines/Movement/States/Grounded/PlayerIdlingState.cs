using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        public PlayerIdlingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {

        }

        #region IState ����
        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.playerStateReusableData.movementSpeedModifier = 0f;

            playerMovementStateMachine.playerStateReusableData.currentJumpForce = playerAirborneData.playerJumpData.stationaryForce;

            ResetVelocity();
        }

        public override void Update()
        {
            base.Update();

            //���û���ƶ�����
            if(playerMovementStateMachine.playerStateReusableData.movementInput == Vector2.zero)
            {
                return;
            }

            OnMove();
        }
        #endregion
    }
}
