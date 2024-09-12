using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [Serializable]
    public class PlayerCameraRecenteringData


    {
        [field: SerializeField][field: Range(0f, 360f)] public float minimumAngle {  get; private set; }
        [field: SerializeField][field: Range(0f, 360f)] public float maximumAngle {  get; private set; }
        [field: SerializeField][field: Range(-1f, 20f)] public float waitTime {  get; private set; }
        [field: SerializeField][field: Range(-1f, 20f)] public float recenteringTime {  get; private set; }

        /// <summary>
        /// ÔÚ½Ç¶È·¶Î§ÄÚ
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public bool isWithinRange(float angle)
        {
            return angle >= minimumAngle && angle <= maximumAngle;
        }
    }
}
