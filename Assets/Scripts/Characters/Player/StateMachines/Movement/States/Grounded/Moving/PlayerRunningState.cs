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

            //0.5s±£³Örunning×´Ì¬
            if(Time.time < startTime + playerSprintData.runToWalkTime)
            {
                return;
            }

            //Í£Ö¹±¼ÅÜÇÐ»»×´Ì¬
            StopRunning();
        }
        #endregion

        #region Main Methods
        /// <summary>
        /// Í£Ö¹±¼ÅÜ
        /// </summary>
        public void StopRunning()
        {
            //Õ¾Á¢×´Ì¬
            if(playerMovementStateMachine.playerStateReusableData.movementInput == Vector2.zero)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.idlingState);
            }

            //ÐÐ×ß×´Ì¬
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.walkingState);
        }
        #endregion

        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerMiddleStoppingState);
        }

        /// <summary>
        /// Running×´Ì¬µ½Walk×´Ì¬µÄ¹ý¶É
        /// </summary>
        /// <param name="context"></param>
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            //×ßÂ·¹ý¶É
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.walkingState);
        }
        #endregion
    }
}
