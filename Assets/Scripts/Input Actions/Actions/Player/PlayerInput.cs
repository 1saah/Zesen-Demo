using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovement
{
    public class PlayerInput : MonoBehaviour
    {
        
        public PlayerInputActions InputActions { get; private set; }
        // 这里的Action指的是我们在Action Maps中设置的类，他会自动加上Action后缀
        // 但是我们调用的类对象还是原来的名字没有加Action后缀
        public PlayerInputActions.PlayerActions PlayerActions { get; private set; }
        public PlayerInputActions.UIActions UIActions { get; private set; }

        private void Awake()
        {
            InputActions = new PlayerInputActions();
            PlayerActions = InputActions.Player;
            UIActions = InputActions.UI;
        }

        private void OnEnable()
        {
            InputActions.Enable();
            UIActions.Enable();
        }

        private void OnDisable()
        {
            InputActions.Disable();
            UIActions.Disable();
        }

        // 使用协程暂停某一个按键功能一定时间
        public void DiaableAction(InputAction action, float seconds)
        {
            StartCoroutine(IEDisableAction(action, seconds));
        }

        // 使用协程暂停某一个按键功能
        private IEnumerator IEDisableAction(InputAction action, float seconds)
        {
            action?.Disable();
            yield return new WaitForSeconds(seconds);
            action?.Enable();
        }
    }
}
