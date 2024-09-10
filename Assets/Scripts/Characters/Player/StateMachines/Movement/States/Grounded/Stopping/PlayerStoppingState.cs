using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YuanShenImpactMovementSystem
{
    public class PlayerStoppingState : PlayerGroundedState
    {
        public PlayerStoppingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {

        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.playerStateReusableData.movementSpeedModifier = 0f;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            //ʹ����ת����
            RotateTowardsTagetRotation();

            //ˮƽ�ٶ��Ƿ����0.1f
            if (!IsMovingHorizontally())
            {
                return;
            }

            //��������
            DecelerateHorizontallyVelocity();
        }

        public override void OnAnimationTransitionEvent()
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.idlingState);
        }
        #endregion

        #region Reusable Methods
        protected override void AddInputActionsCallback()
        {
            base.AddInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Movement.started += OnMovementStarted;
        }

        protected override void RemoveInputActionsCallback()
        {
            base.RemoveInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Movement.started -= OnMovementStarted;
        }
        #endregion

        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            
        }

        /// <summary>
        /// ��stopping״̬�µ�״̬���ɵ�Movingϵ��״̬
        /// </summary>
        /// <param name="context"></param>
        private void OnMovementStarted(InputAction.CallbackContext context)
        {
            OnMove();
        }
        #endregion
    }
}
