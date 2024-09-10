using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [Serializable]
    public class PlayerLayerData
    {
        [field: SerializeField] public LayerMask groundLayer {  get; private set; }

        /// <summary>
        /// �Ƚϱ����Ƿ���� ��0��Ҳ�����ҵ������Layer
        /// </summary>
        /// <param name="layerMask"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public bool ContainsLayer(LayerMask layerMask, int layer)
        {
            return (1 << layer & layerMask) != 0;
        }

        /// <summary>
        /// �ж��Ƿ�Ϊ����
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public bool IsGroundLayer(int layer)
        {
            return ContainsLayer(groundLayer,layer);
        }
    }
}
