using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    public class AnimationTrigger : MonoBehaviour
    {
        private PlayerController controller;

        private void Awake()
        {
            controller = transform.parent.GetComponent<PlayerController>();
        }

        public void OnAnimationEnterTrigger()
        {
            if(IsInAnimationTrasation())
            {
                return;
            }

            controller.AnimationEnter();
        }

        public void OnAnimationExitTrigger()
        {
            if (IsInAnimationTrasation())
            {
                return;
            }

            controller.AnimationExit();
        }

        public void OnAnimationTransationTrigger()
        {
            if (IsInAnimationTrasation())
            {
                return;
            }

            controller.AnimationTransition();
        }

        // Ϊ��Ԥ������˵����HardStoping����Ѿ�����HardStop��Trasation�ˣ�������ΪDash�˴��������DashState�����Ե���
        // DashState��Trasation�ˣ��������Ѿ���Trasation����ôû�н����Ͳ��ܵ���������Trasation��
        private bool IsInAnimationTrasation(int layerIndex = 0)
        {
            if(controller.animator.IsInTransition(layerIndex))
            {
                return true;
            }

            return false;
        }
    }
}
