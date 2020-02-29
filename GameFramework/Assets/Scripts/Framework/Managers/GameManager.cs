﻿/*
 * GameManager - Backbone of the game application
 * Contains data that needs to persist and be accessed from anywhere
 * Created by : Allan N. Murillo
 * Last Edited : 2/24/2020
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

        private PlayerControls _controls;
        private SaveSettings _save;
        private float _deltaTime;


        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            SaveSettings.SettingsLoadedIni = false;
            DontDestroyOnLoad(gameObject);
            _save = new SaveSettings();
            _save.LoadGameSettings();
            Instance = this;
        }
        
        private void ControllerSetup()
        {
            if(_controls == null) _controls = new PlayerControls();
            _controls.Player.PauseToggle.performed += context => TogglePause();
            _controls.Enable();
        }

        private void Start()
        {
            SceneExtension.StartSceneLoadEvent += OnStartLoadSceneEvent;
            SceneExtension.FinishSceneLoadEvent += OnFinishLoadSceneEvent;
            Invoke(nameof(Initialize), 1f);
            ControllerSetup();
        }

        private void Initialize()
        {
            if (SceneExtension.IsThisSceneActive(SceneExtension.SplashSceneName))
                StartCoroutine(SceneExtension.ForceMenuSceneSequence());
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

        private void OnDestroy()
        {
            if (Instance != this) return;
            SceneExtension.StartSceneLoadEvent -= OnStartLoadSceneEvent;
            SceneExtension.FinishSceneLoadEvent -= OnFinishLoadSceneEvent;
            Resources.UnloadUnusedAssets();
            _controls.Disable();
            GC.Collect();
        }
        
        public void SetIsGamePaused(bool b)
        {
            isGamePaused = b;
            if(isGamePaused) RaisePause();
            else RaiseResume();
            Time.timeScale = b ? 0 : 1;
        }
        
        public void ReloadScene()
        {
            StartCoroutine(SceneExtension.ReloadCurrentSceneSequence());
        }

        public void Reset()
        {
            Time.timeScale = 1;
            isGamePaused = false;
        }
        
        public void HardReset()
        {
            RaiseAppQuit();
            Reset();
        }

        public void LoadCredits()
        {
            HardReset();
            StartCoroutine(SceneExtension.LoadSingleSceneSequence(
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
            if (GetIsMainMenuActive()) return;
            SetIsGamePaused(!GetIsGamePaused());
        }
        
        private void RaisePause()
        {
            onGamePause.Raise();
        }

        private void RaiseResume()
        {
            onGameResume.Raise();
        }
        
        private void RaiseAppQuit()
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
    }
}
