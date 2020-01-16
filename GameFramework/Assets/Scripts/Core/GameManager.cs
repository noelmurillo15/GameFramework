/*
 *   GameManager - Backbone of the game application
 *   TODO : Use a preload scene for persistent manager
 *   Created by : Allan N. Murillo
 */
using System;
using UnityEngine;
using System.Collections;

namespace GameFramework.Core
{
    public sealed class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private float _deltaTime = 0.0f;
        private SaveSettings _saveSettings = null;
        [SerializeField] private bool displayFps = false;
        [SerializeField] private GameEvent onApplicationQuitEvent = null;
        [HideInInspector] public SceneTransitionManager sceneTransitionManager = null;
        

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            DontDestroyOnLoad(gameObject);
            Instance = this;
            
            GameSettingsManager.SettingsLoadedIni = false;
            _saveSettings = new SaveSettings();
            _saveSettings.Initialize();

            sceneTransitionManager = gameObject.GetComponentInChildren<SceneTransitionManager>();
            sceneTransitionManager.ScreenMaskBrightness = 0f;
            
            if (Application.platform == RuntimePlatform.WebGLPlayer) return;
            sceneTransitionManager.Initialize();
        }

        private void Update()
        {
            if (!displayFps) return;
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        }

        private void OnGUI()
        {
            if (!displayFps) return;
            var style = new GUIStyle();
            int w = Screen.width, h = Screen.height;
            h *= 2 / 100;
            var rect = new Rect(w - 180, 0, w, h);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = Color.white;
            var msecs = _deltaTime * 1000.0f;
            var fps = 1.0f / _deltaTime;
            var text = $"{msecs:0.0} ms ({fps:0.} fps)";
            GUI.Label(rect, text, style);
        }

        public void StartGameEvent()
        {
            sceneTransitionManager.LoadLevel(2);
        }

        public void LoadMainMenuEvent()
        {
            sceneTransitionManager.LoadMainMenu();
        }

        public void QuitApplicationEvent()
        {
            StartCoroutine(QuitSequence());
        }

        private IEnumerator QuitSequence()
        {
            _saveSettings.SaveGameSettings();
            yield return sceneTransitionManager.FadeOut(3f);
            onApplicationQuitEvent.Raise();
            sceneTransitionManager.LoadCreditsScene();
        }

        private void OnDestroy()
        {
            if (Instance != this) return;
            onApplicationQuitEvent.UnregisterAllListeners();
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        #region External JavaScript Library
        public void LoadSettingsFromIndexedDb()
        {    //    When playing in WebGL, a Javascript plugin will initialize SceneLoader via LoadSettingsFromIndexedDb()
            _saveSettings.LoadGameSettings();
            sceneTransitionManager.Initialize();
        }
        
#if UNITY_WEBGL && !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("__Internal")]
        static extern void QuitGame();


        public void Quit()
        { 
            Debug.Log("GM::Quit() -Trying to call JavaScript Func : QuitGame()");      
            QuitGame();
        }        
#else
        public void Quit() {  }
#endif
        #endregion
    }
}