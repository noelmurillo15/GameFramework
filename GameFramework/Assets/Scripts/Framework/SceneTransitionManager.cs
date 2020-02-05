/*
 * SceneTransitionManager - Used for async scene transitions
 * Fades the screen in-between loading scenes
 * Created by : Allan N. Murillo
 */

using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ANM.Framework
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SceneTransitionManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeOutDelay = 1.5f;
        [SerializeField] private float fadeInDelay = 0.5f;

        public GameEvent onLoadScene;
        public GameEvent onFinishLoadScene;
        
        private const string MenuUiSceneName = "Menu Ui";
        private const string CreditsSceneName = "Credits";
        private const string GameplaySceneName = "Level 1";
        
        private string[] _sceneNames;
        private Coroutine _currentFade;
        

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            FadeInImmediate();
            
            var sceneNumber = SceneManager.sceneCountInBuildSettings;
            _sceneNames = new string[sceneNumber];
            for (var i = 0; i < sceneNumber; i++)
                _sceneNames[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            
            if (IsThisSceneActive(MenuUiSceneName)) return;
            LoadMenu();
        }
        
        private void LoadMenu()
        {
            if (SceneManager.GetSceneByName(MenuUiSceneName).isLoaded) return;
            SceneManager.LoadSceneAsync(MenuUiSceneName, LoadSceneMode.Additive).completed += operation =>
            {
                if (!SceneManager.GetSceneByName(_sceneNames[0]).isLoaded) return;
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(MenuUiSceneName));
                SceneManager.UnloadSceneAsync(_sceneNames[0]);
            };
        }
        
        public void LoadGameplay()
        {
            StartCoroutine(LoadMultiScene(GameplaySceneName));
        }
        
        public void LoadCredits()
        {
            StartCoroutine(LoadSimpleScene(CreditsSceneName));
        }

        public void ReloadCurrentScene()
        {    //    TODO : Currently bugged
            var sceneToBeReloaded = GetCurrentSceneName();
            if (SceneManager.SetActiveScene(SceneManager.GetSceneByName(MenuUiSceneName)))
            {
                SceneManager.UnloadSceneAsync(sceneToBeReloaded).completed += operation =>
                {
                    StartCoroutine(LoadMultiScene(sceneToBeReloaded));
                };
            }
        }
        
        public static bool IsMainMenuActive()
        {
            return IsThisSceneActive(MenuUiSceneName);
        }

        public static void SwitchToLoadedScene(string sceneName)
        {
            if (!SceneManager.GetSceneByName(sceneName).isLoaded) return;
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

        private IEnumerator LoadMultiScene(string sceneName)
        {
            yield return FadeOut();
            onLoadScene.Raise();
            yield return LoadAdditiveScene(sceneName);
            onFinishLoadScene.Raise();
            yield return FadeIn();
        }
        
        private static IEnumerator LoadAdditiveScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }
        
        private static IEnumerator LoadSimpleScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }
        
        private static string GetCurrentSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }
        
        private static bool IsThisSceneActive(string sceneName)
        {
            return GetCurrentSceneName().Contains(sceneName);
        }

        #region Screen Fade
        private void FadeOutImmediate()
        {
            canvasGroup.alpha = 1f;
        }
        
        private void FadeInImmediate()
        {
            canvasGroup.alpha = 0f;
        }

        public Coroutine FadeOut()
        {
            return Fade(1f, fadeOutDelay);
        }

        private Coroutine FadeIn()
        {
            return Fade(0f, fadeInDelay);
        }

        private Coroutine Fade(float target, float time)
        {
            if (_currentFade != null) { StopCoroutine(_currentFade); }
            _currentFade = StartCoroutine(FadeRoutine(target, time));
            return _currentFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
        #endregion
    }
}
