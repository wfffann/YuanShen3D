using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public class PlayerMovementState : IState
    {
        protected PlayerMovementStateMachine playerMovementStateMachine;

        protected Vector2 movementInput;

        protected float baseSpeed = 5f;
        protected float speedModifier = 1f;//�ٶ��޸���

        public PlayerMovementState(PlayerMovementStateMachine _playerMovementStateMachine)
        {
            playerMovementStateMachine = _playerMovementStateMachine;
        }

        #region IState �ӿڵ�ʵ�ַ���
        public virtual void Enter()
        {
            Debug.Log("State: " + GetType().Name);
        }

        public virtual void Exit()
        {
            
        }

        public virtual void HandleInput()
        {
            ReadMovementInput();
        }

        public virtual void PhysicsUpdate()
        {
            Move();
        }

        public virtual void Update()
        {
            
        }
        #endregion

        #region ��Ҫ����
        /// <summary>
        /// ��ȡ�ƶ�����
        /// </summary>
        private void ReadMovementInput()
        {
            movementInput = playerMovementStateMachine.player.input.playerActions.Movement.ReadValue<Vector2>();   
        }

        /// <summary>
        /// �ƶ�
        /// </summary>
        private void Move()
        {
            if(movementInput == Vector2.zero || speedModifier == 0f)
            {
                return;
            }
            
            //��ȡ��������(�ѹ�һ��
            Vector3 movementDirction = GetMovementInputDirection();

            //��ȡ�ٶ�
            float movementSpeed = GetMovementSpeed();

            //��ȡ��ǰ�����ˮƽ�ٶ�
            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

            //�ı��ٶ�(�������ٶ��в�ֵ����ô������ȥ��������ٶȲ�ֵ���൱�ڽӽ�TargetSpeed��û�в�ֵ�򲻸�����
            playerMovementStateMachine.player.rb.AddForce(movementSpeed * movementDirction - currentPlayerHorizontalVelocity,
                ForceMode.VelocityChange);
        }
        #endregion

        #region �����÷���
        /// <summary>
        /// ��ȡ�ƶ����������
        /// </summary>
        /// <returns></returns>
        protected Vector3 GetMovementInputDirection()
        {
            return new Vector3(movementInput.x, 0f, movementInput.y);
        }

        /// <summary>
        /// ��ȡ�ƶ��ٶ�
        /// </summary>
        /// <returns></returns>
        protected float GetMovementSpeed()
        {
            return baseSpeed * speedModifier;
        }

        /// <summary>
        /// ��ȡPlayer��ˮƽ����
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected Vector3 GetPlayerHorizontalVelocity()
        {
            Vector3 playerHorizontalVelocity = playerMovementStateMachine.player.rb.velocity;

            playerHorizontalVelocity.y = 0f;

            return playerHorizontalVelocity;
        }
        #endregion
    }
}
