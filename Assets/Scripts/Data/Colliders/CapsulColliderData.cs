using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public class CapsulColliderData
    {
        public CapsuleCollider collider { get; private set; }

        public Vector3 colliderCenterInLocalSpace {  get; private set; }

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
        }
    }
}
