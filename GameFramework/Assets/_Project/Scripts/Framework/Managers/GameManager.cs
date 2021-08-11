/*
 * GameManager - Backbone of the game application
 * Contains data that needs to persist and be accessed from anywhere
 * Created by : Allan N. Murillo
 * Last Edited : 7/4/2021
 */

using System;
using UnityEngine;
using ANM.Framework.Audio;
using ANM.Framework.Utils;
using ANM.Framework.Events;
using ANM.Framework.Options;
using ANM.Framework.Extensions;
using ANM.Framework.Scriptables;
using AudioType = ANM.Framework.Audio.AudioType;

namespace ANM.Framework.Managers
{
    public sealed class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game Events")]
        [SerializeField] private GameEvent onGameResume = null;
        [SerializeField] private GameEvent onGamePause = null;

        [Space] [Header("Local Game Info")]
        [SerializeField] private int score = 0;
        [SerializeField] private bool displayFps = false;
        [SerializeField] private bool isGamePaused = false;
        [SerializeField] private bool debug = false;

        [Space] [Header("Cursor")]
        [SerializeField] private Texture2D customCursor = null;

        private SaveSettings _save;
        private FpsDisplay _fpsDisplay;
        private static ResourcesManager _resources;


        #region Unity Funcs

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
#else
            Debug.unityLogger.logEnabled = debug;
#endif

            Log("Awake");
            _resources = Resources.Load("ResourcesManager") as ResourcesManager;
            SaveSettings.SettingsLoadedIni = false;
            Application.targetFrameRate = -1;
            DontDestroyOnLoad(gameObject);
            _save = new SaveSettings();
            _save.Initialize();
            Instance = this;
        }

        private void Start()
        {
            Log("Start");
            Cursor.visible = true;
            _resources?.Initialize();
            Invoke(nameof(Initialize), 0.5f);
            Cursor.lockState = CursorLockMode.None;
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
        }

        private void OnDestroy()
        {
            if (Instance != this) return;
            Log("OnDestroy");
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        #endregion

        #region Public Funcs

        //  Used whenever the gameplay scene Starts
        public void AttachFpsDisplay(FpsDisplay fps = null)
        {
            _fpsDisplay = fps;
            _fpsDisplay?.ToggleFpsDisplay(displayFps);
            
            //  
            var splitScreenManager = FindObjectOfType<SplitScreenManager>();
            if (splitScreenManager == null) return;
            splitScreenManager.Initialize();
            splitScreenManager.EnableSplitScreen();
        }

        public void SetDisplayFps(bool b)
        {
            displayFps = b;
            _fpsDisplay?.ToggleFpsDisplay(b);
        }

        public void SaveGameSettings() => _save.SaveGameSettings();

        public void ReloadScene() => StartCoroutine(SceneExtension.ReloadCurrentSceneSequence());

        public void IncreaseScore(int amount) => score += amount;

        public void DecreaseScore(int amount) => Mathf.Clamp(score - amount, 0f, 9999f);

        public int GetScore() => score;

        public void TogglePause() => SetPause(!isGamePaused);

        public void SoftReset()
        {
            Log("SoftReset");
            SetScore(0);
            SetPause(false);
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
        }

        public static void HardReset()
        {
            //Debug.Log("[GM]: HardReset");
            //  TODO : Reset Game Resources here
        }

        public static void ReturnToMenu(MenuPageType pageToLoad)
        {
            //Debug.Log("[GM]: ReturnToMenu");
            SceneExtension.TrySwitchToScene(SceneExtension.MenuUiSceneName);
            SceneExtension.UnloadAllScenesExcept(SceneExtension.MenuUiSceneName);
            FindObjectOfType<MenuManager>().Reset();
            var controller = FindObjectOfType<MenuPageController>();
            controller.TurnMenuPageOff(controller.GetCurrentMenuPageType(), pageToLoad);
        }

        public static ResourcesManager GetResources() => _resources;

        #endregion

        #region Private Funcs

        private void Initialize()
        {
            Log("Initialize");
#if UNITY_EDITOR
            AudioController.Instance.PlayAudio(AudioType.St01, true, 1f);
#else
            AudioController.Instance.PlayAudio(AudioType.St01);
            StartCoroutine(SceneExtension.ForceMenuSceneSequence());
#endif
        }

        private void SetPause(bool b)
        {
            if (isGamePaused == b) return;
            if (b) RaisePause();
            else RaiseResume();
        }

        private void RaisePause()
        {
            if (isGamePaused) return;
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
            isGamePaused = true;
            onGamePause.Raise();
            Time.timeScale = 0;
        }

        private void RaiseResume()
        {
            if (!isGamePaused) return;
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
            Time.timeScale = 1;
            isGamePaused = false;
            onGameResume.Raise();
        }

        private void SetScore(int amount) => score = amount;

        private void Log(string log)
        {
            if (!debug) return;
            Debug.Log("[GM]: " + log);
        }

        #endregion
    }
}
