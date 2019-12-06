#if UNITY_EDITOR
/*
    *   EditorUtilities - Contains Static Editor functions which can only be used during development in Editor
    *   FindAssetByType - Used to populate scriptable game data
    *   Created by : Allan N. Murillo
 */
using UnityEditor;
using System.Collections.Generic;


namespace GameFramework.Editor
{
    public static class EditorUtilities
    {
        public static List<T> FindAssetByType<T>() where T : UnityEngine.Object
        {
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null) { assets.Add(asset); }
            }

            return assets;
        }
    }
}
#endif