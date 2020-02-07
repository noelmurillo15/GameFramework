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

        [HideInInspector] public SceneTransitionManager sceneTransitionManager;
        
        [SerializeField] private bool displayFps = false;
        [SerializeField] private bool isMainMenuActive = false;
        [SerializeField] private bool isGamePaused = false;
        
        public GameEvent onApplicationQuitEvent = null;
        public bool IsSceneTransitioning { get; set; }
        
        private float _deltaTime;
        private SaveSettings _saveSettings;


        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            DontDestroyOnLoad(gameObject);
            Instance = this;
            
            SaveSettings.SettingsLoadedIni = false;
            _saveSettings = new SaveSettings();
            _saveSettings.Initialize();

            sceneTransitionManager = gameObject.GetComponentInChildren<SceneTransitionManager>();
            Reset();
        }
        
        private void Update()
        {
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

        public void Reset()
        {
            Time.timeScale = 1;
            isGamePaused = false;
        }

        public void OnPauseEvent()
        {
            SceneTransitionManager.SwitchToLoadedScene("Menu Ui");
            SetIsGamePaused(true);
        }

        public void OnResumeEvent()
        {
            SceneTransitionManager.SwitchToLoadedScene("Level 1");
            SetIsGamePaused(false);
        }

        public void LoadSettingsFromIndexedDb()
        {    //    When playing in WebGL, a Javascript plugin will initialize SceneLoader via LoadSettingsFromIndexedDb()
            _saveSettings.LoadGameSettings();
        }

        public void StartGameEvent()
        {
            sceneTransitionManager.LoadGameplay();
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

        public bool GetIsMainMenuActive()
        {
            return isMainMenuActive;
        }

        public void SetIsMainMenuActive(bool b)
        {
            isMainMenuActive = b;
        }
        
        public bool GetIsGamePaused()
        {
            return isGamePaused;
        }

        public void SetIsGamePaused(bool b)
        {
            isGamePaused = b;
            Time.timeScale = b ? 0 : 1;
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
