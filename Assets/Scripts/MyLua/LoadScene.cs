using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace GenshinImpactMovement
{
    public class LoadScene : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(DownLoadFromLocalServer());
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        IEnumerator DownLoadFromLocalServer()
        {
            UnityWebRequest request = UnityWebRequest.Get(@"D:\Codes\UnityProjects\GenshinImpactMovement\NetWork\" + "Update11.lua.txt");
            yield return request.SendWebRequest();
            string file = request.downloadHandler.text;
            File.WriteAllText(@"D:\Codes\UnityProjects\GenshinImpactMovement\Assets\XLua\LuaFiles\Update11.lua.txt", file);

            UnityWebRequest request1 = UnityWebRequest.Get(@"D:\Codes\UnityProjects\GenshinImpactMovement\NetWork\" + "Dispose.lua.txt");
            yield return request1.SendWebRequest();
            string file1 = request1.downloadHandler.text;
            File.WriteAllText(@"D:\Codes\UnityProjects\GenshinImpactMovement\Assets\XLua\LuaFiles\Dispose.lua.txt", file1);
            SceneManager.LoadScene(0);
        }
    }
}
