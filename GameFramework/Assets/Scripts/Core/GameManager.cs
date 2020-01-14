/*
    *   GameManager - Backbone of the game application
    *   Created by : Allan N. Murillo
 */
using System;
using UnityEngine;
using System.Collections;

namespace GameFramework.Core
{
    public sealed class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance
        {  get  {  if (_instance == null) { _instance = FindObjectOfType<GameManager>(); }  return _instance;  }  }
        
        [HideInInspector] public SceneTransitionManager sceneTransitionManager = null;
        public GameEvent onApplicationQuitEvent = null;

        private SaveSettings _saveSettings = null;
        private float _deltaTime = 0.0f;
        private bool _displayFps = false;


        private void Awake()
        {
            if (_instance != null && _instance != this) { Destroy(gameObject); return; }
            DontDestroyOnLoad(gameObject);
            _instance = this;
            
            _displayFps = true;
            
            GameSettingsManager.SettingsLoadedIni = false;
            _saveSettings = new SaveSettings();
            _saveSettings.Initialize();

            sceneTransitionManager = gameObject.GetComponentInChildren<SceneTransitionManager>();
            sceneTransitionManager.ScreenMaskBrightness = 0f;
            sceneTransitionManager.Initialize();
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

        #region Game Events
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
            if (onApplicationQuitEvent != null) { onApplicationQuitEvent.Raise(); }
            sceneTransitionManager.LoadCreditsScene();
        }

        private void OnDestroy()
        {
            if (Instance != this) return;
            if (onApplicationQuitEvent != null) { onApplicationQuitEvent.UnregisterAllListeners(); }
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }
        #endregion
    }
}