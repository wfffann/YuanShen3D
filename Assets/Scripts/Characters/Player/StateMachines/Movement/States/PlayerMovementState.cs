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
        /// ��ʼ������
        /// </summary>
        private void InitializaData()
        {
            //�Ƕ�ƽ������������Ҫ��ʱ��
            playerMovementStateMachine.playerStateReusableData.TimeToReachTargetRotation = playerGroundedMovementData.baseRotationData.targetRotationReachTime;
        }

        #region IState �ӿڵ�ʵ�ַ���
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
            //�ƶ�
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
        /// ��ȡ�ƶ�����
        /// </summary>
        private void ReadMovementInput()
        {
            playerMovementStateMachine.playerStateReusableData.movementInput = playerMovementStateMachine.player.input.playerActions.Movement.ReadValue<Vector2>();   
        }

        /// <summary>
        /// �ƶ�
        /// </summary>
        private void Move()
        {
            if(playerMovementStateMachine.playerStateReusableData.movementInput == Vector2.zero || playerMovementStateMachine.playerStateReusableData.movementSpeedModifier == 0f)
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
            playerMovementStateMachine.playerStateReusableData.CurrentTargetRotation.y = targetAngle;

            playerMovementStateMachine.playerStateReusableData.DampedTargetRotationPassedTime.y = 0f;
        }

        #endregion

        #region Reusable Methods

        /// <summary>
        /// ��ȡ�ƶ����������
        /// </summary>
        /// <returns></returns>
        protected Vector3 GetMovementInputDirection()
        {
            return new Vector3(playerMovementStateMachine.playerStateReusableData.movementInput.x, 0f, playerMovementStateMachine.playerStateReusableData.movementInput.y);
        }

        /// <summary>
        /// ��ȡ�ƶ��ٶ�
        /// </summary>
        /// <returns></returns>
        protected float GetMovementSpeed()
        {
            return playerGroundedMovementData.BaseSpeed * 
                playerMovementStateMachine.playerStateReusableData.movementSpeedModifier * 
                playerMovementStateMachine.playerStateReusableData.movementOnSlopeSpeedModifier;
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
        /// ��ȡPlayer�Ĵ�ֱ�ٶ�
        /// </summary>
        /// <returns></returns>
        protected Vector3 GetPlayerVerticalVelocity()
        {
            return new Vector3(0f, playerMovementStateMachine.player.rb.velocity.y, 0f);
        }

        /// <summary>
        /// ת��Ŀ��Ƕ�
        /// </summary>
        private void RotateTowardsTagetRotation()
        {
            //��ȡ����Y�ᵱǰ�ĽǶ�
            float currentYAngle = playerMovementStateMachine.player.rb.rotation.eulerAngles.y;

            //�жϵ�ǰ������Ŀ�����
            if(currentYAngle == playerMovementStateMachine.playerStateReusableData.CurrentTargetRotation.y)
            {
                return;
            }

            //ƽ����ת
            float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, playerMovementStateMachine.playerStateReusableData.CurrentTargetRotation.y, 
                ref playerMovementStateMachine.playerStateReusableData.DampedTargetRotationCurrentVelocity.y,
                playerMovementStateMachine.playerStateReusableData.TimeToReachTargetRotation.y - playerMovementStateMachine.playerStateReusableData.DampedTargetRotationPassedTime.y);

            //���ϼ��ٵ���ƽ����ת����ʱ��
            playerMovementStateMachine.playerStateReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;

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
            if (directionAngle != playerMovementStateMachine.playerStateReusableData.CurrentTargetRotation.y)
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

        /// <summary>
        /// ������ҵ��ٶ�Ϊ 0
        /// </summary>
        protected void ResetVelocity()
        {
            playerMovementStateMachine.player.rb.velocity = Vector3.zero;
        }

        /// <summary>
        /// ��Ӱ���һ�εĲ����ص�
        /// </summary>
        protected virtual void AddInputActionsCallback()
        {
            playerMovementStateMachine.player.input.playerActions.WalkToggle.started += OnWalkToggleStarted;
        }

        /// <summary>
        /// ɾ������һ�εĲ����Ļص�
        /// </summary>
        protected virtual void RemoveInputActionsCallback()
        {
            playerMovementStateMachine.player.input.playerActions.WalkToggle.started -= OnWalkToggleStarted;
        }

        /// <summary>
        /// ˮƽ����ļ���
        /// </summary>
        protected void DecelerateHorizontally()
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

            //��ʱ����ص�ForceMode(v += F * dt
            playerMovementStateMachine.player.rb.AddForce(-playerHorizontalVelocity * 
                playerMovementStateMachine.playerStateReusableData.movementDecelerationForce, 
                ForceMode.Acceleration);
        }

        /// <summary>
        /// �ж��Ƿ���ˮƽ�����ƶ�������0.1f�����ֵ
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
        /// Walk����״̬���л���ֵ�Ե�
        /// </summary>
        /// <param name="context"></param>
        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.playerStateReusableData.shouldWalk = !playerMovementStateMachine.playerStateReusableData.shouldWalk;
        }



        #endregion
    }
}
