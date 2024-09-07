using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YuanShenImpactMovementSystem
{
    public class PlayerRunningState : PlayerMovingState
    {
        private PlayerSprintData playerSprintData;

        private float startTime;

        public PlayerRunningState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            playerSprintData = playerGroundedMovementData.playerSprintData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.playerStateReusableData.movementSpeedModifier = playerGroundedMovementData.playerRunningData.speedModifier;

            startTime = Time.time;
        }

        public override void Update()
        {
            base.Update();

            if (!playerMovementStateMachine.playerStateReusableData.shouldWalk)
            {
                return;
            }

            //0.5s����running״̬
            if(Time.time < startTime + playerSprintData.runToWalkTime)
            {
                return;
            }

            //ֹͣ�����л�״̬
            StopRunning();
        }
        #endregion

        #region Main Methods
        /// <summary>
        /// ֹͣ����
        /// </summary>
        public void StopRunning()
        {
            //վ��״̬
            if(playerMovementStateMachine.playerStateReusableData.movementInput == Vector2.zero)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.idlingState);
            }

            //����״̬
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.walkingState);
        }
        #endregion

        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerMiddleStoppingState);
        }

        /// <summary>
        /// Running״̬��Walk״̬�Ĺ���
        /// </summary>
        /// <param name="context"></param>
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            //��·����
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.walkingState);
        }
        #endregion
    }
}
