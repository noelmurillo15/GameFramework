/*
 * SceneExtension - Static Class used for async scene transitions from anywhere
 * Created by : Allan N. Murillo
 * Last Edited : 2/17/2020
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
        

        private static string GetCurrentSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }
        
        private static void OnStartSceneLoadEvent(bool wait = false)
        {
            StartSceneLoadEvent?.Invoke(wait);
        }

        private static void OnFinishSceneLoadEvent(bool wait = false)
        {
            FinishSceneLoadEvent?.Invoke(wait);
        }
        
        public static IEnumerator LoadSimpleScene(string sceneName, bool wait = false)
        {
            OnStartSceneLoadEvent(wait);
            if(wait) yield return new WaitForSeconds(1.1f);
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single).completed +=
                callback => OnFinishSceneLoadEvent(wait);
        }
        
        public static IEnumerator LoadAdditiveScene(string sceneName, bool wait = false)
        {
            OnStartSceneLoadEvent(wait);
            if(wait) yield return new WaitForSeconds(1.1f);
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            OnFinishSceneLoadEvent(wait);
        }
        
        public static IEnumerator ForceSwitchToMenu(bool unloadAll = false)
        {
            var menu = SceneManager.GetSceneByName(MenuUiSceneName);
            if (menu.isLoaded)
            {
                yield return SceneManager.SetActiveScene(SceneManager.GetSceneByName(MenuUiSceneName));
                if (!unloadAll) yield break;
                UnloadAllSceneExceptMenu();
                OnFinishSceneLoadEvent();
                yield break;
            }
            SceneManager.LoadSceneAsync(MenuUiSceneName, LoadSceneMode.Single).completed +=
                callback => OnFinishSceneLoadEvent(true);
        }

        public static IEnumerator ReloadScene()
        {
            OnStartSceneLoadEvent();
            var sceneToBeReloaded = GetCurrentSceneName();
            yield return ForceSwitchToMenu();
            if(SceneManager.GetSceneByName(sceneToBeReloaded).isLoaded)
                yield return SceneManager.UnloadSceneAsync(sceneToBeReloaded);
            yield return SceneManager.LoadSceneAsync(sceneToBeReloaded, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToBeReloaded));
            OnFinishSceneLoadEvent();
        }

        public static bool IsSceneActive(string sceneName)
        {
            return GetCurrentSceneName().Contains(sceneName);
        }
        
        public static void TrySwitchToScene(string sceneName)
        {
            var tryScene = SceneManager.GetSceneByName(sceneName);
            if (!tryScene.isLoaded) return;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }
        
        public static void UnloadAllSceneExceptMenu()
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name.Contains(MenuUiSceneName))
                {
                    SceneManager.SetActiveScene(SceneManager.GetSceneAt(i));
                    continue;
                }
                
                if (SceneManager.GetSceneAt(i).isLoaded)
                {
                    SceneManager.UnloadSceneAsync(
                        SceneManager.GetSceneByName(SceneManager.GetSceneAt(i).name));
                }
            }
        }
    }
}
