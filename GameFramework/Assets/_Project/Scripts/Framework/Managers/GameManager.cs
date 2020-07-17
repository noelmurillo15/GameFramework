/*
 * GameManager - Backbone of the game application
 * Contains data that needs to persist and be accessed from anywhere
 * Created by : Allan N. Murillo
 * Last Edited : 7/10/2020
 */

using System;
using ANM.Utils;
using UnityEngine;
using ANM.Framework.Audio;
using ANM.Framework.Events;
using ANM.Framework.Options;
using ANM.Framework.Extensions;
using AudioType = ANM.Framework.Audio.AudioType;

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
        [SerializeField] private int score = 0;
        [SerializeField] private bool displayFps = false;
        [SerializeField] private bool isGamePaused = false;
        [Space] [Header("Cursor")]
        [SerializeField] private Texture2D customCursor = null;

        private float _deltaTime;
        private SaveSettings _save;
        private FpsDisplay _fpsDisplay;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            SaveSettings.SettingsLoadedIni = false;
            Application.targetFrameRate = -1;
            DontDestroyOnLoad(gameObject);
            _save = new SaveSettings();
            _save.Initialize();
            Instance = this;
        }

        private void Start()
        {
            Cursor.visible = true;
            Invoke(nameof(Initialize), 2f);
            Cursor.lockState = CursorLockMode.None;
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
        }

        private void Initialize()
        {
#if UNITY_EDITOR
            AudioController.instance.PlayAudio(AudioType.St01);
            if (SceneExtension.IsThisSceneActive("Splash"))
                StartCoroutine(SceneExtension.ForceMenuSceneSequence());
#else
            AudioController.instance.PlayAudio(AudioType.St01, true, 1f);
            StartCoroutine(SceneExtension.ForceMenuSceneSequence());
#endif
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
            if (b) RaisePause();
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

        private void RaiseAppQuit()
        {
            onApplicationQuit.Raise();
        }

        private void SetScore(int amount)
        {
            score = amount;
        }

        private void LoadSettingsFromIndexedDb()
        {
            SaveSettings.SettingsLoadedIni = _save.LoadGameSettings();
        }

        public void HardReset()
        {
            SetScore(0);
            SetPause(false);
            RaiseAppQuit();
        }

        public void AttachFpsDisplay(FpsDisplay fps = null)
        {
            _fpsDisplay = fps;
            _fpsDisplay?.ToggleFpsDisplay(displayFps);
        }

        public void SetDisplayFps(bool b)
        {
            displayFps = b;
            _fpsDisplay?.ToggleFpsDisplay(b);
        }

        public void ReloadScene()
        {
            StartCoroutine(SceneExtension.ReloadCurrentSceneSequence());
        }

        public static void ReturnToMenu(MenuPageType pageToLoad)
        {
            SceneExtension.TrySwitchToScene(SceneExtension.MenuUiSceneName);
            SceneExtension.UnloadAllScenesExcept(SceneExtension.MenuUiSceneName);
            FindObjectOfType<MenuManager>().Reset();
            var controller = FindObjectOfType<MenuPageController>();
            controller.TurnMenuPageOff(controller.GetCurrentMenuPageType(), pageToLoad);
        }

        public void SaveGameSettings()
        {
            _save.SaveGameSettings();
        }

        public void IncreaseScore(int amount)
        {
            score += amount;
        }

        public void TogglePause()
        {
            SetPause(!isGamePaused);
        }

        public int GetScore()
        {
            return score;
        }


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
