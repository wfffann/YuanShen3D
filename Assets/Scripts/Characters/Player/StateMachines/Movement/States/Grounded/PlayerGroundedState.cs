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
                playerMovementStateMachine.player.layerData.groundLayer,
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

                //��ȡ��ײ����������������center
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
        #endregion

        #region  Reusable Methods
        /// <summary>
        /// ������ƶ�ʱ�����ƶ���������
        /// </summary>
        protected virtual void OnMove()
        {
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

        protected override void AddInputActionsCallback()
        {
            base.AddInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Movement.canceled += OnMovementCanceled;
        }

        protected override void RemoveInputActionsCallback()
        {
            base.RemoveInputActionsCallback();

            playerMovementStateMachine.player.input.playerActions.Movement.canceled -= OnMovementCanceled;
        }
        #endregion

        #region Input Methods
        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.idlingState);
        }
        #endregion
    }
}
