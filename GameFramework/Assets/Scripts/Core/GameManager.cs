/*
    *   GameManager - Backbone of the game application
    *   Created by : Allan N. Murillo
 */
using UnityEngine;


namespace GameFramework.Core
{
    public class GameManager : MonoBehaviour
    {
        #region GameManager Class Members
        static GameManager _instance;
        public static GameManager Instance { get { return _instance; } }

        public PauseManager pauseManager = null;
        public ScreenFader screenFader = null;
        public SaveSettings saveSettings = null;

        #region Events
        public delegate void GeneralEvent();
        public event GeneralEvent OnInventoryToggle;
        public event GeneralEvent OnRestartLevel;
        public event GeneralEvent OnMenuToggle;
        public event GeneralEvent OnStartGame;
        public event GeneralEvent OnEndGame;
        public event GeneralEvent OnQuitApp;
        #endregion

        float deltaTime = 0.0f;
        bool displayFPS = false;

        bool isGameOver = false;
        bool isInventoryActive = false;
        bool isMenuActive = false;
        bool isPauseActive = false;

        public bool IsInventoryUIActive { get { return isInventoryActive; } set { isInventoryActive = value; } }
        public bool IsPauseUIActive { get { return isPauseActive; } set { isPauseActive = value; } }
        public bool IsMenuUIActive { get { return isMenuActive; } set { isMenuActive = value; } }
        public bool IsGameOver { get { return isGameOver; } set { isGameOver = value; } }
        #endregion


        void Awake()
        {
            if (_instance != null && _instance != this) Destroy(gameObject);
            else _instance = this;

            //  Default Screen Size
            Screen.SetResolution(1920, 1080, true, 60);

            // Save Settings
            saveSettings = new SaveSettings();

            //  Register Event Listeners
            OnQuitApp += Quit;

            //  Enable Flags
            displayFPS = true;

            //  Persist throughout scenes
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            if (displayFPS) { deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f; }
        }

        void OnGUI()
        {
            if (displayFPS)
            {
                int w = Screen.width, h = Screen.height;

                GUIStyle style = new GUIStyle();

                Rect rect = new Rect(w - 180, 0, w, h * 2 / 100);
                style.alignment = TextAnchor.UpperLeft;
                style.fontSize = h * 2 / 100;
                style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                float msec = deltaTime * 1000.0f;
                float fps = 1.0f / deltaTime;
                string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
                GUI.Label(rect, text, style);
            }
        }

        #region Game Settings
        public bool LoadSettings(bool full = false)
        {
            return saveSettings.LoadFromJson(full);
        }

        public void SaveSettings()
        {
            saveSettings.SaveToJson();
        }
        #endregion

        #region Events
        public void StartGameEvent()
        {
            if (OnStartGame != null)
            {
                isMenuActive = false;
                isGameOver = false;
                OnStartGame();
            }
        }
        public void ToggleInventoryEvent()
        {
            if (OnInventoryToggle != null)
            {
                OnInventoryToggle();
            }
        }
        public void ToggleMenuEvent()
        {
            if (OnMenuToggle != null)
            {
                OnMenuToggle();
            }
        }
        public void RestartLevelEvent()
        {
            if (OnRestartLevel != null)
            {
                OnRestartLevel();
            }
        }
        public void EndGameEvent()
        {
            if (OnEndGame != null)
            {
                if (!isGameOver)
                {
                    isMenuActive = true;
                    isGameOver = true;
                    OnEndGame();
                }
            }
        }
        public void QuitApplicationEvent()
        {
            if (OnQuitApp != null)
            {
                OnQuitApp();
            }
        }
        #endregion

        void Quit()
        {
            OnQuitApp -= Quit;
        }
    }
}