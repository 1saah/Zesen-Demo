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

        // 为了预防比如说我在HardStoping最后已经调用HardStop的Trasation了，但是因为Dash了代码进入了DashState，所以调用
        // DashState的Trasation了，因此如果已经在Trasation，那么没有结束就不能调用其它的Trasation。
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
