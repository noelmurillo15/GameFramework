#if UNITY_EDITOR
/*
 * Folders - 
 * Created by : Allan N. Murillo
 * Last Edited : 9/21/2021
 */

using static System.IO.Path;
using static System.IO.Directory;
using static UnityEngine.Application;

namespace ANM.EditorUtils
{
    public static class Folders
    {
        public static void CreateDirectories(string root, params string[] dir)
        {
            var fullPath = Combine(dataPath, root);
            if (!Exists(fullPath)) CreateDirectory(fullPath);
            foreach (var newFolder in dir) CreateDirectory(Combine(fullPath, newFolder));
        }
    }
}
#endif