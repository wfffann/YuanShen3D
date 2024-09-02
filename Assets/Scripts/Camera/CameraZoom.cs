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

        private float targetDistance;//目标缩放距离

        private void Awake()
        {
            cinemachineFramingTransposer = 
                GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
            cinemachineInputProvider = GetComponent<CinemachineInputProvider>();

            //初始缩放距离
            targetDistance = defaultDistance;
        }

        private void Update()
        {
            Zoom();
        }
        
        /// <summary>
        /// 镜头缩放
        /// </summary>
        private void Zoom()
        {
            float zoomValue = cinemachineInputProvider.GetAxisValue(2) * zoomSensitivity;//获取鼠标滑轮的Z轴输入

            //限制目标缩放距离
            targetDistance = Mathf.Clamp(targetDistance + zoomValue, minimumDistance, maximumDistance);

            //获取当前缩放距离
            float currentDistance = cinemachineFramingTransposer.m_CameraDistance;

            //判断距离
            if(currentDistance == targetDistance)
            {
                return;
            }

            //插值移动（弹性模拟（但永远不会到达
            float lerpedZoomValue = Mathf.Lerp(currentDistance, targetDistance, smoothing * Time.deltaTime);
            cinemachineFramingTransposer.m_CameraDistance = lerpedZoomValue;
        }
    }
}
