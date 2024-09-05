using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YuanShenImpactMovementSystem
{
    public class PlayerGroundedState : PlayerMovementState
    {
        public PlayerGroundedState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {

        }

        #region  Reusable Methods
        /// <summary>
        /// ������ƶ�ʱ�����ƶ���������
        /// </summary>
        protected virtual void OnMove()
        {
            //�������walk��
            if (playerMovementStateMachine.playerStateReusableData.shouldWalk)
            {
                //�л�Ϊ����״̬
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.walkingState);
                return;
            }

            //�����Ǳ���״̬
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.runningState);
        }

        protected override void AddInputActionsCallback()
        {
            base.AddInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Movement.canceled += OnMovementCanceled;
        }

        protected override void RemoveInputActionsCallback()
        {
            base.RemoveInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Movement.canceled -= OnMovementCanceled;
        }
        #endregion

        #region Input Methods
        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.idlingState);
        }
        #endregion
    }
}
