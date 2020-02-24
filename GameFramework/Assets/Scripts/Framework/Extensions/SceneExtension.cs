/*
 * SceneExtension - Static Class used for async scene transitions via Co-routines from anywhere
 * Classes can subscribe to SceneLoadEvents and be notified before and after a scene transition occured
 * Created by : Allan N. Murillo
 * Last Edited : 2/24/2020
 */

using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ANM.Framework.Extensions
{
    public static class SceneExtension
    {
        public static event Action<bool> StartSceneLoadEvent = (wait) => { };
        public static event Action<bool> FinishSceneLoadEvent = (wait) => { };

        public const string SplashSceneName = "Splash";
        public const string MenuUiSceneName = "Menu Ui";
        public const string CreditsSceneName = "Credits";
        public const string GameplaySceneName = "Level 1";

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
        
        public static IEnumerator LoadSingleSceneSequence(string sceneName, bool fade = false)
        {
            yield return OnStartLoadWithFade(fade);
            LoadSingleSceneWithOnFinish(sceneName, fade);
        }
        
        public static IEnumerator LoadMultiSceneSequence(string sceneName, bool fade = false)
        {
            yield return OnStartLoadWithFade(fade);
            LoadMultiSceneWithOnFinish(sceneName);
        }
        
        public static IEnumerator LoadMultiSceneWithBuildIndexSequence(int sceneIndex, bool fade = false)
        {
            yield return OnStartLoadWithFade(fade);
            
            if (TrySwitchToScene(MenuUiSceneName)) UnloadAllScenesExcept(MenuUiSceneName);
            else yield return ForceMenuSceneSequence(true);
            
            yield return SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            SetThisSceneActive(SceneManager.GetSceneByBuildIndex(sceneIndex));
            CallOnFinishSceneLoadEvent(fade);
        }
        
        public static IEnumerator ReloadCurrentSceneSequence()
        {
            var sceneToReload = GetCurrentScene();
            yield return OnStartLoadWithFade();
            yield return ForceMenuSceneSequence();
            var reloadSceneName = sceneToReload.name;
            if(IsSceneLoaded(sceneToReload)) UnloadThisActiveScene(sceneToReload);
            LoadMultiSceneWithOnFinish(reloadSceneName);
        }
        
        public static IEnumerator ForceMenuSceneSequence(bool unloadAll = false)
        {
            var menu = GetLoadedScene(MenuUiSceneName);
            if (IsSceneLoaded(menu))
            {
                SetThisSceneActive(menu);
                if (!unloadAll) yield break;
                UnloadAllScenesExcept(MenuUiSceneName);
                CallOnFinishSceneLoadEvent();
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

        private static IEnumerator OnStartLoadWithFade(bool fade = false)
        {
            CallOnStartSceneLoadEvent(fade);
            if(fade) yield return new WaitForSeconds(1.1f);
        }

        private static void SetThisSceneActive(Scene scene)
        {
            SceneManager.SetActiveScene(scene);
        }
        
        private static void UnloadThisActiveScene(Scene scene)
        {
            SceneManager.UnloadSceneAsync(scene);
        }
        
        private static void LoadSingleSceneWithOnFinish(string sceneName, bool fade = false)
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single).completed +=
                callback => CallOnFinishSceneLoadEvent(fade);
        }

        private static void LoadMultiSceneWithOnFinish(string sceneName, bool fade = false)
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed += 
                callback => {
                SetThisSceneActive(GetLoadedScene(sceneName));
                CallOnFinishSceneLoadEvent(fade);
            };
        }
        
        private static void CallOnStartSceneLoadEvent(bool wait = false)
        {
            StartSceneLoadEvent?.Invoke(wait);
        }
        
        private static void CallOnFinishSceneLoadEvent(bool wait = false)
        {
            FinishSceneLoadEvent?.Invoke(wait);
        }
    }
}
