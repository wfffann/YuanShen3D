using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YuanShenImpactMovementSystem
{
    public class PlayerDashingState : PlayerGroundedState
    {
        private PlayerDashData playerDashData;

        private float startTime;

        private int consecutiveDashesUsed;//�������´���
        public PlayerDashingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            playerDashData = playerGroundedMovementData.playerDashData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            //�ٶ��޸���
            playerMovementStateMachine.playerStateReusableData.movementSpeedModifier = playerGroundedMovementData.playerDashData.speedModifier;

            AddForceOnTransitionFromStationaryState();
            
            //�������״̬�ͻ����Ӱ��´���
            UpdateConsecutiveDashes();

            //��¼��һ�ΰ��µ�ʱ��
            startTime = Time.time;
        }

        /// <summary>
        /// ���״̬�Ķ�������(���ڶ����Ĺؼ�ִ֡��
        /// </summary>
        public override void OnAnimationTransitionEvent()
        {
            base.OnAnimationTransitionEvent();

            //���û���ƶ�����
            if(playerMovementStateMachine.playerStateReusableData.movementInput == Vector2.zero)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.idlingState);

                return;
            }

            //���ƶ���������뼲��״̬
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.sprintingState);
        }
        #endregion

        #region Main Methods
        private void AddForceOnTransitionFromStationaryState()
        {
            //�����ʱ���ƶ�������
            if(playerMovementStateMachine.playerStateReusableData.movementInput != Vector2.zero)
            {
                return;
            }

            Vector3 characterRotationDirection = playerMovementStateMachine.player.transform.forward;

            characterRotationDirection.y = 0f;

            //��ȡ��ǰ���ƶ��ٶ�
            playerMovementStateMachine.player.rb.velocity = characterRotationDirection * GetMovementSpeed();
        }

        /// <summary>
        /// ���°��µĳ�̴���
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void UpdateConsecutiveDashes()
        {
            //�ж��Ƿ��������³�̼�
            if (!IsConsecutive())
            {
                consecutiveDashesUsed = 0;
            }

            ++consecutiveDashesUsed;

            //���������������մ���
            if(consecutiveDashesUsed == playerDashData.consecutiveDashesLimitAmount)
            {
                consecutiveDashesUsed = 0;

                //ִ�н��ó�̵���ʱЭ��
                playerMovementStateMachine.player.input.DishingActionFor(playerMovementStateMachine.player.input.playerActions.Dash, 
                    playerGroundedMovementData.playerDashData.dashLimitReachedCooldown);
            }
        }

        /// <summary>
        /// �ж��Ƿ��������³�̼�
        /// </summary>
        /// <returns></returns>
        private bool IsConsecutive()
        {
            //�����һ��֮����������
            return Time.time < startTime + playerDashData.timeToBeConsideredConsecutive;
        }
        #endregion

        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            //�˺����ѱ���գ�Ŀ���Ƿ�ֹ�ڳ��״̬��û���ƶ��ٶ������ΪIdleState
            //�˺��������ñ����״̬�Ķ���֡Eventͬ���滻
        }

        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
            base.OnDashStarted(context);
        }
        #endregion
    }
}
