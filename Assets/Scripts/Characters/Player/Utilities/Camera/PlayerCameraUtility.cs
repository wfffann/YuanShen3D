using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [Serializable]
    public class PlayerCameraUtility
    {
        [field: SerializeField] public CinemachineVirtualCamera virtualCamera {  get; private set; }
        [field: SerializeField] public float defaultHorizontalWaitTime { get; private set; } = 0f;
        [field: SerializeField] public float defaultHorizontalRecenteringTime { get; private set; } = 4f;

        private CinemachinePOV cinemachinePOV;

        public void Initialize()
        {
            cinemachinePOV = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        }

        /// <summary>
        /// ��ˮƽ���У�����������
        /// </summary>
        /// <param name="waitTime"></param>
        /// <param name="recenteringTime"></param>
        public void EnableRecentering(float waitTime = -1f, float recenteringTime = -1f,
            float baseMovementSpeed = 1f, float movementSpeed = 1f)
        {
            cinemachinePOV.m_HorizontalRecentering.m_enabled = true;

            //����ˮƽ����
            cinemachinePOV.m_HorizontalRecentering.CancelRecentering();//ȡ������ִ�е�

            if(waitTime == -1f)
            {
                waitTime = defaultHorizontalWaitTime;
            }

            if(recenteringTime == -1f)
            {
                recenteringTime = defaultHorizontalRecenteringTime;
            }

            recenteringTime = recenteringTime * baseMovementSpeed / movementSpeed;

            cinemachinePOV.m_HorizontalRecentering.m_WaitTime = waitTime;
            cinemachinePOV.m_HorizontalRecentering.m_RecenteringTime = recenteringTime;
        }

        /// <summary>
        /// �ر�ˮƽ����
        /// </summary>
        public void DisableRecentering()
        {
            cinemachinePOV.m_HorizontalRecentering.m_enabled = false;
        }
    }
}
