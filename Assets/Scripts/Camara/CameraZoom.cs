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
            // ����ڻ�ȡCinemachine��ʱ��Ҫ�ǵ�д������VirtualCamera
            transposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
            transposer.m_CameraDistance = defaultZoomDistance;
        }

        private void Update()
        {
            Zoom();
        }

        private void Zoom()
        {
            // ��ȡ����
            float zoomInput = inputProvider.GetAxisValue(2) * zoomSensitivity;

            // ��ȡ��ǰ��zoomDiatance
            float currentZoomDistance = transposer.m_CameraDistance;

            // ��ȡĿ��zoom���� Clamp n. ǯ��
            // ����ֵ��������Χ
            float targetZoomDistance = Mathf.Clamp(currentZoomDistance + zoomInput, minZoomDistance, maxZoomDistance);

            // �ж��Ƿ���Ҫ�ı����
            if(targetZoomDistance == currentZoomDistance) 
            {
                return;
            }
            else
            {
                // Mathf�в�ֵ������һ�� ���յ�����������0-1��������ֵ�м��ֵ���ձ������� ����Time.deltaTime����ʹ����1���ڴﵽĿ��Ƕ� 
                // ����ͨ��smoothing���ʵ�������ʱ��
                float zoomChangeValue = Mathf.Lerp(currentZoomDistance, targetZoomDistance, Time.deltaTime * smoothing);
                transposer.m_CameraDistance = zoomChangeValue;
            }

        }
    }
}
