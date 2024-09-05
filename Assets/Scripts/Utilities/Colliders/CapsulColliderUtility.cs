using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [Serializable]
    public class CapsulColliderUtility
    {
        public CapsulColliderData capsulColliderData {  get; private set; }
        [field: SerializeField] public DefaultColliderData defaultColliderData { get; private set; }
        [field: SerializeField] public SlopeData slopeData { get; private set; }

        /// <summary>
        /// ������ײ��仯��ĳߴ�
        /// </summary>
        public void CalculateCapsuleColliderDimensions()
        {
            //�뾶
            StepCapsuleColliderRadius(defaultColliderData.radius);

            //����
            StepCapsuleColliderHeight(defaultColliderData.height * (1f - slopeData.stepHeightPercentage));//ԭ���ȵ�75%

            //����
            StepCapsuleColliderCenter();
        }

        /// <summary>
        /// ��ȡ��ײ��仯��İ뾶
        /// </summary>
        /// <param name="radius"></param>
        public void StepCapsuleColliderRadius(float radius)
        {
            capsulColliderData.collider.radius = radius;
        }

        /// <summary>
        /// ��ȡ��ײ��仯��ĳ���
        /// </summary>
        /// <param name="height"></param>
        public void StepCapsuleColliderHeight(float height)
        {
            capsulColliderData.collider.height = height;
        }

        /// <summary>
        /// ��ȡ��ײ��仯�������
        /// </summary>
        private void StepCapsuleColliderCenter()
        {
            //�����ֵ
            float colliderHeightDifference = defaultColliderData.height - capsulColliderData.collider.height;

            //�ƶ�����
            Vector3 newColliderCenter = new Vector3(0f, defaultColliderData.centerY + (colliderHeightDifference / 2f), 0f);

            capsulColliderData.collider.center = newColliderCenter;
        }
    }
}
