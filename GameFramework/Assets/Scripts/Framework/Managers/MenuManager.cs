/*
 * MenuManager - Handles interactions with the Menu Ui
 * Created by : Allan N. Murillo
 * Last Edited : 2/24/2020
 */

using UnityEngine;
using UnityEngine.UI;
using ANM.Framework.Settings;
using ANM.Framework.Extensions;
using UnityEngine.EventSystems;

namespace ANM.Framework.Managers
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainPanel = null;
        [SerializeField] private GameObject pausePanel = null;
        
        [SerializeField] private AudioSettingsPanel audioSettingsPanel = null;
        [SerializeField] private VideoSettingsPanel videoSettingsPanel = null;
        [SerializeField] private QuitOptionsPanel quitPanel = null;
        
        [SerializeField] private Toggle fpsDisplayToggle = null;
        [SerializeField] private Camera menuUiCamera = null;
        
        [SerializeField] private Button mainPanelSelectedObj = null;
        [SerializeField] private Button pausePanelSelectedObj = null;
        [SerializeField] private Button quitPanelSelectedObj = null;

        private EventSystem _eventSystem;
        private GameManager _gameManager;
        

        private void Awake()
        {
            _gameManager = GameManager.Instance;
            _eventSystem = FindObjectOfType<EventSystem>();
            if (!SaveSettings.SettingsLoadedIni) SaveSettings.DefaultSettings();
            ApplyIniSettings();
        }

        private void Start()
        {
            var isMenuActive = SceneExtension.IsThisSceneActive(SceneExtension.MenuUiSceneName);
            mainPanel.SetActive(isMenuActive);
            _gameManager.SetIsMainMenuActive(isMenuActive);
            menuUiCamera.gameObject.SetActive(isMenuActive);
            SceneExtension.StartSceneLoadEvent += OnStartLoadSceneEvent;
            SceneExtension.FinishSceneLoadEvent += OnFinishLoadSceneEvent;
        }

        private void OnGUI()
        {
            if (_gameManager.GetIsMainMenuActive()) return;
            var style = new GUIStyle();
            int w = Screen.width, h = Screen.height;
            h *= 2 / 100;
            var rect = new Rect(16, 16, w * 0.5f, 32);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = Color.white;
            var text = _gameManager.GetIsGamePaused() ? "Press TAB to Resume" : "Press TAB to Pause";
            GUI.Label(rect, text, style);
        }
        
        private void OnDestroy()
        {
            SceneExtension.StartSceneLoadEvent -= OnStartLoadSceneEvent;
            SceneExtension.FinishSceneLoadEvent -= OnFinishLoadSceneEvent;
        }
        
        private void OnStartLoadSceneEvent(bool b)
        {
            TurnOffAllPanels();
        }
        
        private void OnFinishLoadSceneEvent(bool b)
        {
            if (SceneExtension.IsThisSceneActive(SceneExtension.MenuUiSceneName))
            {
                _gameManager.SetIsMainMenuActive(true);
                TurnOnMainPanel();
            }
            else
            {
                _gameManager.SetIsMainMenuActive(false);
                menuUiCamera.gameObject.SetActive(false);
                TurnOffAllPanels();
            }
        }
        
        private void OnPauseEvent()
        {
            if (!SceneExtension.TrySwitchToScene(SceneExtension.MenuUiSceneName)) return;
            TurnOnMainPanel();
        }
        
        private void OnResumeEvent()
        {
            if (!SceneExtension.TrySwitchToScene(SceneExtension.GameplaySceneName)) return;
            menuUiCamera.gameObject.SetActive(false);
            TurnOffAllPanels();
        }
        
        private void TurnOnMainPanel()
        {
            menuUiCamera.gameObject.SetActive(true);
            if (_gameManager.GetIsMainMenuActive())
            {
                mainPanel.SetActive(true);
                _eventSystem.SetSelectedGameObject(mainPanelSelectedObj.gameObject);
                mainPanelSelectedObj.OnSelect(null);
            }
            else
            {
                pausePanel.SetActive(true);
                _eventSystem.SetSelectedGameObject(pausePanelSelectedObj.gameObject);
                pausePanelSelectedObj.OnSelect(null);
            }
            videoSettingsPanel.TurnOffPanel();
            audioSettingsPanel.TurnOffPanel();
        }
        
        private void TurnOffAllPanels()
        {
            quitPanel.TurnOffPanel();
            videoSettingsPanel.TurnOffPanel();
            mainPanel.SetActive(false);
            pausePanel.SetActive(false);
            audioSettingsPanel.TurnOffPanel();
        }

        private void ApplyIniSettings()
        {
            ToggleFpsDisplay(SaveSettings.DisplayFpsIni);
            audioSettingsPanel.ApplyAudioSettings();
            videoSettingsPanel.ApplyVideoSettings();
        }
        
        public void StartGame()
        {
            StartCoroutine(SceneExtension.LoadMultiSceneSequence(
                SceneExtension.GameplaySceneName, true));
        }

        public void Pause()
        {
            _gameManager.SetIsGamePaused(true);
        }
        
        public void Resume()
        {
            _gameManager.SetIsGamePaused(false);
        }

        public void ReturnToMenu()
        {
            menuUiCamera.gameObject.SetActive(true);
            TurnOffAllPanels();
            _gameManager.HardReset();
            mainPanel.SetActive(true);
            _gameManager.SetIsMainMenuActive(true);
            SceneExtension.TrySwitchToScene(SceneExtension.MenuUiSceneName);
            SceneExtension.UnloadAllScenesExcept(SceneExtension.MenuUiSceneName);
            _eventSystem.SetSelectedGameObject(mainPanelSelectedObj.gameObject);
            mainPanelSelectedObj.OnSelect(null);
        }
        
        public void QuitOptions()
        {
            quitPanel.TurnOnPanel();
            videoSettingsPanel.TurnOffPanel();
            audioSettingsPanel.TurnOffPanel();
            _eventSystem.SetSelectedGameObject(quitPanelSelectedObj.gameObject);
            quitPanelSelectedObj.OnSelect(null);
        }

        public void QuitCancel()
        {
            quitPanel.TurnOffPanel();
            TurnOnMainPanel();
        }
        
        public void QuitGame()
        {
            _gameManager.HardReset();
            StartCoroutine(SceneExtension.LoadSingleSceneSequence(SceneExtension.CreditsSceneName, true));
        }

        public void Audio()
        {
            TurnOffAllPanels();
            audioSettingsPanel.AudioPanelIn(_eventSystem);
        }
        
        public void Video()
        {
            TurnOffAllPanels();
            videoSettingsPanel.VideoPanelIn(_eventSystem);
        }
        
        public void UpdateAudioSettings()
        {
            StartCoroutine(audioSettingsPanel.SaveAudioSettings());
            TurnOnMainPanel();
        }
        
        public void UpdateVideoSettings()
        {
            StartCoroutine(videoSettingsPanel.SaveVideoSettings());
            TurnOnMainPanel();
        }

        public void CancelAudioSettings()
        {
            StartCoroutine(audioSettingsPanel.RevertAudioSettings());
            TurnOnMainPanel();
        }

        public void CancelVideoSettings()
        {
            StartCoroutine(videoSettingsPanel.RevertVideoSettings());
            TurnOnMainPanel();
        }

        public void ToggleFpsDisplay(bool b)
        {
            GameManager.Instance.SetDisplayFps(b);
            EventExtension.MuteEventListener(fpsDisplayToggle.onValueChanged);
            fpsDisplayToggle.isOn = b;
            EventExtension.UnMuteEventListener(fpsDisplayToggle.onValueChanged);
            SaveSettings.DisplayFpsIni = b;
        }
    }
}
