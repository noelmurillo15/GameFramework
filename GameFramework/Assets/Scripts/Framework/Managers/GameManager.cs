/*
 * GameManager - Backbone of the game application
 * Contains data that needs to persist and be accessed from anywhere
 * Created by : Allan N. Murillo
 * Last Edited : 3/3/2020
 */

using System;
using UnityEngine;
using ANM.Framework.Events;
using ANM.Framework.Options;
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

        private float _delta;
        private SaveSettings _save;
        private PlayerControls _controls;


        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            SaveSettings.SettingsLoadedIni = false;
            DontDestroyOnLoad(gameObject);
            _save = new SaveSettings();
            _save.Initialize();
            Instance = this;
        }

        private void Start()
        {
            ControlSetup();
            Invoke(nameof(Initialize), 3f);
        }

        private void Initialize()
        {
            if (SceneExtension.IsThisSceneActive(SceneExtension.SplashSceneName))
                StartCoroutine(SceneExtension.ForceMenuSceneSequence());
        }

        private void ControlSetup()
        {
            if(_controls == null) _controls = new PlayerControls();
            _controls.Player.PauseToggle.performed += context => TogglePause();
            _controls.Enable();
        }

        private void Update()
        {
            _delta += (Time.unscaledDeltaTime - _delta) * 0.1f;
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
            var msecs = _delta * 1000.0f;
            var fps = 1.0f / _delta;
            var text = $"{msecs:0.0} ms ({fps:0.} fps)";
            GUI.Label(rect, text, style);
        }

        private void OnDestroy()
        {
            if (Instance != this) return;
            Resources.UnloadUnusedAssets();
            _controls.Disable();
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
