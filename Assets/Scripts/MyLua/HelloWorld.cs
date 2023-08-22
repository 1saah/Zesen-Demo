using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;
using UnityEngine.Networking;

namespace GenshinImpactMovement
{
    [Hotfix]
    public class HelloWorld : MonoBehaviour
    {
        private LuaEnv luaEnv;
        private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

        private void Awake()
        {
            luaEnv = new LuaEnv();
            luaEnv.AddLoader(MyLoader);
            luaEnv.DoString("require 'Update11'");
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDisable()
        {
            luaEnv.DoString("require 'Dispose'");
        }

        private void OnDestroy()
        {
            luaEnv.Dispose();
        }

        #region Usable Methods
        private byte[] MyLoader(ref string path)
        {
            string absPath = @"D:\Codes\UnityProjects\GenshinImpactMovement\Assets\XLua\LuaFiles\" + path + ".lua.txt";
            string textString = File.ReadAllText(absPath);
            return System.Text.Encoding.UTF8.GetBytes(textString);
        }

        [LuaCallCSharp]
        public void LoadResource(string resName, string filePath)
        {
            StartCoroutine(Load(resName, filePath));
        }


        // 需要用协程在不影响主线程的情况下从服务器下载资源并存储在本地字典中
        IEnumerator Load(string resName, string filePath)
        {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(@"http://localhost/AssetBundles/" + filePath);
            yield return request.SendWebRequest();
            AssetBundle ab = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
            GameObject obj = ab.LoadAsset<GameObject>(resName);
            prefabs.Add(resName, obj);
        }

        // 从字典中根据key取出资源
        [LuaCallCSharp]
        public GameObject GetPrefab(string resName)
        {
            if (prefabs.ContainsKey(resName))
            {
                return prefabs[resName];
            }else
            {
                return null;    
            }
        }

        #endregion
    }
}
