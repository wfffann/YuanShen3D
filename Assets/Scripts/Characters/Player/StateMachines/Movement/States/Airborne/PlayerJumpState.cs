using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public class PlayerJumpState : PlayerAirborneState
    {
        private PlayerJumpData playerJumpData;
        private bool shouldKeepRotating;
        private bool canStartFalling;

        public PlayerJumpState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            playerJumpData = playerAirborneData.playerJumpData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            //���������ƶ��ٶ�
            playerMovementStateMachine.playerStateReusableData.movementSpeedModifier = 0f;

            playerMovementStateMachine.playerStateReusableData.movementDecelerationForce = playerJumpData.decelerationForce;

            shouldKeepRotating = playerMovementStateMachine.playerStateReusableData.movementInput != Vector2.zero;

            //ֻ����Ծ�ٶ�
            Jump();
        }

        public override void Exit()
        {
            base.Exit();

            SetBaseRotationData();

            canStartFalling = false;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (shouldKeepRotating)
            {
                RotateTowardsTagetRotation();
            }

            if (IsMovingUp())
            {
                DecelerateVerticalVelocity();
            }
        }

        public override void Update()
        {
            base.Update();

            //����״̬(ǰ������
            if(!canStartFalling && IsMovingUp(0f))
            {
                canStartFalling = true;
            }

            //�ȴ��ٶ� < 0
            if(!canStartFalling || GetPlayerVerticalVelocity().y > 0)
            {
                return;
            }

            //Y���ٶ� < 0
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerFallingState);
        }
        #endregion

        #region Reusable Methods
        protected override void ResetSprintState()
        {
           
        }
        #endregion

        #region Main Methods
        /// <summary>
        /// ��Ծ��ӵ���
        /// </summary>
        private void Jump()
        {
            Vector3 jumpForce = playerMovementStateMachine.playerStateReusableData.currentJumpForce;

            Vector3 jumpDirection = playerMovementStateMachine.player.transform.forward;

            //����ڽ���Jump��Ծ״̬ǰ���ƶ�������ô�ͳ��������ʱ��������Ծ������ֱ����ǰ����Ծ
            if (shouldKeepRotating)
            {
                //��ȡ��ת֮�������
                jumpDirection = GetTargetRotationDirection(playerMovementStateMachine.playerStateReusableData.CurrentTargetRotation.y);
            }

            //�����Ҳ��� ���� ��
            jumpForce.x *= jumpDirection.x;
            jumpForce.z *= jumpDirection.z;

            //��ȡ���ĵ�
            Vector3 capsualColliderCenterInWorldSpace = playerMovementStateMachine.player.colliderUtility.capsulColliderData.collider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsualColliderCenterInWorldSpace, Vector3.down);

            if(Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, playerJumpData.jumpToGroundRayDistance,  
               playerMovementStateMachine.player.playerLayerData.groundLayer,
               QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

                //���������״̬
                if (IsMovingUp())
                {
                    float forceModifier = playerJumpData.jumpForceModifierOnSlopeUpwards.Evaluate(groundAngle);

                    jumpForce.x *= forceModifier;
                    jumpForce.z *= forceModifier;
                }

                //������½�״̬
                if (IsMovingDown())
                {
                    float forceModifier = playerJumpData.jumpForceModifierOnSlopeDownwards.Evaluate(groundAngle);

                    jumpForce.y *= forceModifier;
                }
            }

            //��Ծǰ�����ٶ�
            ResetVelocity();

            playerMovementStateMachine.player.rb.AddForce(jumpForce, ForceMode.VelocityChange);
        }
        #endregion
    }
}
