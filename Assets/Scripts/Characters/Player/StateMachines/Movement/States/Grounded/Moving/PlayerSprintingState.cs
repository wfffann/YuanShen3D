using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YuanShenImpactMovementSystem
{
    public class PlayerSprintingState : PlayerMovingState
    {
        private PlayerSprintData playerSprintData;

        private float startTime;

        private bool keepSprinting;//�Ƿ񱣳ּ���

        public PlayerSprintingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            playerSprintData = playerGroundedMovementData.playerSprintData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            //�޸��ٶȵ�����
            playerMovementStateMachine.playerStateReusableData.movementSpeedModifier = playerSprintData.speedModifier;

            playerMovementStateMachine.playerStateReusableData.currentJumpForce = playerAirborneData.playerJumpData.strongForce;
           
            //��¼��ǰʱ��
            startTime = Time.time;
        }

        public override void Exit()
        {
            base.Exit();

            keepSprinting = false;
        }

        public override void Update()
        {
            base.Update();

            //��1s�ж�ǰһֱ����Ϊfalse
            if (keepSprinting)
            {
                return;
            }

            //1s��ʱ���ж��Ƿ�ı伲��״̬
            if(Time.time < startTime + playerSprintData.sprintToRunTime)
            {
                return;
            }

            //ֹͣ����״̬���ж�ʱ�����1sʱ
            StopSprinting();
        }
        #endregion

        #region
        /// <summary>
        /// ֹͣ����״̬
        /// </summary>
        private void StopSprinting()
        {
            if(playerMovementStateMachine.playerStateReusableData.movementInput == Vector2.zero)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.idlingState);

                return;
            }

            playerMovementStateMachine.ChangeState(playerMovementStateMachine.runningState);
        }
        #endregion

        #region Reusable Methods
        protected override void AddInputActionsCallback()
        {
            base.AddInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Sprint.performed += OnSprintPerformed;
        }

        protected override void RemoveInputActionsCallback()
        {
            base.RemoveInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Sprint.performed -= OnSprintPerformed;
        }
        #endregion

        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerHardStoppingState);
        }

        private void OnSprintPerformed(InputAction.CallbackContext context)
        {
            keepSprinting = true;
        }
        #endregion
    }
}
