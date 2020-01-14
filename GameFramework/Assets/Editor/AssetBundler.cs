#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;


public class AssetBundler : Editor
{
    [MenuItem("Assets/Build AssetBundles")]
    private static void BuildAllAssetBundles()
    {
        string path = Application.streamingAssetsPath;
        if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

        //  Assetbundles need to be downloaded from the same server as the game
        //  For this reason, ChunkBasedCompression (lz4) works better than gz due to decompression of the bundle after loading it
        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }
}
#endif