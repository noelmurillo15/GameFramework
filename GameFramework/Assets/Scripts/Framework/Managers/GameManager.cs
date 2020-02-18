﻿/*
 * GameManager - Backbone of the game application
 * Contains data that needs to persist and be accessed from anywhere
 * Created by : Allan N. Murillo
 * Last Edited : 2/17/2020
 */

using System;
using UnityEngine;
using ANM.Framework.Events;
using ANM.Framework.Settings;
using ANM.Framework.Extensions;

namespace ANM.Framework.Managers
{
    public sealed class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game Events")]
        [SerializeField] private GameEvent onApplicationQuit = null;
        [SerializeField] private GameEvent onGameResume = null;
        [SerializeField] private GameEvent onGamePause = null;

        [Space] [Header("Local Game Info")]
        [SerializeField] private bool displayFps = false;
        [SerializeField] private bool isGamePaused = false;
        [SerializeField] private bool isMainMenuActive = false;
        [SerializeField] private bool isSceneTransitioning = false;
        
        private float _deltaTime;
        private SaveSettings _save;


        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            DontDestroyOnLoad(gameObject);
            Instance = this;
            
            SaveSettings.SettingsLoadedIni = false;
            _save = new SaveSettings();
            _save.Initialize();
            Reset();
        }

        private void Start()
        {
            SceneExtension.StartSceneLoadEvent += OnStartLoadSceneEvent;
            SceneExtension.FinishSceneLoadEvent += OnFinishLoadSceneEvent;
            if (SceneExtension.IsSceneActive(SceneExtension.SplashSceneName))
                StartCoroutine(SceneExtension.ForceSwitchToMenu());
        }

        private void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
            if (GetIsMainMenuActive()) return;
            if (!Input.GetKeyDown(KeyCode.Tab)) return;
            TogglePause();
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
        
        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus) return;
            WindowLostFocus();
        }

        private void OnDestroy()
        {
            if (Instance != this) return;
            SceneExtension.StartSceneLoadEvent -= OnStartLoadSceneEvent;
            SceneExtension.FinishSceneLoadEvent -= OnFinishLoadSceneEvent;
            Resources.UnloadUnusedAssets();
            GC.Collect();
            Quit();
        }
        
        public void SetIsGamePaused(bool b)
        {
            isGamePaused = b;
            if(isGamePaused)
                RaisePauseEvent();
            else 
                RaiseResumeEvent();
            Time.timeScale = b ? 0 : 1;
        }
        
        public void ReloadScene()
        {
            StartCoroutine(SceneExtension.ReloadScene());
        }

        public void Reset()
        {
            Time.timeScale = 1;
            isGamePaused = false;
        }
        
        public void HardReset()
        {
            RaiseAppQuitEvent();
            Reset();
        }

        public void LoadCredits()
        {
            HardReset();
            StartCoroutine(SceneExtension.LoadSimpleScene(
                SceneExtension.CreditsSceneName, true));
        }

        public void SetDisplayFps(bool b)  {  displayFps = b;  }

        public void SaveGameSettings()  {  _save.SaveGameSettings();  }
        
        public void SetIsMainMenuActive(bool b)  {  isMainMenuActive = b;  }
        
        public bool GetIsMainMenuActive()  {  return isMainMenuActive;  }
        
        public bool GetIsGamePaused()  {  return isGamePaused;  }

        public bool GetIsSceneTransitioning()  {  return isSceneTransitioning;  }
        
        
        private void TogglePause()
        {
            SetIsGamePaused(!GetIsGamePaused());
        }
        
        private void RaisePauseEvent()
        {
            onGamePause.Raise();
        }

        private void RaiseResumeEvent()
        {
            onGameResume.Raise();
        }
        
        private void RaiseAppQuitEvent()
        {
            onApplicationQuit.Raise();
        }

        private void OnStartLoadSceneEvent(bool b)
        {
            isSceneTransitioning = true;
        }

        private void OnFinishLoadSceneEvent(bool b)
        {
            isSceneTransitioning = false;
        }
        
        private void LoadSettingsFromIndexedDb()
        {    //    WebGL Only : Called from WebBrowserInteraction.jslib plugin
            SaveSettings.SettingsLoadedIni = _save.LoadGameSettings();
        }
        
        #region External JavaScript Library
#if UNITY_WEBGL && !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("__Internal")]
        static extern void QuitGame();
        [System.Runtime.InteropServices.DllImport("__Internal")]
        static extern void LostFocus();

        private void Quit() {  QuitGame(); }
        private void WindowLostFocus() { LostFocus(); }
#else
        private void Quit() { }
        private void WindowLostFocus() { }
#endif
        #endregion
    }
}
