/*
 * MenuManager - Handles interactions with the Menu Ui
 * Created by : Allan N. Murillo
 * Last Edited : 3/1/2020
 */

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ANM.Framework.Settings;
using ANM.Framework.Extensions;
using UnityEngine.EventSystems;

namespace ANM.Framework.Managers
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private bool isMainMenuActive = false;
        
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
        private IPanel[] _menuPanels;
        

        private void Awake()
        {
            _gameManager = GameManager.Instance;
            _eventSystem = FindObjectOfType<EventSystem>();
            if (!SaveSettings.SettingsLoadedIni) SaveSettings.DefaultSettings();
            ApplyIniSettings();
        }

        private void Start()
        {
            isMainMenuActive = SceneExtension.IsThisSceneActive(SceneExtension.MenuUiSceneName);
            _menuPanels = FindObjectsOfType<MonoBehaviour>().OfType<IPanel>().ToArray();
            SceneExtension.FinishSceneLoadEvent += OnFinishLoadScene;
            SceneExtension.StartSceneLoadEvent += OnStartLoadScene;
            TurnOffAllPanels();
            TurnOnMainPanel();
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
            var text = _gameManager.GetIsGamePaused() 
                ? "Press TAB to Resume" 
                : "Press TAB to Pause";
            GUI.Label(rect, text, style);
        }

        private void OnDestroy()
        {
            SceneExtension.StartSceneLoadEvent -= OnStartLoadScene;
            SceneExtension.FinishSceneLoadEvent -= OnFinishLoadScene;
        }

        private void OnStartLoadScene(bool b)
        {
            isMainMenuActive = false;
            TurnOffAllPanels();
        }
        
        private void OnFinishLoadScene(bool b)
        {
            isMainMenuActive = SceneExtension.IsThisSceneActive(SceneExtension.MenuUiSceneName);
            if (isMainMenuActive)
            {
                TurnOnMainPanel();
                _eventSystem.SetSelectedGameObject(mainPanelSelectedObj.gameObject);
                mainPanelSelectedObj.OnSelect(null);
            }
            else
            {
                _eventSystem.SetSelectedGameObject(pausePanelSelectedObj.gameObject);
                pausePanelSelectedObj.OnSelect(null);
            }
            menuUiCamera.gameObject.SetActive(isMainMenuActive);
        }
        
        private void OnPause()
        {
            if (!SceneExtension.TrySwitchToScene(SceneExtension.MenuUiSceneName)) return;
            TurnOnMainPanel();
        }
        
        private void OnResume()
        {
            if (!SceneExtension.TrySwitchToScene(SceneExtension.GameplaySceneName)) return;
            menuUiCamera.gameObject.SetActive(false);
            TurnOffAllPanels();
        }
        
        private void TurnOnMainPanel()
        {
            if (isMainMenuActive) mainPanel.SetActive(true);
            else pausePanel.SetActive(true);
        }
        
        private void TurnOffAllPanels()
        {
            mainPanel.SetActive(false);
            pausePanel.SetActive(false);
            foreach (var panel in _menuPanels)
            {
                panel.TurnOffPanel();
            }
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

        public void Pause() { _gameManager.SetPause(true); }
        
        public void Resume() { _gameManager.SetPause(false); }
        
        public void ReturnToMenu()
        {
            if (isMainMenuActive){ QuitCancel(); return;}
            TurnOffAllPanels();
            _gameManager.HardReset();
            StartCoroutine(SceneExtension.ForceMenuSceneSequence(true));
            _eventSystem.SetSelectedGameObject(mainPanelSelectedObj.gameObject);
            mainPanelSelectedObj.OnSelect(null);
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
        
        public void ToggleFullscreen(bool b)
        {
            if (b) ToggleFullscreen();
            else ExitFullscreen();
        }

        public void ToggleFpsDisplay(bool b)
        {
            GameManager.Instance.SetDisplayFps(b);
            EventExtension.MuteEventListener(fpsDisplayToggle.onValueChanged);
            fpsDisplayToggle.isOn = b;
            EventExtension.UnMuteEventListener(fpsDisplayToggle.onValueChanged);
            SaveSettings.DisplayFpsIni = b;
        }
        
        public void QuitOptions()
        {
            videoSettingsPanel.TurnOffPanel();
            audioSettingsPanel.TurnOffPanel();
            quitPanel.TurnOnPanel();
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
            StartCoroutine(SceneExtension.LoadSingleSceneSequence(
                SceneExtension.CreditsSceneName, true));
        }

        #region External JS LIBRARY
#if UNITY_WEBGL && !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("__Internal")]
        static extern void WindowFullscreen();
        [System.Runtime.InteropServices.DllImport("__Internal")]
        static extern void CancelFullscreen();

        private void ToggleFullscreen() { WindowFullscreen(); }
        private void ExitFullscreen() { CancelFullscreen(); }
#else
        private static void ToggleFullscreen() { }
        private static void ExitFullscreen() { }
#endif
        #endregion
    }
}
