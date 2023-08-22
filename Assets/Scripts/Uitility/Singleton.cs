using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovement
{
    // 主要给项目中的UI进行使用
    // 好烦啊啊啊啊啊啊啊
    // 想回国顺利找到工作呜呜呜
    // 2023/06/23 不知道以后还有没有机会看到这里
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;

        public static T Instance
        {
            get { return instance; }
        }

        protected virtual void Awake()
        {
            if (instance != null)
            {
                Destroy(this); 
                return;
            }
            else
            {
                instance = (T)this;
            }
        }

        public static bool IsInitialized
        {
            get { return instance != null; }
        }

        protected virtual void OnDestroy()
        {
            if( instance != null )
            {
                instance = null;
            }
        }
    }
}
