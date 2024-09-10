using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YuanShenImpactMovementSystem
{
    public class PlayerGroundedState : PlayerMovementState
    {
        private SlopeData slopeData;

        public PlayerGroundedState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            slopeData = playerMovementStateMachine.player.colliderUtility.slopeData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            UpdateShouldSpringState();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            Float();
        }

        #endregion

        #region Main Methods
        /// <summary>
        /// ����������
        /// </summary>
        private void Float()
        {
            //������ײ�����������λ��
            Vector3 capsuleColliderCenterInWorldSpace = 
                playerMovementStateMachine.player.colliderUtility.capsulColliderData.collider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

            //�ڽ�����ײ���Center���·ų�����
            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, slopeData.FloatRayDistance,
                playerMovementStateMachine.player.playerLayerData.groundLayer,
                QueryTriggerInteraction.Ignore))//������groundLayer���Trigger����
            {
                //���㷨��������֮��ĽǶ�
                float groundAngle = Vector3.Angle(hit.normal, - downwardsRayFromCapsuleCenter.direction);

                //��ȡ��ǰ�¶ȵ��ٶ��޸���
                float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

                //��������ʹ��Ҳ����������¶Ⱥܴ������
                if(slopeSpeedModifier == 0)
                {
                    return;
                }

                //����������ľ���
                float distanceToFloatingPoint = 
                    playerMovementStateMachine.player.colliderUtility.capsulColliderData.colliderCenterInLocalSpace.y *
                    playerMovementStateMachine.player.transform.localScale.y -  //���������ұ���
                    hit.distance;

                if(distanceToFloatingPoint == 0f)
                {
                    return;
                }

                //����Y����ٶȱ仯��ֵ���������ٶȣ���ΪAddForceMode�������ٶ�(������ - ��������׹�ٶ�
                float amountToLift = distanceToFloatingPoint * slopeData.stepReachForce - GetPlayerVerticalVelocity().y;

                //ָ������
                Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

                //��Ӹ�����
                playerMovementStateMachine.player.rb.AddForce(liftForce, ForceMode.VelocityChange);
            }
        }

        /// <summary>
        /// ����������ȡ��ǰ�¶ȵ��ٶ��޸���
        /// </summary>
        /// <param name="groundAngle"></param>
        private float SetSlopeSpeedModifierOnAngle(float groundAngle)
        {
            float slopeSpeedModifier = playerGroundedMovementData.slopeSpeedAngleCurve.Evaluate(groundAngle);

            playerMovementStateMachine.playerStateReusableData.movementOnSlopeSpeedModifier = slopeSpeedModifier;

            return slopeSpeedModifier;
        }

        /// <summary>
        /// ʹPlayer������������״̬ʱ�������Ƿ񱣳ּ��ܵ�״̬
        /// </summary>
        private void UpdateShouldSpringState()
        {
            //�Ǽ���״̬
            if (!playerMovementStateMachine.playerStateReusableData.shouldSprint)
            {
                return;
            }

            //û���ƶ�����
            if (playerMovementStateMachine.playerStateReusableData.movementInput != Vector2.zero)
            {
                return;
            }

            playerMovementStateMachine.playerStateReusableData.shouldSprint = false;
        }

        /// <summary>
        /// ����ײ���ڼ���Ƿ�������
        /// </summary>
        /// <returns></returns>
        private bool IsThereGroundUnderneath()
        {
            //��ײ��
            BoxCollider groundCheckCollider = playerMovementStateMachine.player.colliderUtility.triggerColliderData.groundCheckCollider;

            //��ײ�����ĵ�
            Vector3 groundColliderCenterInWorldSpace = groundCheckCollider.bounds.center;

            //������ײ����ײ������
            Collider[] overlappedGroundColliders = Physics.OverlapBox(groundColliderCenterInWorldSpace,
                groundCheckCollider.bounds.center, 
                groundCheckCollider.transform.rotation, 
                playerMovementStateMachine.player.playerLayerData.groundLayer,
                QueryTriggerInteraction.Ignore);

            return overlappedGroundColliders.Length > 0;
        }
        #endregion

        #region  Reusable Methods
        /// <summary>
        /// ������ƶ�ʱ�����ƶ���������(�����������ƶ�״̬
        /// </summary>
        protected virtual void OnMove()
        {
            //����Ǽ���״̬
            if (playerMovementStateMachine.playerStateReusableData.shouldSprint)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.sprintingState);

                return;
            }

            //�������walk��
            if (playerMovementStateMachine.playerStateReusableData.shouldWalk)
            {
                //�л�Ϊ����״̬
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.walkingState);
                return;
            }

            //�����Ǳ���״̬
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.runningState);
        }

        //��Ӱ����Ļص�
        protected override void AddInputActionsCallback()
        {
            base.AddInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Movement.canceled += OnMovementCanceled;

            playerMovementStateMachine.player.input.playerActions.Dash.started += OnDashStarted;

            playerMovementStateMachine.player.input.playerActions.Jump.started += OnJumpStarted;
        }

        //�Ƴ������Ļص�
        protected override void RemoveInputActionsCallback()
        {
            base.RemoveInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Movement.canceled -= OnMovementCanceled;

            playerMovementStateMachine.player.input.playerActions.Dash.started -= OnDashStarted;

            playerMovementStateMachine.player.input.playerActions.Jump.started -= OnJumpStarted;
        }

        /// <summary>
        /// �·���ײ���뿪׼������״̬
        /// </summary>
        /// <param name="collider"></param>
        protected override void OnContactWithGroundExited(Collider collider)
        {
            base.OnContactWithGroundExited(collider);

            //�·�groundCheck��ײ�����Ƿ�������
            if (IsThereGroundUnderneath())
            {
                return;
            }

            Vector3 capsuleColliderCenterInWorldSpace = 
                playerMovementStateMachine.player.colliderUtility.capsulColliderData.collider.bounds.center;

            //������ײ��ĵײ�λ�����·��������
            Ray downwardsRayFromCapsuleBottom = new Ray(capsuleColliderCenterInWorldSpace - 
                playerMovementStateMachine.player.colliderUtility.capsulColliderData.colliderVerticalExtents, 
                Vector3.down);

            //�ڶ̾�������߼����û�����壬��ôִ�������״̬
            if(!Physics.Raycast(downwardsRayFromCapsuleBottom,
                out _, 
                playerGroundedMovementData.groundToFallRayDistance, 
                playerMovementStateMachine.player.playerLayerData.groundLayer, 
                QueryTriggerInteraction.Ignore))
            {
                //����״̬
                OnFall();
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        protected virtual void OnFall()
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerFallingState);
        }

        #endregion

        #region Input Methods
        /// <summary>
        /// û���ƶ�����������
        /// </summary>
        /// <param name="context"></param>
        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.idlingState);
        }

        /// <summary>
        /// ����һ�γ��
        /// </summary>
        /// <param name="context"></param>
        protected virtual void OnDashStarted(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.dashState);
        }

        /// <summary>
        /// ����һ����Ծ
        /// </summary>
        /// <param name="context"></param>
        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.playerJumpState);
        }
        #endregion
    }
}
