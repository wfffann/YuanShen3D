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
        protected float speedModifier = 1f;//速度修改器

        public PlayerMovementState(PlayerMovementStateMachine _playerMovementStateMachine)
        {
            playerMovementStateMachine = _playerMovementStateMachine;
        }

        #region IState 接口的实现方法
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

        #region 主要方法
        /// <summary>
        /// 读取移动输入
        /// </summary>
        private void ReadMovementInput()
        {
            movementInput = playerMovementStateMachine.player.input.playerActions.Movement.ReadValue<Vector2>();   
        }

        /// <summary>
        /// 移动
        /// </summary>
        private void Move()
        {
            if(movementInput == Vector2.zero || speedModifier == 0f)
            {
                return;
            }
            
            //获取输入向量(已归一化
            Vector3 movementDirction = GetMovementInputDirection();

            //获取速度
            float movementSpeed = GetMovementSpeed();

            //获取当前刚体的水平速度
            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

            //改变速度(如果这个速度有差值，那么增加力去补偿这个速度差值（相当于接近TargetSpeed（没有差值则不给予力
            playerMovementStateMachine.player.rb.AddForce(movementSpeed * movementDirction - currentPlayerHorizontalVelocity,
                ForceMode.VelocityChange);
        }
        #endregion

        #region 可重用方法
        /// <summary>
        /// 获取移动输入的向量
        /// </summary>
        /// <returns></returns>
        protected Vector3 GetMovementInputDirection()
        {
            return new Vector3(movementInput.x, 0f, movementInput.y);
        }

        /// <summary>
        /// 获取移动速度
        /// </summary>
        /// <returns></returns>
        protected float GetMovementSpeed()
        {
            return baseSpeed * speedModifier;
        }

        /// <summary>
        /// 获取Player的水平输入
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
