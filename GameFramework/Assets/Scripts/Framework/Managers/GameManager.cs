/*
 * GameManager - Backbone of the game application
 * Contains data that needs to persist and be accessed from anywhere
 * Created by : Allan N. Murillo
 * Last Edited : 3/4/2020
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
        
        private float _deltaTime;
        private SaveSettings _save;


        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            SaveSettings.SettingsLoadedIni = false;
            Application.targetFrameRate = -1;
            DontDestroyOnLoad(gameObject);
            _save = new SaveSettings();
            _save.Initialize();
            Instance = this;
        }

        private void Start()
        {
            Invoke(nameof(Initialize), 2f);
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


        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus) return;
            WindowLostFocus();
        }

        private void OnDestroy()
        {
            if (Instance != this) return;
            Resources.UnloadUnusedAssets();
            GC.Collect();
            Quit();
        }
        
        private void SetPause(bool b)
        {
            if (isGamePaused == b) return;
            if(b) RaisePause();
            else RaiseResume();
        }

        private void RaisePause()
        {
            Time.timeScale = 0;
            isGamePaused = true;
            onGamePause.Raise();
        }

        private void RaiseResume()
        {
            Time.timeScale = 1;
            isGamePaused = false;
            onGameResume.Raise();
        }
        
        private void RaiseAppQuit() { onApplicationQuit.Raise(); }

        private void LoadSettingsFromIndexedDb()
        {    //    WebGL Only : Called from WebBrowserInteraction.jslib plugin
            SaveSettings.SettingsLoadedIni = _save.LoadGameSettings();
        }

        
        public void HardReset()
        {
            SetPause(false);
            RaiseAppQuit();
        }
        
        public void SaveGameSettings()  {  _save.SaveGameSettings();  }
        
        public void SetDisplayFps(bool b)  {  displayFps = b;  }

        public void TogglePause() { SetPause(!isGamePaused); }

        
        #region External JS Lib
#if UNITY_WEBGL && !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("__Internal")]
        static extern void QuitGame();
        [System.Runtime.InteropServices.DllImport("__Internal")]
        static extern void LostFocus();
        
        private void WindowLostFocus(){
            LostFocus();
        }
        private void Quit(){
            QuitGame();
        }
#else
        private static void WindowLostFocus() { }
        private static void Quit() { }
#endif
        #endregion
    }
}
