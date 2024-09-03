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

        protected Vector3 currentTargetRotation;//目标旋转角度
        protected Vector3 timeToReachTargetRotation;//角度平滑方法调用需要的时间
        protected Vector3 dampedTargetRotationCurrentVelocity;
        protected Vector3 dampedTargetRotationPassedTime;

        public PlayerMovementState(PlayerMovementStateMachine _playerMovementStateMachine)
        {
            playerMovementStateMachine = _playerMovementStateMachine;

            InitializaData();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitializaData()
        {
            //角度平滑方法调用需要的时间
            timeToReachTargetRotation.y = 0.14f;
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
            currentTargetRotation.y = targetAngle;

            dampedTargetRotationPassedTime.y = 0f;
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

        /// <summary>
        /// 转向目标角度
        /// </summary>
        private void RotateTowardsTagetRotation()
        {
            //获取人物Y轴当前的角度
            float currentYAngle = playerMovementStateMachine.player.rb.rotation.eulerAngles.y;

            //判断当前度数与目标度数
            if(currentYAngle == currentTargetRotation.y)
            {
                return;
            }

            //平滑旋转
            float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, currentTargetRotation.y, 
                ref dampedTargetRotationCurrentVelocity.y,
                timeToReachTargetRotation.y - dampedTargetRotationPassedTime.y);

            //不断减少调用平滑旋转的总时间
            dampedTargetRotationPassedTime.y += Time.deltaTime;

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
            if (directionAngle != currentTargetRotation.y)
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
        #endregion
    }
}
