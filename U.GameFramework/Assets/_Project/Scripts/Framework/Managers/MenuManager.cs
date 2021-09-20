/*
 * MenuManager - Handles interactions with the Menu Ui
 * Created by : Allan N. Murillo
 * Last Edited : 7/4/2021
 */

using UnityEngine;
using UnityEngine.UI;
using ANM.Framework.Input;
using ANM.Framework.Options;
using ANM.Framework.Extensions;
using UnityEngine.EventSystems;

namespace ANM.Framework.Managers
{
    public class MenuManager : MonoBehaviour
    {
        [Header("Menu Panels")]
        [SerializeField] private AudioOptionsPanel audioOptionsPanel = null;
        [SerializeField] private VideoOptionsPanel videoOptionsPanel = null;
        [SerializeField] private QuitOptionsPanel quitOptionsPanel = null;
        [SerializeField] private Button mainPanelSelectedObj = null;
        [SerializeField] private Button pausePanelSelectedObj = null;
        [SerializeField] private Button quitPanelSelectedObj = null;

        [Space] [Header("Local Game Info")]
        [SerializeField] private bool isSceneTransitioning = false;
        [SerializeField] private bool isMainMenuActive = false;
        [SerializeField] private int lastSceneBuildIndex = 0;

        private bool _isPaused;
        private Camera _menuUiCamera;
        private EventSystem _eventSystem;
        private InputController _inputController;
        private MenuPageController _controller;
        private GameManager _gameManager;


        #region Unity Funcs

        private void Awake()
        {
            isSceneTransitioning = true;
            _gameManager = GameManager.Instance;
            _eventSystem = FindObjectOfType<EventSystem>();
            _controller = FindObjectOfType<MenuPageController>();
            if (!SaveSettings.SettingsLoadedIni) SaveSettings.DefaultSettings();
        }

        private void Start()
        {
            ControllerSetup();
            _menuUiCamera = GetComponentInChildren<Camera>();
            isMainMenuActive = SceneExtension.IsThisSceneActive(SceneExtension.MenuUiSceneName);
            SceneExtension.FinishSceneLoadEvent += OnFinishLoadScene;
            SceneExtension.StartSceneLoadEvent += OnStartLoadScene;

            ApplyIniSettings();
            _isPaused = false;
        }

        private void OnGUI()
        {
            if (isMainMenuActive) return;
            var style = new GUIStyle();
            int w = Screen.width, h = Screen.height;
            h *= 2 / 100;
            var rect = new Rect(16, 16, w * 0.5f, 32);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = Color.white;
            var text = _isPaused
                ? "Press TAB to Resume"
                : "Press TAB to Pause";
            GUI.Label(rect, text, style);
        }

        private void OnDestroy()
        {
            _inputController.OnPauseToggleEvent -= OnTogglePauseEventCalled;
            _inputController.OnCancelEvent -= OnCancelEventCalled;
            SceneExtension.StartSceneLoadEvent -= OnStartLoadScene;
            SceneExtension.FinishSceneLoadEvent -= OnFinishLoadScene;
        }

        #endregion

        #region Public Funcs

        public void OnTogglePauseEventCalled()
        {
            if (isMainMenuActive || isSceneTransitioning) return;
            _gameManager.TogglePause();
        }

        public void Audio()
        {
            _controller.TurnMenuPageOff(_controller.GetCurrentMenuPageType(), MenuPageType.AudioSettings);
            audioOptionsPanel.AudioPanelIn(_eventSystem);
        }

        public void Video()
        {
            _controller.TurnMenuPageOff(_controller.GetCurrentMenuPageType(), MenuPageType.VideoSettings);
            videoOptionsPanel.VideoPanelIn(_eventSystem);
        }

        public void UpdateAudioSettings()
        {
            StartCoroutine(audioOptionsPanel.SaveAudioSettings());
            BackToPreviousPage();
        }

        public void UpdateVideoSettings()
        {
            StartCoroutine(videoOptionsPanel.SaveVideoSettings());
            BackToPreviousPage();
        }

        public void CancelAudioSettings()
        {
            StartCoroutine(audioOptionsPanel.RevertAudioSettings());
            BackToPreviousPage();
        }

