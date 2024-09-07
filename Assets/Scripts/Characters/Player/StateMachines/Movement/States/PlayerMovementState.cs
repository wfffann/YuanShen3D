using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YuanShenImpactMovementSystem
{
    public class PlayerMovementState : IState
    {
        protected PlayerMovementStateMachine playerMovementStateMachine;

        protected PlayerGroundedData playerGroundedMovementData;

        public PlayerMovementState(PlayerMovementStateMachine _playerMovementStateMachine)
        {
            playerMovementStateMachine = _playerMovementStateMachine;

            playerGroundedMovementData = playerMovementStateMachine.player.playerData.playerGroundedData;

            InitializaData();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitializaData()
        {
            //角度平滑方法调用需要的时间
            playerMovementStateMachine.playerStateReusableData.TimeToReachTargetRotation = playerGroundedMovementData.baseRotationData.targetRotationReachTime;
        }

        #region IState 接口的实现方法
        public virtual void Enter()
        {
            Debug.Log("State: " + GetType().Name);

            AddInputActionsCallback();
        }

        public virtual void Exit()
        {
            RemoveInputActionsCallback();
        }

        public virtual void HandleInput()
        {
            ReadMovementInput();
        }

        public virtual void PhysicsUpdate()
        {
            //移动
            Move();
        }

        public virtual void Update()
        {
            
        }

        public virtual void OnAnimationEnterEvent()
        {
            
        }

        public virtual void OnAnimationExitEvent()
        {
            
        }

        public virtual void OnAnimationTransitionEvent()
        {
            
        }

        #endregion

        #region Main Methods

        /// <summary>
        /// 读取移动输入
        /// </summary>
        private void ReadMovementInput()
        {
            playerMovementStateMachine.playerStateReusableData.movementInput = playerMovementStateMachine.player.input.playerActions.Movement.ReadValue<Vector2>();   
        }

        /// <summary>
        /// 移动
        /// </summary>
        private void Move()
        {
            if(playerMovementStateMachine.playerStateReusableData.movementInput == Vector2.zero || playerMovementStateMachine.playerStateReusableData.movementSpeedModifier == 0f)
            {
                return;
            }
            
            //获取输入向量(已归一化
            Vector3 movementDirction = GetMovementInputDirection();

            //人物旋转(只有在移动时才旋转
            float targetRotationYAngle = PlayerRotate(movementDirction);

            //获取旋转的单位向量
            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

            //获取速度
            float movementSpeed = GetMovementSpeed();

            //获取当前刚体的水平速度
            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

            //改变速度(如果这个速度有差值，那么增加力去补偿这个速度差值（相当于接近TargetSpeed（没有差值则不给予力
            playerMovementStateMachine.player.rb.AddForce(targetRotationDirection * movementSpeed  - currentPlayerHorizontalVelocity,
                ForceMode.VelocityChange);
        }

        /// <summary>
        /// 获取人物的旋转角度并旋转至Target角度
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public float PlayerRotate(Vector3 direction)
        {
            //获取目标旋转角度
            float directionAngle = UpdateTargetRotation(direction);

            //旋转至目标角度
            RotateTowardsTagetRotation();

            return directionAngle;
        }

        /// <summary>
        /// 返回移动输入向量的角度
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public float GetDirectionAngle(Vector3 direction)
        {
            //获取移动输入的角度（弧度转角度
            float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            //确保旋转时负角转换为正角（旋转不会总是以最短路径旋转
            if (directionAngle < 0f)
            {
                directionAngle += 360f;
            }

            return directionAngle;
        }

        /// <summary>
        /// 增加摄像机的旋转度数
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public float AddCameraRotationToAngle(float angle)
        {
            //增加相机的旋转度数
            angle += playerMovementStateMachine.player.mainCameraTransform.eulerAngles.y;

            //超过一圈了（相机的旋转度数是可以大于360°的，勾了Wrap
            if (angle > 360f)
            {
                angle -= 360f;
            }

            return angle;   
        }

        /// <summary>
        /// 更新Target目标旋转度数
        /// </summary>
        /// <param name="targetAngle"></param>
        private void UpdateTargetRotationData(float targetAngle)
        {
            playerMovementStateMachine.playerStateReusableData.CurrentTargetRotation.y = targetAngle;

            playerMovementStateMachine.playerStateReusableData.DampedTargetRotationPassedTime.y = 0f;
        }

        #endregion

        #region Reusable Methods

        /// <summary>
        /// 获取移动输入的向量
        /// </summary>
        /// <returns></returns>
        protected Vector3 GetMovementInputDirection()
        {
            return new Vector3(playerMovementStateMachine.playerStateReusableData.movementInput.x, 0f, playerMovementStateMachine.playerStateReusableData.movementInput.y);
        }

        /// <summary>
        /// 获取移动速度
        /// </summary>
        /// <returns></returns>
        protected float GetMovementSpeed()
        {
            return playerGroundedMovementData.BaseSpeed * 
                playerMovementStateMachine.playerStateReusableData.movementSpeedModifier * 
                playerMovementStateMachine.playerStateReusableData.movementOnSlopeSpeedModifier;
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

        /// <summary>
        /// 获取Player的垂直速度
        /// </summary>
        /// <returns></returns>
        protected Vector3 GetPlayerVerticalVelocity()
        {
            return new Vector3(0f, playerMovementStateMachine.player.rb.velocity.y, 0f);
        }

        /// <summary>
        /// 转向目标角度
        /// </summary>
        private void RotateTowardsTagetRotation()
        {
            //获取人物Y轴当前的角度
            float currentYAngle = playerMovementStateMachine.player.rb.rotation.eulerAngles.y;

            //判断当前度数与目标度数
            if(currentYAngle == playerMovementStateMachine.playerStateReusableData.CurrentTargetRotation.y)
            {
                return;
            }

            //平滑旋转
            float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, playerMovementStateMachine.playerStateReusableData.CurrentTargetRotation.y, 
                ref playerMovementStateMachine.playerStateReusableData.DampedTargetRotationCurrentVelocity.y,
                playerMovementStateMachine.playerStateReusableData.TimeToReachTargetRotation.y - playerMovementStateMachine.playerStateReusableData.DampedTargetRotationPassedTime.y);

            //不断减少调用平滑旋转的总时间
            playerMovementStateMachine.playerStateReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;

            //创建旋转四元数
            Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);

            //设置玩家旋转度数
            playerMovementStateMachine.player.rb.MoveRotation(targetRotation);
        }

        /// <summary>
        /// 获取目标角度并更新
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
        {
            //获取输入向量的角度
            float directionAngle = GetDirectionAngle(direction);

            if (shouldConsiderCameraRotation)
            {
                //增加摄像机的角度
                directionAngle = AddCameraRotationToAngle(directionAngle);
            }
            
            //判断上一帧目标旋转度数与当前一帧目标旋转度数是否相同
            if (directionAngle != playerMovementStateMachine.playerStateReusableData.CurrentTargetRotation.y)
            {
                //更新目标旋转度数
                UpdateTargetRotationData(directionAngle);
            }

            return directionAngle;
        }

        /// <summary>
        /// 获取方向角方向的单位向量
        /// </summary>
        /// <param name="targetAngle"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected Vector3 GetTargetRotationDirection(float targetAngle)
        {
            return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        /// <summary>
        /// 重置玩家的速度为 0
        /// </summary>
        protected void ResetVelocity()
        {
            playerMovementStateMachine.player.rb.velocity = Vector3.zero;
        }

        /// <summary>
        /// 添加按下一次的操作回调
        /// </summary>
        protected virtual void AddInputActionsCallback()
        {
            playerMovementStateMachine.player.input.playerActions.WalkToggle.started += OnWalkToggleStarted;
        }

        /// <summary>
        /// 删除按下一次的操作的回调
        /// </summary>
        protected virtual void RemoveInputActionsCallback()
        {
            playerMovementStateMachine.player.input.playerActions.WalkToggle.started -= OnWalkToggleStarted;
        }

        /// <summary>
        /// 水平方向的减速
        /// </summary>
        protected void DecelerateHorizontally()
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

            //与时间相关的ForceMode(v += F * dt
            playerMovementStateMachine.player.rb.AddForce(-playerHorizontalVelocity * 
                playerMovementStateMachine.playerStateReusableData.movementDecelerationForce, 
                ForceMode.Acceleration);
        }

        /// <summary>
        /// 判断是否在水平方向移动（具有0.1f的误差值
        /// </summary>
        /// <param name="minimumMagnitude"></param>
        /// <returns></returns>
        protected bool IsMovingHorizontally(float minimumMagnitude = 0.1f)
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

            Vector2 playerHorizontalMovement = new Vector2(playerHorizontalVelocity.x, playerHorizontalVelocity.y);

            return playerHorizontalMovement.magnitude > minimumMagnitude;
        }

        #endregion

        #region Input Methods
        /// <summary>
        /// Walk行走状态的切换（值对调
        /// </summary>
        /// <param name="context"></param>
        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.playerStateReusableData.shouldWalk = !playerMovementStateMachine.playerStateReusableData.shouldWalk;
        }



        #endregion
    }
}
