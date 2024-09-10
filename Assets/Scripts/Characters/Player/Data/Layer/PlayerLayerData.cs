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
        /// 比较编码是否输出 ！0（也就是找到了这个Layer
        /// </summary>
        /// <param name="layerMask"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public bool ContainsLayer(LayerMask layerMask, int layer)
        {
            return (1 << layer & layerMask) != 0;
        }

        /// <summary>
        /// 判断是否为地面
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public bool IsGroundLayer(int layer)
        {
            return ContainsLayer(groundLayer,layer);
        }
    }
}
