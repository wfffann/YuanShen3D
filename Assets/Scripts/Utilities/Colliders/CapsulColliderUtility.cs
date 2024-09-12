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
        /// 初始化数据
        /// </summary>
        /// <param name="gameObject"></param>
        public void Initialize(GameObject gameObject)
        {
            if(capsulColliderData != null)
            {
                return;
            }

            capsulColliderData = new CapsulColliderData();

            capsulColliderData.Initialize(gameObject);

            OnInitialize();
        }

        protected virtual void OnInitialize()
        {

        }

        /// <summary>
        /// 计算碰撞体变化后的尺寸
        /// </summary>
        public void CalculateCapsuleColliderDimensions()
        {
            //半径
            SetCapsuleColliderRadius(defaultColliderData.radius);

            //长度
            SetCapsuleColliderHeight(defaultColliderData.height * (1f - slopeData.stepHeightPercentage));//原长度的75%

            //中心
            SetCapsuleColliderCenter();

            //碰撞体为一个球形时
            float halfColliderHeight = capsulColliderData.collider.height / 2f;
            if ( halfColliderHeight < capsulColliderData.collider.radius)
            {
                //碰撞体变为一个很大的球形
                SetCapsuleColliderRadius(halfColliderHeight);
            }

            capsulColliderData.UpdateColliderData();
        }

        /// <summary>
        /// 获取碰撞体变化后的半径
        /// </summary>
        /// <param name="radius"></param>
        public void SetCapsuleColliderRadius(float radius)
        {
            capsulColliderData.collider.radius = radius;
        }

        /// <summary>
        /// 获取碰撞体变化后的长度
        /// </summary>
        /// <param name="height"></param>
        public void SetCapsuleColliderHeight(float height)
        {
            capsulColliderData.collider.height = height;
        }

        /// <summary>
        /// 获取碰撞体变化后的中心
        /// </summary>
        private void SetCapsuleColliderCenter()
        {
            //计算插值
            float colliderHeightDifference = defaultColliderData.height - capsulColliderData.collider.height;

            //移动中心
            Vector3 newColliderCenter = new Vector3(0f, defaultColliderData.centerY + (colliderHeightDifference / 2f), 0f);

            capsulColliderData.collider.center = newColliderCenter;
        }
    }
}
