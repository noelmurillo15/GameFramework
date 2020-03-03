/*
 * GameManager - Backbone of the game application
 * Contains data that needs to persist and be accessed from anywhere
 * Created by : Allan N. Murillo
 * Last Edited : 3/3/2020
 */

using System;
using UnityEngine;
using ANM.Framework.Events;
using ANM.Framework.Extensions;
using ANM.Framework.Options;

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
        
        private float _deltaTime;
        private SaveSettings _save;


        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            SaveSettings.SettingsLoadedIni = false;
            Application.targetFrameRate = 60;
            DontDestroyOnLoad(gameObject);
            _save = new SaveSettings();
            _save.Initialize();
            Instance = this;
        }

        private void Start()
        {
            Invoke(nameof(Initialize), 1f);
        }

        private void Initialize()
        {
            if (SceneExtension.IsThisSceneActive(SceneExtension.SplashSceneName))
                StartCoroutine(SceneExtension.ForceMenuSceneSequence());
        }

        private void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
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

        private void OnDestroy()
        {
            if (Instance != this) return;
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }
        
        private void SetPause(bool b)
        {
            isGamePaused = b;
            if(b) RaisePause();
            else RaiseResume();
            Time.timeScale = b ? 0 : 1;
        }
        
        private void RaisePause() { onGamePause.Raise(); }

        private void RaiseResume() { onGameResume.Raise(); }
        
        private void RaiseAppQuit() { onApplicationQuit.Raise(); }

        
        public void HardReset()
        {
            SetPause(false);
            RaiseAppQuit();
        }
        
        public void SaveGameSettings()  {  _save.SaveGameSettings();  }
        
        public void SetDisplayFps(bool b)  {  displayFps = b;  }

        public void TogglePause() { SetPause(!isGamePaused); }
    }
}
