using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public class PlayerLightLandingState : PlayerLandingState
    {
        public PlayerLightLandingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {

        }

        #region IState Methods
        public override void Enter()
        {
            playerMovementStateMachine.playerStateReusableData.movementSpeedModifier = 0f;

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

            //如果此时没有移动输入
            if(playerMovementStateMachine.playerStateReusableData.movementInput == Vector2.zero)
            {
                return;
            }

            OnMove();//对于其他状态的过渡
        }

        public override void OnAnimationTransitionEvent()
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.idlingState);
        }
        #endregion


    }
}
