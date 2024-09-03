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

        protected Vector3 currentTargetRotation;//Ŀ����ת�Ƕ�
        protected Vector3 timeToReachTargetRotation;//�Ƕ�ƽ������������Ҫ��ʱ��
        protected Vector3 dampedTargetRotationCurrentVelocity;
        protected Vector3 dampedTargetRotationPassedTime;

        public PlayerMovementState(PlayerMovementStateMachine _playerMovementStateMachine)
        {
            playerMovementStateMachine = _playerMovementStateMachine;

            InitializaData();
        }

        /// <summary>
        /// ��ʼ������
        /// </summary>
        private void InitializaData()
        {
            //�Ƕ�ƽ������������Ҫ��ʱ��
            timeToReachTargetRotation.y = 0.14f;
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

            //������ת(ֻ�����ƶ�ʱ����ת
            float targetRotationYAngle = PlayerRotate(movementDirction);

            //��ȡ��ת�ĵ�λ����
            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

            //��ȡ�ٶ�
            float movementSpeed = GetMovementSpeed();

            //��ȡ��ǰ�����ˮƽ�ٶ�
            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

            //�ı��ٶ�(�������ٶ��в�ֵ����ô������ȥ��������ٶȲ�ֵ���൱�ڽӽ�TargetSpeed��û�в�ֵ�򲻸�����
            playerMovementStateMachine.player.rb.AddForce(targetRotationDirection * movementSpeed  - currentPlayerHorizontalVelocity,
                ForceMode.VelocityChange);
        }

        /// <summary>
        /// ��ȡ�������ת�ǶȲ���ת��Target�Ƕ�
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public float PlayerRotate(Vector3 direction)
        {
            //��ȡĿ����ת�Ƕ�
            float directionAngle = UpdateTargetRotation(direction);

            //��ת��Ŀ��Ƕ�
            RotateTowardsTagetRotation();

            return directionAngle;
        }

        /// <summary>
        /// �����ƶ����������ĽǶ�
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public float GetDirectionAngle(Vector3 direction)
        {
            //��ȡ�ƶ�����ĽǶȣ�����ת�Ƕ�
            float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            //ȷ����תʱ����ת��Ϊ���ǣ���ת�������������·����ת
            if (directionAngle < 0f)
            {
                directionAngle += 360f;
            }

            return directionAngle;
        }

        /// <summary>
        /// �������������ת����
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public float AddCameraRotationToAngle(float angle)
        {
            //�����������ת����
            angle += playerMovementStateMachine.player.mainCameraTransform.eulerAngles.y;

            //����һȦ�ˣ��������ת�����ǿ��Դ���360��ģ�����Wrap
            if (angle > 360f)
            {
                angle -= 360f;
            }

            return angle;   
        }

        /// <summary>
        /// ����TargetĿ����ת����
        /// </summary>
        /// <param name="targetAngle"></param>
        private void UpdateTargetRotationData(float targetAngle)
        {
            currentTargetRotation.y = targetAngle;

            dampedTargetRotationPassedTime.y = 0f;
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

        /// <summary>
        /// ת��Ŀ��Ƕ�
        /// </summary>
        private void RotateTowardsTagetRotation()
        {
            //��ȡ����Y�ᵱǰ�ĽǶ�
            float currentYAngle = playerMovementStateMachine.player.rb.rotation.eulerAngles.y;

            //�жϵ�ǰ������Ŀ�����
            if(currentYAngle == currentTargetRotation.y)
            {
                return;
            }

            //ƽ����ת
            float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, currentTargetRotation.y, 
                ref dampedTargetRotationCurrentVelocity.y,
                timeToReachTargetRotation.y - dampedTargetRotationPassedTime.y);

            //���ϼ��ٵ���ƽ����ת����ʱ��
            dampedTargetRotationPassedTime.y += Time.deltaTime;

            //������ת��Ԫ��
            Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);

            //���������ת����
            playerMovementStateMachine.player.rb.MoveRotation(targetRotation);
        }

        /// <summary>
        /// ��ȡĿ��ǶȲ�����
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
        {
            //��ȡ���������ĽǶ�
            float directionAngle = GetDirectionAngle(direction);

            if (shouldConsiderCameraRotation)
            {
                //����������ĽǶ�
                directionAngle = AddCameraRotationToAngle(directionAngle);
            }
            
            //�ж���һ֡Ŀ����ת�����뵱ǰһ֡Ŀ����ת�����Ƿ���ͬ
            if (directionAngle != currentTargetRotation.y)
            {
                //����Ŀ����ת����
                UpdateTargetRotationData(directionAngle);
            }

            return directionAngle;
        }

        /// <summary>
        /// ��ȡ����Ƿ���ĵ�λ����
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
