/*
 * GameSettingsManager - Manages GameSettings Ui & Functionality
 * Created by : Allan N. Murillo
 */

using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace ANM.Framework
{
    public class MenuUiManager : MonoBehaviour
    {
        [Header("Required Components")]
        public GameEvent onGamePauseEvent;
        public GameEvent onGameResumeEvent;
        
        [SerializeField] private GameObject mainPanel = null;
        [SerializeField] private GameObject videoPanel = null;
        [SerializeField] private GameObject audioPanel = null;
        [SerializeField] private GameObject quitOptionsPanel = null;
        [SerializeField] private GameObject pausePanel = null;

        [SerializeField] private Animator audioPanelAnimator = null;
        [SerializeField] private Animator quitOptionsPanelAnimator = null;
        [SerializeField] private Animator videoPanelAnimator = null;

        [SerializeField] private Camera menuUiCamera = null;

        [SerializeField] private TMP_Dropdown aaCombo = null;
        [SerializeField] private TMP_Dropdown afCombo = null;
        [SerializeField] private Slider renderDistSlider = null;
        [SerializeField] private Slider shadowDistSlider = null;
        [SerializeField] private Slider audioMasterVolumeSlider = null;
        [SerializeField] private Slider effectsVolumeSlider = null;
        [SerializeField] private Slider backgroundVolumeSlider = null;
        [SerializeField] private Slider masterTexSlider = null;
        [SerializeField] private Slider shadowCascadesSlider = null;
        [SerializeField] private Toggle vSyncToggle = null;
        [SerializeField] private TMP_Text presetLabel = null;
        [SerializeField] private float[] shadowDist = null;
        [SerializeField] private AudioSource bgMusic = null;
        [SerializeField] private AudioSource[] sfx = null;

        [SerializeField] private Button mainPanelSelectedObj = null;
        [SerializeField] private Button pausePanelSelectedObj = null;
        [SerializeField] private Button audioPanelSelectedObj = null;
        [SerializeField] private Button videoPanelSelectedObj = null;
        [SerializeField] private Button quitPanelSelectedObj = null;

        private PlayerControls _controls;
        private EventSystem _eventSystem;
        private GameManager _gameManager;
        private string[] _presets;
        

        private void Awake()
        {
            ControllerSetup();
            _eventSystem = FindObjectOfType<EventSystem>();
            _presets = QualitySettings.names;
            if (!SaveSettings.SettingsLoadedIni) { DefaultSettings(); }
            ApplyIniSettings();
        }

        private void Start()
        {
            _gameManager = GameManager.Instance;
            _gameManager.SwitchToLoadedScene("Level 1");
            _gameManager.SetIsMainMenuActive(SceneTransitionManager.IsMainMenuActive());

            var isMainMenu = _gameManager.GetIsMainMenuActive();
            mainPanel.SetActive(isMainMenu);
            menuUiCamera.gameObject.SetActive(isMainMenu);

            UpdateSelectedObject(isMainMenu ? 
                mainPanelSelectedObj : pausePanelSelectedObj);
            
            pausePanel.SetActive(false);
            videoPanel.SetActive(false);
            audioPanel.SetActive(false);
            _gameManager.SetIsGamePaused(false);
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
            var text = _gameManager.GetIsGamePaused() ? 
                "Press Tab or Xbox Start to Resume" : "Press Tab or Xbox Start to Pause";
            GUI.Label(rect, text, style);
        }

        private void ControllerSetup()
        {
            if(_controls == null)
                _controls = new PlayerControls();

            _controls.Player.PauseToggle.performed += context =>
            {
                if (GameManager.Instance.GetIsGamePaused()) onGameResumeEvent.Raise();
                else onGamePauseEvent.Raise();
            };
            
            _controls.Enable();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        #region Menu UI Interactions
        public void StartGame()
        {
            _gameManager.StartGameEvent();
        }

        public void Pause()
        {
            onGamePauseEvent.Raise();
        }

        public void OnPauseEvent()
        {
            menuUiCamera.gameObject.SetActive(true);
            TurnOnMainPanel();
        }

        public void Resume()
        {
            onGameResumeEvent.Raise();
        }

        public void OnResumeEvent()
        {
            menuUiCamera.gameObject.SetActive(false);
            TurnOffAllPanels();
        }

        public void QuitOptions()
        {
            videoPanel.SetActive(false);
            audioPanel.SetActive(false);
            if (quitOptionsPanelAnimator != null)
            {
                quitOptionsPanelAnimator.enabled = true;
                quitOptionsPanelAnimator.Play("QuitPanelIn");
            }
            else
            {
                quitOptionsPanel.SetActive(true);
            }
            UpdateSelectedObject(quitPanelSelectedObj);
        }

        public void QuitCancel()
        {
            if (quitOptionsPanelAnimator != null)
                quitOptionsPanelAnimator.Play("QuitPanelOut");
            else
                quitOptionsPanel.SetActive(false);
            
            UpdateSelectedObject(_gameManager.GetIsMainMenuActive() ?
                mainPanelSelectedObj : pausePanelSelectedObj);
        }

        public void ReturnToMenu()
        {
            menuUiCamera.gameObject.SetActive(true);
            videoPanel.SetActive(false);
            audioPanel.SetActive(false);
            pausePanel.SetActive(false);
            mainPanel.SetActive(true);
            _gameManager.Reset();
            _gameManager.SetIsMainMenuActive(true);
            _gameManager.UnloadScenesExceptMenu();
            UpdateSelectedObject(mainPanelSelectedObj);
        }

        public void StartLoadSceneEvent()
        {    //    Handled by onStartSceneTransition ScriptableObject
            if (SceneTransitionManager.IsMainMenuActive())
            {
                menuUiCamera.gameObject.SetActive(false);
            }
        }
        
        public void FinishLoadSceneEvent()
        {    //    Handled by onFinishSceneTransition ScriptableObject
            if (SceneTransitionManager.IsMainMenuActive())
            {
                menuUiCamera.gameObject.SetActive(true);
                _gameManager.SetIsMainMenuActive(true);
                TurnOnMainPanel();
            }
            else
            {
                _gameManager.SetIsMainMenuActive(false);
                TurnOffAllPanels();
            }
        }

        public void QuitGame()
        {
            onGameResumeEvent.Raise();
            //_gameManager.onApplicationQuitEvent.Raise();
            //_gameManager.UnloadScenesExceptMenu();
            _gameManager.LoadCredits();
        }
        
        private void TurnOffAllPanels()
        {
            videoPanel.SetActive(false);
            mainPanel.SetActive(false);
            pausePanel.SetActive(false);
            audioPanel.SetActive(false);
        }
        
        private void TurnOnMainPanel()
        {
            var isMainMenu = _gameManager.GetIsMainMenuActive();
            
            if (isMainMenu)
                mainPanel.SetActive(true);
            else
                pausePanel.SetActive(true);
            
            UpdateSelectedObject(isMainMenu ?
                mainPanelSelectedObj : pausePanelSelectedObj);
            
            videoPanel.SetActive(false);
            audioPanel.SetActive(false);
        }
        
        private void UpdateSelectedObject(Button obj)
        {
            _eventSystem.SetSelectedGameObject(null);
            _eventSystem.SetSelectedGameObject(obj.gameObject);
            
            obj.Select();
            obj.OnSelect(null);
        }
        #endregion

        #region Audio Panel
        public void Audio()
        {
            AudioPanelIn();
            TurnOffAllPanels();
        }

        public void UpdateAudioSettings()
        {
            StartCoroutine(SaveAudioSettings());
        }

        public void CancelAudioSettings()
        {
            StartCoroutine(RevertAudioSettings());
        }

        private void AudioPanelIn()
        {
            if (audioPanelAnimator == null) return;
            audioPanelAnimator.enabled = true;
            audioPanelAnimator.Play("Audio Panel In");
            UpdateSelectedObject(audioPanelSelectedObj);
        }
        
        private IEnumerator SaveAudioSettings()
        {
            audioPanelAnimator.Play("Audio Panel Out");

            SaveSettings.MasterVolumeIni = audioMasterVolumeSlider.value;
            SaveSettings.EffectVolumeIni = effectsVolumeSlider.value;
            SaveSettings.BackgroundVolumeIni = backgroundVolumeSlider.value;

            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.5f));
            _gameManager.SaveGameSettings();
            TurnOnMainPanel();
        }

        private IEnumerator RevertAudioSettings()
        {
            audioPanelAnimator.Play("Audio Panel Out");

            audioMasterVolumeSlider.value = SaveSettings.MasterVolumeIni;
            effectsVolumeSlider.value = SaveSettings.EffectVolumeIni;
            backgroundVolumeSlider.value = SaveSettings.BackgroundVolumeIni;
            
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.5f));
            TurnOnMainPanel();
        }
        #endregion

        #region Video Panel
        public void Video()
        {
            VideoPanelIn();
            TurnOffAllPanels();
        }
        
        public void UpdateVideoSettings()
        {
            StartCoroutine(SaveVideoSettings());
        }

        public void CancelVideoSettings()
        {
            StartCoroutine(RevertVideoSettings());
        }

        private void VideoPanelIn()
        {
            if (videoPanelAnimator == null) return;
            videoPanelAnimator.enabled = true;
            videoPanelAnimator.Play("Video Panel In");
            UpdateSelectedObject(videoPanelSelectedObj);
        }
        
        private IEnumerator SaveVideoSettings()
        {
            videoPanelAnimator.Play("Video Panel Out");

            SaveSettings.CurrentQualityLevelIni = QualitySettings.GetQualityLevel();
            SaveSettings.VsyncIni = vSyncToggle.isOn;
            SaveSettings.MsaaIni = aaCombo.value;
            SaveSettings.AnisoFilterLevelIni = afCombo.value;
            SaveSettings.RenderDistIni = renderDistSlider.value;
            SaveSettings.TextureLimitIni = (int)masterTexSlider.value;
            SaveSettings.ShadowDistIni = shadowDistSlider.value;
            SaveSettings.ShadowCascadeIni = (int)shadowCascadesSlider.value;

            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.5f));
            _gameManager.SaveGameSettings();
            TurnOnMainPanel();
        }

        private IEnumerator RevertVideoSettings()
        {
            videoPanelAnimator.Play("Video Panel Out");

            QualitySettings.SetQualityLevel(SaveSettings.CurrentQualityLevelIni);
            vSyncToggle.isOn = SaveSettings.VsyncIni;
            aaCombo.value = SaveSettings.MsaaIni;
            afCombo.value = SaveSettings.AnisoFilterLevelIni;
            renderDistSlider.value = SaveSettings.RenderDistIni;
            masterTexSlider.value = SaveSettings.TextureLimitIni;
            shadowDistSlider.value = SaveSettings.ShadowDistIni;
            shadowCascadesSlider.value = SaveSettings.ShadowCascadeIni;

            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.5f));
            TurnOnMainPanel();
        }
        #endregion
        
        #region Game Setting Events
        public void UpdateMasterVol(float f)
        {
            AudioListener.volume = f;
        }
        
        public void UpdateEffectsVol(float f)
        {
            foreach (var effect in sfx)
            {
                effect.volume = f;
            }
        }
        
        public void UpdateBackgroundVol(float f)
        {
            if(bgMusic != null)
                bgMusic.volume = f;
        }

        public void ToggleVSync(bool b)
        {
            QualitySettings.vSyncCount = b ? 1 : 0;
        }

        public void UpdateRenderDist(float f)
        {
            try
            {
                menuUiCamera.farClipPlane = f;
            }
            catch
            {
                menuUiCamera = Camera.main;
                menuUiCamera.farClipPlane = f;
            }
        }

        public void UpdateTex(float textureQuality)
        {
            var f = Mathf.RoundToInt(textureQuality);
            QualitySettings.masterTextureLimit = f;
        }

        public void UpdateShadowDistance(float dist)
        {
            QualitySettings.shadowDistance = dist;
        }

        private static void ForceOnANISO()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        }

        private static void PerTextureAniso()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
        }

        private static void DisableAniso()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
        }

        public void UpdateAniso(int anisoSetting)
        {
            switch (anisoSetting)
            {
                case 0:
                    DisableAniso();
                    break;
                case 1:
                    PerTextureAniso();
                    break;
                case 2:
                    ForceOnANISO();
                    break;
            }
        }

        public void UpdateCascades(float cascades)
        {
            var c = Mathf.RoundToInt(cascades);
            switch (c)
            {
                case 1:
                case 3:
                    c = 2;
                    break;
            }
            QualitySettings.shadowCascades = c;
        }

        public void UpdateMsaa(int msaaAmount)
        {
            switch (msaaAmount)
            {
                case 0:
                    DisableMsaa();
                    break;
                case 1:
                    TwoMsaa();
                    break;
                case 2:
                    FourMsaa();
                    break;
                case 3:
                    EightMsaa();
                    break;
            }
        }

        private static void DisableMsaa()
        {
            QualitySettings.antiAliasing = 0;
        }

        private static void TwoMsaa()
        {
            QualitySettings.antiAliasing = 2;
        }

        private static void FourMsaa()
        {
            QualitySettings.antiAliasing = 4;
        }

        private static void EightMsaa()
        {
            QualitySettings.antiAliasing = 8;
        }

        public void NextPreset()
        {
            QualitySettings.IncreaseLevel();
            var cur = QualitySettings.GetQualityLevel();
            presetLabel.text = _presets[cur];
            shadowDistSlider.value = shadowDist[cur];
            aaCombo.value = cur;
        }

        public void LastPreset()
        {
            QualitySettings.DecreaseLevel();
            var cur = QualitySettings.GetQualityLevel();
            presetLabel.text = _presets[cur];
            shadowDistSlider.value = shadowDist[cur];
            aaCombo.value = cur;
        }
        #endregion

        #region INI Settings
        private void DefaultSettings()
        {
            SaveSettings.MasterVolumeIni = 0.8f;
            SaveSettings.EffectVolumeIni = 0.8f;
            SaveSettings.BackgroundVolumeIni = 0.8f;
            SaveSettings.CurrentQualityLevelIni = _presets.Length - 1;
            SaveSettings.MsaaIni = 2;
            SaveSettings.VsyncIni = true;
            SaveSettings.AnisoFilterLevelIni = 1;
            SaveSettings.RenderDistIni = 800.0f;
            SaveSettings.ShadowDistIni = shadowDist[SaveSettings.CurrentQualityLevelIni];
            SaveSettings.ShadowCascadeIni = 3;
            SaveSettings.TextureLimitIni = 0;
            SaveSettings.SettingsLoadedIni = true;
        }

        private void ApplyIniSettings()
        {
            if (Math.Abs(AudioListener.volume - SaveSettings.MasterVolumeIni) > 0f)
            {
                AudioListener.volume = SaveSettings.MasterVolumeIni;
            }
            if (Math.Abs(audioMasterVolumeSlider.value - SaveSettings.MasterVolumeIni) > 0f)
            {
                MuteEventListener(audioMasterVolumeSlider.onValueChanged);
                audioMasterVolumeSlider.value = SaveSettings.MasterVolumeIni;
                UnMuteEventListener(audioMasterVolumeSlider.onValueChanged);
            }
            
            if (sfx.Length > 0 && Math.Abs(sfx[0].volume - SaveSettings.EffectVolumeIni) > 0f)
            {
                foreach (var effect in sfx)
                {
                    effect.volume = SaveSettings.EffectVolumeIni;
                }
            }
            if (Math.Abs(effectsVolumeSlider.value - SaveSettings.EffectVolumeIni) > 0f)
            {    //    TODO : setup slider for effects
                MuteEventListener(effectsVolumeSlider.onValueChanged);
                effectsVolumeSlider.value = SaveSettings.EffectVolumeIni;
                UnMuteEventListener(effectsVolumeSlider.onValueChanged);
            }
            
            if (bgMusic != null && Math.Abs(bgMusic.volume - SaveSettings.BackgroundVolumeIni) > 0f)
            {
                bgMusic.volume = SaveSettings.BackgroundVolumeIni;
            }
            if (Math.Abs(backgroundVolumeSlider.value - SaveSettings.BackgroundVolumeIni) > 0f)
            {    //    TODO : setup slider for background
                MuteEventListener(backgroundVolumeSlider.onValueChanged);
                backgroundVolumeSlider.value = SaveSettings.BackgroundVolumeIni;
                UnMuteEventListener(backgroundVolumeSlider.onValueChanged);
            }

            if (QualitySettings.GetQualityLevel() != SaveSettings.CurrentQualityLevelIni)
            {
                QualitySettings.SetQualityLevel(SaveSettings.CurrentQualityLevelIni);
            }
            if (!presetLabel.text.Contains(_presets[SaveSettings.CurrentQualityLevelIni]))
            {
                presetLabel.text = _presets[SaveSettings.CurrentQualityLevelIni];
            }

            if (SaveSettings.VsyncIni && QualitySettings.vSyncCount == 0)
            {
                QualitySettings.vSyncCount = 1;
            }
            else if (!SaveSettings.VsyncIni && QualitySettings.vSyncCount > 0)
            {
                QualitySettings.vSyncCount = 0;
            }
            if (vSyncToggle.isOn != SaveSettings.VsyncIni)
            {
                MuteEventListener(vSyncToggle.onValueChanged);
                vSyncToggle.isOn = SaveSettings.VsyncIni;
                UnMuteEventListener(vSyncToggle.onValueChanged);
            }

            if (SaveSettings.MsaaIni == 0 && QualitySettings.antiAliasing != 0)
            {
                DisableMsaa();
            }
            else if (SaveSettings.MsaaIni == 1 && QualitySettings.antiAliasing != 2)
            {
                TwoMsaa();
            }
            else if (SaveSettings.MsaaIni == 2 && QualitySettings.antiAliasing != 4)
            {
                FourMsaa();
            }
            else if (SaveSettings.MsaaIni > 2 && QualitySettings.antiAliasing < 8)
            {
                EightMsaa();
            }
            if (aaCombo.value != SaveSettings.MsaaIni)
            {
                MuteEventListener(aaCombo.onValueChanged);
                aaCombo.value = SaveSettings.MsaaIni;
                UnMuteEventListener(aaCombo.onValueChanged);
            }

            if ((int)QualitySettings.anisotropicFiltering != SaveSettings.AnisoFilterLevelIni)
            {
                QualitySettings.anisotropicFiltering = (AnisotropicFiltering)SaveSettings.AnisoFilterLevelIni;
            }
            if (afCombo.value != SaveSettings.AnisoFilterLevelIni)
            {
                MuteEventListener(afCombo.onValueChanged);
                afCombo.value = SaveSettings.AnisoFilterLevelIni;
                UnMuteEventListener(afCombo.onValueChanged);
            }

            if (Camera.main.farClipPlane != SaveSettings.RenderDistIni)
            {
                Camera.main.farClipPlane = SaveSettings.RenderDistIni;
            }
            if (Math.Abs(renderDistSlider.value - SaveSettings.RenderDistIni) > 0f)
            {
                MuteEventListener(renderDistSlider.onValueChanged);
                renderDistSlider.value = SaveSettings.RenderDistIni;
                UnMuteEventListener(renderDistSlider.onValueChanged);
            }

            if (Math.Abs(QualitySettings.shadowDistance - SaveSettings.ShadowDistIni) > 0f)
            {
                QualitySettings.shadowDistance = SaveSettings.ShadowDistIni;
            }
            if (Math.Abs(shadowDistSlider.value - SaveSettings.ShadowDistIni) > 0f)
            {
                MuteEventListener(shadowDistSlider.onValueChanged);
                shadowDistSlider.value = SaveSettings.ShadowDistIni;
                UnMuteEventListener(shadowDistSlider.onValueChanged);
            }

            if (QualitySettings.masterTextureLimit != SaveSettings.TextureLimitIni)
            {
                QualitySettings.masterTextureLimit = SaveSettings.TextureLimitIni;
            }
            if (Math.Abs(masterTexSlider.value - SaveSettings.TextureLimitIni) > 0f)
            {
                MuteEventListener(masterTexSlider.onValueChanged);
                masterTexSlider.value = SaveSettings.TextureLimitIni;
                UnMuteEventListener(masterTexSlider.onValueChanged);
            }

            if (QualitySettings.shadowCascades != SaveSettings.ShadowCascadeIni)
            {
                QualitySettings.shadowCascades = SaveSettings.ShadowCascadeIni;
            }
            if (Math.Abs(shadowCascadesSlider.value - SaveSettings.ShadowCascadeIni) > 0f)
            {
                MuteEventListener(shadowCascadesSlider.onValueChanged);
                shadowCascadesSlider.value = SaveSettings.ShadowCascadeIni;
                UnMuteEventListener(shadowCascadesSlider.onValueChanged);
            }
        }
        #endregion

        #region Toggle OnValueChanged Listeners
        private static void MuteEventListener(UnityEngine.Events.UnityEventBase eventBase)
        {
            var count = eventBase.GetPersistentEventCount();
            for (var x = 0; x < count; x++)
            {
                eventBase.SetPersistentListenerState(x, UnityEngine.Events.UnityEventCallState.Off);
            }
        }

        private static void UnMuteEventListener(UnityEngine.Events.UnityEventBase eventBase)
        {
            var count = eventBase.GetPersistentEventCount();
            for (var x = 0; x < count; x++)
            {
                eventBase.SetPersistentListenerState(x, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
            }
        }
        #endregion
    }
}
