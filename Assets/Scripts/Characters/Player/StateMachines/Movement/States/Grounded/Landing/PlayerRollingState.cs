using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YuanShenImpactMovementSystem
{
    public class PlayerRollingState : PlayerLandingState
    {
        private PlayerRollData playerRollData;
        public PlayerRollingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            playerRollData = playerGroundedMovementData.playerRollData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.playerStateReusableData.movementSpeedModifier = playerRollData.speedModifier;

            playerMovementStateMachine.playerStateReusableData.shouldSprint = false;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if(playerMovementStateMachine.playerStateReusableData.movementInput != Vector2.zero)
            {
                return;
            }

            RotateTowardsTagetRotation();
        }

        public override void OnAnimationTransitionEvent()
        {
            if(playerMovementStateMachine.playerStateReusableData.movementInput == Vector2.zero)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerMediumStoppingState);

                return;
            }

            OnMove();
        }
        #endregion

        #region Input Methods
        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
            base.OnJumpStarted(context);


        }
        #endregion
    }
}
