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

        private bool shouldKeepRotating;

        public PlayerDashingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            playerDashData = playerGroundedMovementData.playerDashData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            //�ٶ��޸���
            playerMovementStateMachine.playerStateReusableData.movementSpeedModifier = 
                playerGroundedMovementData.playerDashData.speedModifier;

            playerMovementStateMachine.playerStateReusableData.currentJumpForce = 
                playerAirborneData.playerJumpData.strongForce;

            playerMovementStateMachine.playerStateReusableData.RotationData = playerDashData.rotationData;

            AddForceOnTransitionFromStationaryState();

            //����Dashing���״̬ʱ�Ƿ����ƶ�����
            shouldKeepRotating = playerMovementStateMachine.playerStateReusableData.movementInput != Vector2.zero;
            
            //�������״̬�ͻ����Ӱ��´���
            UpdateConsecutiveDashes();

            //��¼��һ�ΰ��µ�ʱ��
            startTime = Time.time;
        }

        public override void Exit()
        {
            base.Exit();

            SetBaseRotationData();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!shouldKeepRotating)
            {
                return;
            }

            //������ת
            RotateTowardsTagetRotation();
        }

        /// <summary>
        /// ���״̬�Ķ�������(���ڶ����Ĺؼ�ִ֡��
        /// </summary>
        public override void OnAnimationTransitionEvent()
        {
            //���û���ƶ�����
            if(playerMovementStateMachine.playerStateReusableData.movementInput == Vector2.zero)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerHardStoppingState);

                return;
            }

            //���ƶ���������뼲��״̬
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.sprintingState);
        }
        #endregion

        #region Main Methods
        /// <summary>
        /// ���״̬���ӵ��ٶ�
        /// </summary>
        private void AddForceOnTransitionFromStationaryState()
        {
            //�����ʱ���ƶ�������
            if(playerMovementStateMachine.playerStateReusableData.movementInput != Vector2.zero)
            {
                return;
            }

            Vector3 characterRotationDirection = playerMovementStateMachine.player.transform.forward;

            characterRotationDirection.y = 0f;

            //����ת��ʱ���̣���ô���ڽ����ʱ��û���ƶ����룩��ת��������ǰ���ĳ���(������������ƶ��Ƕ�
            UpdateTargetRotation(characterRotationDirection, false);

            //���ʱ���ƶ��ٶ�
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

        #region Reusable Methods
        protected override void AddInputActionsCallback()
        {
            base.AddInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Movement.performed += OnMovementPerformed;
        }

        protected override void RemoveInputActionsCallback()
        {
            base.RemoveInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Movement.performed -= OnMovementPerformed;
        }
        #endregion

        #region Input Methods
        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            shouldKeepRotating = true;
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            //�˺����ѱ���գ�Ŀ���Ƿ�ֹ�ڳ��״̬��û���ƶ��ٶ������ΪIdleState
            //�˺��������ñ����״̬�Ķ���֡Eventͬ���滻
        }

        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
            //base.OnDashStarted(context);
        }
        #endregion
    }
}
