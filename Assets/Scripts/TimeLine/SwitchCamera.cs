using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

namespace GenshinImpactMovement
{
    public class SwitchCamera : Singleton<SwitchCamera>
    {
        private GameObject virtualCamera_main;
        private GameObject virtualCamera_start;

        private PlayableDirector playableDirector;

        private PlayerInput playerInput;

        protected override void Awake()
        {
            base.Awake();
            virtualCamera_main = GameObject.Find("Player VM");
            virtualCamera_start = GameObject.Find("CM vcam1");
            playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();

            playableDirector = GetComponent<PlayableDirector>();

            playableDirector.stopped += SwitchToMainVM;


        }

        private void Start()
        {
            SwitchToStartVM();
            playableDirector.Play();
        }

        private void SwitchToStartVM()
        {
            // 禁止玩家输入
            playerInput.enabled = false;
            virtualCamera_start.SetActive(true);
            virtualCamera_main.SetActive(false);
        }

        private void SwitchToMainVM(PlayableDirector director)
        {
            // 允许玩家输入
            if(playerInput!=null)
            {
                playerInput.enabled = true;
            }
           
            if (virtualCamera_start != null && virtualCamera_main != null)
            {
                virtualCamera_start.SetActive(false);
                virtualCamera_main.SetActive(true);
            }
        }
    }
}
