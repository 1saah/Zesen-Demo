using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace GenshinImpactMovement
{
    [Hotfix]
    public class VersionPrinter : MonoBehaviour
    {
        void Start()
        {
            Print();
        }

        private void Update()
        {
        }

        [LuaCallCSharp]
        private void Print()
        {
            Debug.Log("Version ???");
        }
    }
}
