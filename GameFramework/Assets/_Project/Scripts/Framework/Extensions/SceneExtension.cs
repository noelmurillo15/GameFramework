/*
 * SceneExtension - Static Class used for async scene transitions via Co-routines from anywhere
 * Classes can subscribe to SceneLoadEvents and be notified before and after a scene transition occured
 * Created by : Allan N. Murillo
 * Last Edited : 7/8/2020
 */

using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ANM.Framework.Extensions
{
    public static class SceneExtension
    {
        public static event Action<bool, bool> StartSceneLoadEvent = (fade, save) => { };
        public static event Action<bool, bool> FinishSceneLoadEvent = (fade, save) => { };
        public const string MenuUiSceneName = "Menu Ui";


        public static int GetCurrentSceneBuildIndex()
        {
            return GetCurrentScene().buildIndex;
        }

        public static bool IsThisSceneActive(string sceneName)
        {
            return GetCurrentScene().name.Contains(sceneName);
        }

        public static bool TrySwitchToScene(string sceneName)
        {
            var tryScene = GetLoadedScene(sceneName);
            if (!IsSceneLoaded(tryScene)) return false;
            SetThisSceneActive(tryScene);
            return true;
        }

        public static bool TrySwitchToScene(int buildIndex)
        {
            var tryScene = GetSceneFromBuildIndex(buildIndex);
            if (!IsSceneLoaded(tryScene)) return false;
            SetThisSceneActive(tryScene);
            return true;
        }

        public static IEnumerator LoadSingleSceneSequence(string sceneName, bool fade = false)
        {
            yield return OnStartLoadWithFade(fade);
            LoadSingleSceneWithOnFinish(sceneName);
        }

        public static IEnumerator LoadMultiSceneSequence(string sceneName, bool fade = false)
        {
            yield return OnStartLoadWithFade(fade);
            LoadMultiSceneWithOnFinish(sceneName);
        }

        public static IEnumerator LoadMultiSceneWithBuildIndexSequence(int index, bool fade = false, bool save = false)
        {
            if (index == -1) yield break;
            yield return OnStartLoadWithFade(fade);

            if (TrySwitchToScene(MenuUiSceneName)) UnloadAllScenesExcept(MenuUiSceneName);
            else yield return ForceMenuSceneSequence(true);

            yield return SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
            SetThisSceneActive(GetSceneFromBuildIndex(index));
            CallOnFinishSceneLoadEvent(fade, save);
        }

        public static IEnumerator ReloadCurrentSceneSequence()
        {
            Debug.Log("[SceneExtension]: ReloadCurrentSceneSequence");
            var sceneToReload = GetCurrentScene();
            yield return OnStartLoadWithFade();
            yield return ForceMenuSceneSequence();
            var reloadSceneName = sceneToReload.name;
            if (IsSceneLoaded(sceneToReload)) UnloadThisActiveScene(sceneToReload);
            LoadMultiSceneWithOnFinish(reloadSceneName);
        }

        public static IEnumerator ForceMenuSceneSequence(bool unloadAll = false, bool fade = false, bool save = false,
            int lastBuildIndex = -1)
        {
            if (lastBuildIndex != -1)
            {
                SetThisSceneActive(GetSceneFromBuildIndex(lastBuildIndex));
                CallOnStartSceneLoadEvent(fade, save);
            }

            var menu = GetLoadedScene(MenuUiSceneName);
            if (IsSceneLoaded(menu))
            {
                SetThisSceneActive(menu);
                if (!unloadAll) yield break;
                UnloadAllScenesExcept(MenuUiSceneName);
                CallOnFinishSceneLoadEvent(fade);
                yield break;
            }

            LoadSingleSceneWithOnFinish(MenuUiSceneName);
        }

        public static void UnloadAllScenesExcept(string sceneName)
        {
            for (var sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
            {
                var loadedScene = GetLoadedScene(sceneIndex);
                if (loadedScene.name.Contains(sceneName)) continue;
                UnloadThisActiveScene(loadedScene);
            }
        }

        private static bool IsSceneLoaded(Scene scene)
        {
            return scene.isLoaded;
        }

        private static Scene GetCurrentScene()
        {
            return SceneManager.GetActiveScene();
        }

        private static Scene GetLoadedScene(int sceneIndex)
        {
            return SceneManager.GetSceneAt(sceneIndex);
        }

        private static Scene GetLoadedScene(string sceneName)
        {
            return SceneManager.GetSceneByName(sceneName);
        }

        private static Scene GetSceneFromBuildIndex(int buildIndex)
        {
            return SceneManager.GetSceneByBuildIndex(buildIndex);
        }

        public static IEnumerator OnStartLoadWithFade(bool fade = false, bool save = false)
        {
            CallOnStartSceneLoadEvent(fade, save);
            if (fade) yield return new WaitForSeconds(1.1f);
        }

        public static IEnumerator OnFinishedLoadWithFade(bool fade = false)
        {
            CallOnFinishSceneLoadEvent(fade);
            if (fade) yield return new WaitForSeconds(1.1f);
        }

        private static void SetThisSceneActive(Scene scene)
        {
            SceneManager.SetActiveScene(scene);
        }

        private static void UnloadThisActiveScene(Scene scene)
        {
            SceneManager.UnloadSceneAsync(scene);
        }

        private static void LoadSingleSceneWithOnFinish(string sceneName)
        {
            Debug.Log("[SceneExtension]: LoadSingleSceneWithOnFinish - "+sceneName);
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single).completed +=
                callback => CallOnFinishSceneLoadEvent(true);
        }

        private static void LoadMultiSceneWithOnFinish(string sceneName)
        {
            if (IsThisSceneActive(sceneName)) return;
            Debug.Log("[SceneExtension]: LoadMultiSceneWithOnFinish - "+sceneName);
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed +=
                callback =>
                {
                    SetThisSceneActive(GetLoadedScene(sceneName));
                    CallOnFinishSceneLoadEvent(true, true);
                };
        }

        private static void CallOnStartSceneLoadEvent(bool fade = false, bool save = false)
        {
            StartSceneLoadEvent?.Invoke(fade, save);
        }

        private static void CallOnFinishSceneLoadEvent(bool fade = false, bool load = false)
        {
            FinishSceneLoadEvent?.Invoke(fade, load);
        }
    }
}
