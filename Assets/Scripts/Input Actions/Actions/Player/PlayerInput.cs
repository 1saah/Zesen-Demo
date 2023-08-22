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
        // �����Actionָ����������Action Maps�����õ��࣬�����Զ�����Action��׺
        // �������ǵ��õ��������ԭ��������û�м�Action��׺
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

        // ʹ��Э����ͣĳһ����������һ��ʱ��
        public void DiaableAction(InputAction action, float seconds)
        {
            StartCoroutine(IEDisableAction(action, seconds));
        }

        // ʹ��Э����ͣĳһ����������
        private IEnumerator IEDisableAction(InputAction action, float seconds)
        {
            action?.Disable();
            yield return new WaitForSeconds(seconds);
            action?.Enable();
        }
    }
}
