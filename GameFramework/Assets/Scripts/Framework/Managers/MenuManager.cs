﻿/*
 * MenuManager - Handles interactions with the Menu Ui
 * Created by : Allan N. Murillo
 * Last Edited : 3/4/2020
 */

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ANM.Framework.Options;
using ANM.Framework.Extensions;
using UnityEngine.EventSystems;

namespace ANM.Framework.Managers
{
    public class MenuManager : MonoBehaviour
    {
        [Header("Menu Panels")]
        [SerializeField] private GameObject mainPanel = null;
        [SerializeField] private GameObject pausePanel = null;
        [SerializeField] private AudioOptionsPanel audioOptionsPanel = null;
        [SerializeField] private VideoOptionsPanel videoOptionsPanel = null;
        [SerializeField] private QuitOptionsPanel quitOptionsPanel = null;

        [Space]
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
        private GameManager _gameManager;
        private IPanel[] _menuPanels;
        

        private void Awake()
        {
            isSceneTransitioning = true;
            _gameManager = GameManager.Instance;
            _eventSystem = FindObjectOfType<EventSystem>();
            if (!SaveSettings.SettingsLoadedIni) SaveSettings.DefaultSettings();
        }

        private void Start()
        {
            _menuUiCamera = GetComponentInChildren<Camera>();
            isMainMenuActive = SceneExtension.IsThisSceneActive(SceneExtension.MenuUiSceneName);
            _menuPanels = FindObjectsOfType<MonoBehaviour>().OfType<IPanel>().ToArray();
            SceneExtension.FinishSceneLoadEvent += OnFinishLoadScene;
            SceneExtension.StartSceneLoadEvent += OnStartLoadScene;
            ApplyIniSettings();
            TurnOffAllPanels();
            TurnOnMainPanel();
            _isPaused = false;
        }

        private void Update()
        {
            if (isMainMenuActive || isSceneTransitioning) return;
            if (!Input.GetKeyDown(KeyCode.Tab)) return;
            TogglePause();
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
            SceneExtension.StartSceneLoadEvent -= OnStartLoadScene;
            SceneExtension.FinishSceneLoadEvent -= OnFinishLoadScene;
        }

        private void OnStartLoadScene(bool b1, bool b2)
        {
            isSceneTransitioning = true;
            isMainMenuActive = false;
            TurnOffAllPanels();
        }
        
        private void OnFinishLoadScene(bool b1, bool b2)
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
            _menuUiCamera.gameObject.SetActive(isMainMenuActive);
            isSceneTransitioning = false;
        }
        
        private void OnPause()
        {
            lastSceneBuildIndex = SceneExtension.GetCurrentSceneBuildIndex();
            if (!SceneExtension.TrySwitchToScene(SceneExtension.MenuUiSceneName)) return;
            TurnOnMainPanel();
            _isPaused = true;
        }
        
        private void OnResume()
        {
            if (!SceneExtension.TrySwitchToScene(lastSceneBuildIndex)) return;
            _menuUiCamera.gameObject.SetActive(false);
            TurnOffAllPanels();
            _isPaused = false;
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
            audioOptionsPanel.ApplyAudioSettings();
            videoOptionsPanel.ApplyVideoSettings();
        }
        
        private void LoadCredits()
        {
            StartCoroutine(SceneExtension.LoadSingleSceneSequence(
                SceneExtension.CreditsSceneName, true));
        }
        
        
        public void StartGame()
        {
            StartCoroutine(SceneExtension.LoadMultiSceneSequence(
                SceneExtension.GameplaySceneName, true));
        }

        public void TogglePause()
        {
            _gameManager.TogglePause();
        }
        
        public void ReturnToMenu()
        {
            if (isMainMenuActive){ QuitCancel(); return;}
            TurnOffAllPanels();
            _gameManager.HardReset();
            StartCoroutine(SceneExtension.ForceMenuSceneSequence(true, true, true, lastSceneBuildIndex));
            _eventSystem.SetSelectedGameObject(mainPanelSelectedObj.gameObject);
            mainPanelSelectedObj.OnSelect(null);
        }
        
        public void QuitOptions()
        {
            videoOptionsPanel.TurnOffPanel();
            audioOptionsPanel.TurnOffPanel();
            quitOptionsPanel.TurnOnPanel();
            _eventSystem.SetSelectedGameObject(quitPanelSelectedObj.gameObject);
            quitPanelSelectedObj.OnSelect(null);
        }

        public void QuitCancel()
        {
            quitOptionsPanel.TurnOffPanel();
            TurnOnMainPanel();
        }
        
        public void QuitGame()
        {
            _gameManager.HardReset();
            Invoke(nameof(LoadCredits), 0.15f);
        }

        public void Audio()
        {
            TurnOffAllPanels();
            audioOptionsPanel.AudioPanelIn(_eventSystem);
        }
        
        public void Video()
        {
            TurnOffAllPanels();
            videoOptionsPanel.VideoPanelIn(_eventSystem);
        }
        
        public void UpdateAudioSettings()
        {
            StartCoroutine(audioOptionsPanel.SaveAudioSettings());
            TurnOnMainPanel();
        }
        
        public void UpdateVideoSettings()
        {
            StartCoroutine(videoOptionsPanel.SaveVideoSettings());
            TurnOnMainPanel();
        }

        public void CancelAudioSettings()
        {
            StartCoroutine(audioOptionsPanel.RevertAudioSettings());
            TurnOnMainPanel();
        }

        public void CancelVideoSettings()
        {
            StartCoroutine(videoOptionsPanel.RevertVideoSettings());
            TurnOnMainPanel();
        }
    }
}
