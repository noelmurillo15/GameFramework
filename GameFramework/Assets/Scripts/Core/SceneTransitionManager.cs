/*
    *   SceneTransitionManager - Used for async scene transitions
    *   Created by : Allan N. Murillo
 */
using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace GameFramework.Core
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SceneTransitionManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeOutDelay = 1.5f;
        [SerializeField] private float fadeInDelay = 0.5f;

        public float ScreenMaskBrightness
        {
            get => _screenMaskBrightness;
            set
            {
                _screenMaskBrightness = value;
                FadeInImmediate();
            }
        }
        private float _screenMaskBrightness = 0.5f;

        public GameEvent onLoadScene;
        public GameEvent onFinishLoadScene;
        
        private string[] _sceneNames = null;
        private Coroutine _currentFade = null;
        

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            FadeInImmediate();
        }
        
        public void Initialize()
        {
            var sceneNumber = SceneManager.sceneCountInBuildSettings;
            _sceneNames = new string[sceneNumber];

            for (int i = 0; i < sceneNumber; i++)
            {
                _sceneNames[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            }
            if (GetCurrentSceneName() != "SplashScreen") return;
            LoadMainMenu();
        }

        public void LoadMainMenu()
        {
            StartCoroutine(LoadNewScene("MainMenu"));
        }

        public void LoadCreditsScene()
        {
            StartCoroutine(LoadNewScene("ExitScreen"));
        }

        public void LoadLevel(int index)
        {
            if (index > _sceneNames.Length - 1 || index < 0)
            {
                Debug.Log("Scene Index out of range : " + index);
            }
            else
            {
                StartCoroutine(LoadNewScene(index));
            }
        }

        public static string GetCurrentSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }

        private IEnumerator LoadNewScene(int index)
        {
            onLoadScene.Raise();
            yield return FadeOut(fadeOutDelay);
            AsyncOperation async = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
            while (!async.isDone) { yield return null; }
            onFinishLoadScene.Raise();
            yield return FadeIn(fadeInDelay);
        }

        private IEnumerator LoadNewScene(string sceneName)
        {
            onLoadScene.Raise();
            yield return FadeOut(fadeOutDelay);
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (!async.isDone) { yield return null; }
            onFinishLoadScene.Raise();
            yield return FadeIn(fadeInDelay);
        }
        
        public void LoadSceneEvent()
        {    //    Handled by onStartSceneTransition ScriptableObject
            //    TODO : Do stuff before scene transition (ie:saving settings)
        }

        public void FinishLoadSceneEvent()
        {    //    Handled by onFinishSceneTransition ScriptableObject
            //    TODO : Do stuff after new scene transition (ie:loading settings)
        }
        
        private void FadeOutImmediate()
        {
            canvasGroup.alpha = 1f;
        }
        
        private void FadeInImmediate()
        {
            canvasGroup.alpha = ScreenMaskBrightness;
        }

        public Coroutine FadeOut(float time)
        {
            return Fade(1f, time);
        }

        private Coroutine FadeIn(float time)
        {
            return Fade(ScreenMaskBrightness, time);
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
    }
}