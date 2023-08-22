using UnityEditor;
using System.IO;

namespace GenshinImpactMovement
{
    public class CreateAssetBundles
    {
        [MenuItem("Assets/CreateAB")]
        static void CreateAB()
        {
            string dir = "AssetBundles";
            if(!Directory.Exists(dir)) { 
                Directory.CreateDirectory(dir);
            }

            BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        }
    }
}
