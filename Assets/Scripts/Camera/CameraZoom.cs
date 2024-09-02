using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] [Range(0f, 10f)]private float defaultDistance = 6f;
        [SerializeField] [Range(0f, 10f)] private float minimumDistance = 1f;
        [SerializeField] [Range(0f, 10f)] private float maximumDistance = 6f;

        [SerializeField] [Range(0f, 10f)] private float smoothing = 4f;
        [SerializeField] [Range(0f, 10f)] private float zoomSensitivity = 1f;

        private CinemachineFramingTransposer cinemachineFramingTransposer;
        private CinemachineInputProvider cinemachineInputProvider;

        private float targetDistance;//Ŀ�����ž���

        private void Awake()
        {
            cinemachineFramingTransposer = 
                GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
            cinemachineInputProvider = GetComponent<CinemachineInputProvider>();

            //��ʼ���ž���
            targetDistance = defaultDistance;
        }

        private void Update()
        {
            Zoom();
        }
        
        /// <summary>
        /// ��ͷ����
        /// </summary>
        private void Zoom()
        {
            float zoomValue = cinemachineInputProvider.GetAxisValue(2) * zoomSensitivity;//��ȡ��껬�ֵ�Z������

            //����Ŀ�����ž���
            targetDistance = Mathf.Clamp(targetDistance + zoomValue, minimumDistance, maximumDistance);

            //��ȡ��ǰ���ž���
            float currentDistance = cinemachineFramingTransposer.m_CameraDistance;

            //�жϾ���
            if(currentDistance == targetDistance)
            {
                return;
            }

            //��ֵ�ƶ�������ģ�⣨����Զ���ᵽ��
            float lerpedZoomValue = Mathf.Lerp(currentDistance, targetDistance, smoothing * Time.deltaTime);
            cinemachineFramingTransposer.m_CameraDistance = lerpedZoomValue;
        }
    }
}
