using System.IO;
using UnityEditor;
using UnityEngine;


namespace GameFramework
{
    public class AssetBundler : Editor
    {
        [MenuItem("Assets/Build AssetBundles")]
        static void BuildAllAssetBundles()
        {
            string path = Application.streamingAssetsPath;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh();
        }
    }
}