using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public class CapsulColliderData
    {
        public CapsuleCollider collider { get; private set; }

        public Vector3 colliderCenterInLocalSpace {  get; private set; }//碰撞体的中心

        public Vector3 colliderVerticalExtents { get; private set; }

        /// <summary>
        /// 碰撞体初始化
        /// </summary>
        /// <param name="gameObject"></param>
        public void Initialize(GameObject gameObject)
        {
            if(collider != null)
            {
                return;
            }

            collider = gameObject.GetComponent<CapsuleCollider>();

            //更新当前碰撞体的数据
            UpdateColliderData();
        }

        /// <summary>
        /// 更新当前碰撞体的数据
        /// </summary>
        public void UpdateColliderData()
        {
            colliderCenterInLocalSpace = collider.center;

            colliderVerticalExtents = new Vector3(0f, collider.bounds.extents.y, 0f);//一半的Height
        }
    }
}
