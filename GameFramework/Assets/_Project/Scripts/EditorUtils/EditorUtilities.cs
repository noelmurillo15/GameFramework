#if UNITY_EDITOR
/*
 * EditorUtilities - Contains Static Editor functions which can only be used during development in the Unity Editor
 * FindAssetByType - Used to populate scriptable game data
 * Created by : Allan N. Murillo
 * Last Edited : 5/26/2020
 */

using UnityEditor;
using System.Collections.Generic;

namespace ANM.EditorUtils
{
    public static class EditorUtilities
    {
        public static List<T> FindAssetByType<T>() where T : UnityEngine.Object
        {
            var assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            for (var i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }

            return assets;
        }
    }
}
#endif
