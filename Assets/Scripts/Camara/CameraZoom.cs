using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField][Range(0f, 10f)] private float defaultZoomDistance = 6f;
        [SerializeField][Range(0f, 10f)] private float maxZoomDistance = 6f;
        [SerializeField][Range(0f, 10f)] private float minZoomDistance = 1f;

        [SerializeField][Range(0f, 20f)] private float zoomSensitivity = 10f;
        [SerializeField][Range(0f, 10f)] private float smoothing = 4f;

        private CinemachineInputProvider inputProvider;
        private CinemachineFramingTransposer transposer;

        private void Awake()
        {
            inputProvider = GetComponent<CinemachineInputProvider>();
            // 这个在获取Cinemachine的时候要记得写上类型VirtualCamera
            transposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
            transposer.m_CameraDistance = defaultZoomDistance;
        }

        private void Update()
        {
            Zoom();
        }

        private void Zoom()
        {
            // 获取输入
            float zoomInput = inputProvider.GetAxisValue(2) * zoomSensitivity;

            // 获取当前的zoomDiatance
            float currentZoomDistance = transposer.m_CameraDistance;

            // 获取目标zoom距离 Clamp n. 钳子
            // 限制值不超过范围
            float targetZoomDistance = Mathf.Clamp(currentZoomDistance + zoomInput, minZoomDistance, maxZoomDistance);

            // 判断是否需要改变距离
            if(targetZoomDistance == currentZoomDistance) 
            {
                return;
            }
            else
            {
                // Mathf中插值函数的一种 按照第三个参数（0-1）将两个值中间的值按照比例返回 输入Time.deltaTime可以使其在1秒内达到目标角度 
                // 可以通过smoothing来适当提高这个时间
                float zoomChangeValue = Mathf.Lerp(currentZoomDistance, targetZoomDistance, Time.deltaTime * smoothing);
                transposer.m_CameraDistance = zoomChangeValue;
            }

        }
    }
}
