#if UNITY_EDITOR
/*
 * ToolsMenu - Creates a tool bar menu in Unity Editor with several tools functionality
 * Created by : Allan N. Murillo
 * Last Edited : 9/22/2021
 */

using UnityEditor;
using static UnityEditor.AssetDatabase;

namespace ANM.EditorUtils
{
    public static class ToolsMenu
    {
        [MenuItem("Tools/ANM/Setup/Create Project Hierarchy")]
        public static void CreateDefaultFolders()
        {
            Folders.CreateDirectories("_Project", "Scripts", "Scenes", "Art", "Audio", "Data", "Prefabs");
            Folders.CreateDirectories("AssetPacks");
            Refresh();
        }

        [MenuItem("Tools/ANM/Packages/Load Default Packages")]
        private static async void LoadNewManifest() => await Packages.ReplacePackageFromGist(
            "85d589de97334e33cfed4b1d80537e44", "46f62c12f5aa3a62ccaed474744992d500a2b407");

        [MenuItem("Tools/ANM/Packages/Load Input System")]
        private static void AddNewInputSystem() => Packages.InstallUnityPackage("inputsystem");

        [MenuItem("Tools/ANM/Packages/Load Post Processing")]
        private static void AddPostProcessing() => Packages.InstallUnityPackage("postprocessing");

        [MenuItem("Tools/ANM/Packages/Load Cinemachine")]
        private static void AddCinemachine() => Packages.InstallUnityPackage("cinemachine");

        [MenuItem("Tools/ANM/Packages/Load Timeline")]
        private static void AddTimeline() => Packages.InstallUnityPackage("timeline");
    }
}
#endif