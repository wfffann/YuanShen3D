using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public class CapsulColliderData
    {
        public CapsuleCollider collider { get; private set; }

        public Vector3 colliderCenterInLocalSpace {  get; private set; }//��ײ�������

        public Vector3 colliderVerticalExtents { get; private set; }

        /// <summary>
        /// ��ײ���ʼ��
        /// </summary>
        /// <param name="gameObject"></param>
        public void Initialize(GameObject gameObject)
        {
            if(collider != null)
            {
                return;
            }

            collider = gameObject.GetComponent<CapsuleCollider>();

            //���µ�ǰ��ײ�������
            UpdateColliderData();
        }

        /// <summary>
        /// ���µ�ǰ��ײ�������
        /// </summary>
        public void UpdateColliderData()
        {
            colliderCenterInLocalSpace = collider.center;

            colliderVerticalExtents = new Vector3(0f, collider.bounds.extents.y, 0f);//һ���Height
        }
    }
}
