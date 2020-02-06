/*
 * GameManager - Backbone of the game application
 * Contains the most important data used throughout the game (ie: Score, Settings)
 * Created by : Allan N. Murillo
 */

using System;
using UnityEngine;

namespace ANM.Framework
{
    public sealed class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [HideInInspector] public SceneTransitionManager sceneTransitionManager = null;
        public GameEvent onApplicationQuitEvent = null;
        
        public bool IsSceneTransitioning { get; set; } = false;
        
        [SerializeField] private float _deltaTime = 0.0f;
        [SerializeField] private bool _displayFps = false;
        [SerializeField] private bool _isMainMenuActive = false;
        [SerializeField] private bool _isGamePaused = false;
        [SerializeField] private SaveSettings _saveSettings = null;


        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            DontDestroyOnLoad(gameObject);
            Instance = this;
            
            SaveSettings.SettingsLoadedIni = false;
            _saveSettings = new SaveSettings();
            _saveSettings.Initialize();

            sceneTransitionManager = gameObject.GetComponentInChildren<SceneTransitionManager>();
            sceneTransitionManager.ScreenMaskBrightness = 0f;
            Reset();
        }
        
        private void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        }

        private void OnGUI()
        {
            if (!_displayFps) return;
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

        public void Reset()
        {
            Time.timeScale = 1;
            _isGamePaused = false;
        }

        public void OnPauseEvent()
        {
            SwitchToLoadedScene("Menu Ui");
            SetIsGamePaused(true);
        }

        public void OnResumeEvent()
        {
            SwitchToLoadedScene("Level 1");
            SetIsGamePaused(false);
        }

        #region Game Settings
        public void LoadSettingsFromIndexedDb()
        {    //    When playing in WebGL, a Javascript plugin will initialize SceneLoader via LoadSettingsFromIndexedDb()
            _saveSettings.LoadGameSettings();
        }
        #endregion

        #region Game Events
        public void StartGameEvent()
        {
            sceneTransitionManager.LoadSceneByBuildIndex(2);
        }

        public void ReloadScene()
        {
            sceneTransitionManager.ReloadCurrentScene();
        }

        public void SaveGameSettings()
        {
            _saveSettings.SaveGameSettings();
        }
        
        public void LoadCredits()
        {
            sceneTransitionManager.LoadCredits();
        }

        private void OnDestroy()
        {
            if (Instance != this) return;
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }
        #endregion

        public void SwitchToLoadedScene(string sceneName)
        {
            sceneTransitionManager.SwitchToLoadedScene(sceneName);
        }

        public void UnloadAllLoadedScenes()
        {
            sceneTransitionManager.UnloadAllSceneExcept("Credits");
        }

        public void UnloadScenesExceptMenu()
        {
            sceneTransitionManager.UnloadAllSceneExcept("Menu Ui");
        }

        public bool GetIsMainMenuActive()
        {
            return _isMainMenuActive;
        }

        public void SetIsMainMenuActive(bool b)
        {
            _isMainMenuActive = b;
        }
        
        public bool GetIsGamePaused()
        {
            return _isGamePaused;
        }

        public void SetIsGamePaused(bool b)
        {
            _isGamePaused = b;
            //Time.timeScale = b ? 0 : 1;
        }

        #region External JavaScript Library

#if UNITY_WEBGL && !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("__Internal")]
        static extern void QuitGame();


        public void Quit() {  QuitGame(); }
#endif

        #endregion
    }
}