        public void CancelVideoSettings()
        {
            StartCoroutine(videoOptionsPanel.RevertVideoSettings());
            BackToPreviousPage();
        }

        public void BackToPreviousPage()
        {
            var target = isMainMenuActive ? MenuPageType.MainMenu : MenuPageType.Pause;
            _controller.TurnMenuPageOff(_controller.GetCurrentMenuPageType(), target);
        }

        public void PlayAgain()
        {
            GameManager.HardReset();
            _controller.TurnMenuPageOff(_controller.GetCurrentMenuPageType(), MenuPageType.MainMenu);
        }

        public void Reset()
        {
            isMainMenuActive = true;
            _menuUiCamera.gameObject.SetActive(true);
        }

        public void ReturnToMenu()
        {
            if (isMainMenuActive) return;
            GameManager.HardReset();
            StartCoroutine(SceneExtension.ForceMenuSceneSequence(
                true, true, true, lastSceneBuildIndex));
            isMainMenuActive = true;
        }

        public void QuitOptions()
        {
            if (quitOptionsPanel == null || quitPanelSelectedObj == null) return;
            quitOptionsPanel.TurnOnPanel();
            if (_eventSystem == null) _eventSystem = EventSystem.current;
            _eventSystem.SetSelectedGameObject(quitPanelSelectedObj.gameObject);
            quitPanelSelectedObj.OnSelect(null);
        }

        public void QuitCancel()
        {
            if (quitOptionsPanel == null) return;
            quitOptionsPanel.TurnOffPanel();
            BackToPreviousPage();
        }

        public void Quit()
        {
            if (Time.timeScale <= 0) _gameManager.TogglePause();
            isMainMenuActive = true;
            GameManager.HardReset();
            GameManager.ReturnToMenu(MenuPageType.Credits);
        }

        #endregion

        #region Private Funcs

        private void ControllerSetup()
        {
            if (_inputController == null) _inputController = GameManager.GetResources().GetInput();
            _inputController.OnCancelEvent += OnCancelEventCalled;
            _inputController.OnPauseToggleEvent += OnTogglePauseEventCalled;
        }

        private void OnStartLoadScene(bool b1, bool b2)
        {
            _controller.TurnMenuPageOff(_controller.GetCurrentMenuPageType());
            _inputController.OnCancelEvent -= OnCancelEventCalled;
            isSceneTransitioning = true;
            if (Time.timeScale <= 0f) _gameManager.SoftReset();
            isMainMenuActive = false;
        }

        private void OnFinishLoadScene(bool b1, bool b2)
        {
            isMainMenuActive = SceneExtension.IsThisSceneActive(SceneExtension.MenuUiSceneName);
            if (isMainMenuActive)
            {
                BackToPreviousPage();
                _eventSystem.SetSelectedGameObject(mainPanelSelectedObj.gameObject);
                mainPanelSelectedObj.OnSelect(null);
            }
            else
            {
                _eventSystem.SetSelectedGameObject(pausePanelSelectedObj.gameObject);
                pausePanelSelectedObj.OnSelect(null);
            }

            _menuUiCamera.gameObject.SetActive(isMainMenuActive);
            isSceneTransitioning = false;
        }

        public void OnPause()
        {
            lastSceneBuildIndex = SceneExtension.GetCurrentSceneBuildIndex();
            if (!SceneExtension.TrySwitchToScene(SceneExtension.MenuUiSceneName)) return;
            _inputController.OnCancelEvent += OnCancelEventCalled;
            BackToPreviousPage();
            _isPaused = true;
        }

        public void OnResume()
        {
            if (!SceneExtension.TrySwitchToScene(lastSceneBuildIndex)) return;
            _inputController.OnCancelEvent -= OnCancelEventCalled;
            _controller.TurnMenuPageOff(_controller.GetCurrentMenuPageType());
            _isPaused = false;
        }

        private void ApplyIniSettings()
        {
            audioOptionsPanel.ApplyAudioSettings();
            videoOptionsPanel.ApplyVideoSettings();
        }

        private void OnCancelEventCalled() => BackToPreviousPage();

        #endregion
    }
}
