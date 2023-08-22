using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace GenshinImpactMovement
{
    public class PauseManager : Singleton<PauseManager>
    {
        public PlayerController controller;
        public bool isPause = true;

        public void SwitchPause()
        {
            isPause = !isPause;
        }

        public void UpdatePauseState()
        {
            if(isPause)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        public void PauseGame()
        {
            CinemachinePOV pov = controller.VirtualCamera.GetCinemachineComponent<CinemachinePOV>();
            pov.m_HorizontalAxis.m_MaxSpeed = 0;
            pov.m_VerticalAxis.m_MaxSpeed = 0;  
            Time.timeScale = 0.0f;
            isPause = true;
        }

        public void ResumeGame()
        {
            CinemachinePOV pov = controller.VirtualCamera.GetCinemachineComponent<CinemachinePOV>();
            pov.m_HorizontalAxis.m_MaxSpeed = 0.16f;
            pov.m_VerticalAxis.m_MaxSpeed = 0.1f;
            Time.timeScale = 1.0f;
            isPause = false;
        }
    }
}
