/*
    *   GameSettingsManager - Manages GameSettings UI & Functionality
    *   Created by : Allan N. Murillo
 */
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace GameFramework.Managers
{
    public class GameSettingsManager : MonoBehaviour
    {
        #region Class Members
        //  Panels
        [SerializeField] GameObject mask = null;
        [SerializeField] GameObject mainPanel = null;
        [SerializeField] GameObject vidPanel = null;
        [SerializeField] GameObject audioPanel = null;
        [SerializeField] GameObject quitPanel = null;
        [SerializeField] GameObject TitleTexts = null;

        //  Animators
        [SerializeField] Animator audioPanelAnimator = null;
        [SerializeField] Animator quitPanelAnimator = null;
        [SerializeField] Animator vidPanelAnimator = null;

        //  Misc
        [SerializeField] Camera mainCam = null;
        [SerializeField] String mainMenuSceneName = string.Empty;
        [SerializeField] float timeScale = 1f;

        //  Settings UI
        [SerializeField] TMP_Text pauseMenuTitleText = null;

        [SerializeField] Dropdown aaCombo = null;
        [SerializeField] Dropdown afCombo = null;

        [SerializeField] Slider renderDistSlider = null;
        [SerializeField] Slider shadowDistSlider = null;
        [SerializeField] Slider audioMasterVolumeSlider = null;
        [SerializeField] Slider masterTexSlider = null;
        [SerializeField] Slider shadowCascadesSlider = null;
        [SerializeField] Toggle vSyncToggle = null;

        [SerializeField] Text presetLabel = null;
        String[] presets;

        //  Hardcoded Shadow Distance
        [SerializeField] float[] shadowDist = null;

        //  Audio Sources
        [SerializeField] AudioSource bgMusic = null;
        [SerializeField] AudioSource[] sfx = null;

        public bool paused = false;
        GameManager gameManager = null;

        //  INI Settings
        internal static float masterVolumeINI;
        internal static int currentQualityLevelINI;
        internal static bool vsyncINI;
        internal static int msaaINI;
        internal static float renderDistINI;
        internal static float shadowDistINI;
        internal static int textureLimitINI;
        internal static int anisoFilterLevelINI;
        internal static int shadowCascadeINI;
        internal static bool settingsLoadedINI;
        #endregion


        //  TODO : updated the graphics preset changes other graphics settings but their UI is not updated =(
        public void Awake()
        {   //  Default Values
            if (TitleTexts != null)
            {
                TitleTexts.transform.GetChild(0).GetComponent<TMP_Text>().text = Application.productName;
            }

            paused = false;
            Time.timeScale = timeScale;
            presets = QualitySettings.names;
            pauseMenuTitleText.text = "Main Menu";

            gameManager = GameManager.Instance;
            gameManager.InitializeManager(this);

            //  TODO : Create an audio setting for BG music
            if (bgMusic != null) { bgMusic.volume = 0.8f; }

            if (gameManager.sceneLoader.GetCurrentSceneName() == mainMenuSceneName)
            {   //  Loaded Main Menu Scene                
                gameManager.IsMenuUIActive = true;
                gameManager.IsGameOver = true;
                mask.SetActive(true);
                mainPanel.SetActive(true);
                TitleTexts.SetActive(true);
                audioPanel.SetActive(false);
                vidPanel.SetActive(false);
            }
            else
            {   //  Loaded a Level Scene
                gameManager.IsMenuUIActive = false;
                gameManager.IsGameOver = false;
                mask.SetActive(false);
                mainPanel.SetActive(false);
                TitleTexts.SetActive(false);
                audioPanel.SetActive(false);
                vidPanel.SetActive(false);
            }

            if (!settingsLoadedINI) { DefaultSettings(); }
            ApplyINISettings();
        }

        #region Game Settings Panel Buttons
        public void StartGame()
        {
            gameManager.StartGameEvent();
        }

        public void Pause()
        {
            paused = true;
            Time.timeScale = 0f;
            gameManager.IsPauseUIActive = true;
            mainPanel.SetActive(true);
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            TitleTexts.SetActive(true);
            mask.SetActive(true);
        }

        public void Resume()
        {
            paused = false;
            Time.timeScale = timeScale;
            gameManager.IsPauseUIActive = false;
            mainPanel.SetActive(false);
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            TitleTexts.SetActive(false);
            mask.SetActive(false);
        }

        public void quitOptions()
        {
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            if (quitPanelAnimator != null)
            {
                quitPanelAnimator.enabled = true;
                quitPanelAnimator.Play("QuitPanelIn");
            }
            else
            {
                quitPanel.SetActive(true);
            }
        }

        public void quitCancel()
        {
            if (quitPanelAnimator != null)
            {
                quitPanelAnimator.Play("QuitPanelOut");
            }
            else
            {
                quitPanel.SetActive(false);
            }
        }

        public void returnToMenu()
        {
            paused = false;
            Time.timeScale = timeScale;
            gameManager.LoadMainMenuEvent();
        }

        public void quitGame()
        {
            paused = false;
            Time.timeScale = timeScale;
            gameManager.QuitApplicationEvent();
        }
        #endregion

        #region Audio Panel
        public void Audio()
        {
            vidPanel.SetActive(false);
            audioPanel.SetActive(true);
            if (!gameManager.IsMenuUIActive) { mainPanel.SetActive(false); }
            AudioPanelIn();
        }

        public void AudioPanelIn()
        {
            pauseMenuTitleText.text = "Audio Menu";
            if (audioPanelAnimator != null)
            {
                audioPanelAnimator.enabled = true;
                audioPanelAnimator.Play("Audio Panel In");
            }
        }

        public void applyAudio()
        {
            StartCoroutine(ApplyAudioSettings());
        }

        public void cancelAudio()
        {
            StartCoroutine(RevertAudioSettings());
        }

        internal IEnumerator ApplyAudioSettings()
        {
            if (audioPanelAnimator != null) { audioPanelAnimator.Play("Audio Panel Out"); }

            masterVolumeINI = audioMasterVolumeSlider.value;

            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.5f));
            pauseMenuTitleText.text = "Main Menu";
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            mainPanel.SetActive(true);
        }

        internal IEnumerator RevertAudioSettings()
        {
            if (audioPanelAnimator != null) { audioPanelAnimator.Play("Audio Panel Out"); }

            audioMasterVolumeSlider.value = masterVolumeINI;

            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.5f));
            pauseMenuTitleText.text = "Main Menu";
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            mainPanel.SetActive(true);
        }
        #endregion

        #region Video Panel
        public void Video()
        {   //  Turn On/Off Panels
            vidPanel.SetActive(true);
            audioPanel.SetActive(false);
            if (!gameManager.IsMenuUIActive) { mainPanel.SetActive(false); }
            VideoPanelIn();
        }

        public void VideoPanelIn()
        {
            pauseMenuTitleText.text = "Video Menu";
            if (vidPanelAnimator != null)
            {
                vidPanelAnimator.enabled = true;
                vidPanelAnimator.Play("Video Panel In");
            }
        }

        public void apply()
        {
            StartCoroutine(ApplyVideoSettings());
        }

        public void cancelVideo()
        {
            StartCoroutine(RevertVideoSettings());
        }

        internal IEnumerator ApplyVideoSettings()
        {
            if (vidPanelAnimator != null) { vidPanelAnimator.Play("Video Panel Out"); }

            currentQualityLevelINI = QualitySettings.GetQualityLevel();
            vsyncINI = vSyncToggle.isOn;
            msaaINI = aaCombo.value;
            anisoFilterLevelINI = afCombo.value;
            renderDistINI = renderDistSlider.value;
            textureLimitINI = (int)masterTexSlider.value;
            shadowDistINI = shadowDistSlider.value;
            shadowCascadeINI = (int)shadowCascadesSlider.value;

            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.5f));
            pauseMenuTitleText.text = "Main Menu";
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            mainPanel.SetActive(true);
        }

        internal IEnumerator RevertVideoSettings()
        {
            if (vidPanelAnimator != null) { vidPanelAnimator.Play("Video Panel Out"); }

            QualitySettings.SetQualityLevel(currentQualityLevelINI);
            vSyncToggle.isOn = vsyncINI;
            aaCombo.value = msaaINI;
            afCombo.value = anisoFilterLevelINI;
            renderDistSlider.value = renderDistINI;
            masterTexSlider.value = textureLimitINI;
            shadowDistSlider.value = shadowDistINI;
            shadowCascadesSlider.value = shadowCascadeINI;

            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.5f));
            pauseMenuTitleText.text = "Main Menu";
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            mainPanel.SetActive(true);
        }
        #endregion

        #region Game Setting Events
        public void updateMasterVol(float f)
        {
            AudioListener.volume = f;
            // Debug.Log("GameSettingsManager::updateMasterVol " + AudioListener.volume);
        }

        public void toggleVSync(Boolean B)
        {
            if (B == true)
            {
                QualitySettings.vSyncCount = 1;
            }
            else
            {
                QualitySettings.vSyncCount = 0;
            }
            // Debug.Log("GameSettingsManager::toggleVSync : " + QualitySettings.vSyncCount);
        }

        public void updateRenderDist(float f)
        {
            try
            {
                mainCam.farClipPlane = f;
            }
            catch
            {
                // Debug.Log("Camera was not assigned, using Camera.Main");
                mainCam = Camera.main;
                mainCam.farClipPlane = f;
            }
            // Debug.Log("GameSettingsManager::updateRenderDist() : " + Camera.main.farClipPlane);
        }

        public void updateTex(float qual)
        {
            int f = Mathf.RoundToInt(qual);
            QualitySettings.masterTextureLimit = f;
            // Debug.Log("GameSettingsManager::updateTex() : " + QualitySettings.masterTextureLimit);
        }

        public void updateShadowDistance(float dist)
        {
            QualitySettings.shadowDistance = dist;
            // Debug.Log("GameSettingsManager::updateShadowDistance() : " + QualitySettings.shadowDistance);
        }

        public void forceOnANISO()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        }

        public void perTexANISO()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
        }

        public void disableANISO()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
        }

        public void updateANISO(int anisoSetting)
        {
            if (anisoSetting == 0)
            {
                disableANISO();
            }
            else if (anisoSetting == 1)
            {
                perTexANISO();
            }
            else if (anisoSetting == 2)
            {
                forceOnANISO();
            }
            // Debug.Log("GameSettingsManager::updateANISO() : " + (int)QualitySettings.anisotropicFiltering);
        }

        public void updateCascades(float cascades)
        {
            int c = Mathf.RoundToInt(cascades);
            if (c == 1)
            {
                c = 2;
            }
            else if (c == 3)
            {
                c = 2;
            }
            QualitySettings.shadowCascades = c;
            // Debug.Log("GameSettingsManager::updateCascades() : " + QualitySettings.shadowCascades);
        }

        public void updateMSAA(int msaaAmount)
        {
            if (msaaAmount == 0)
            {
                disableMSAA();
            }
            else if (msaaAmount == 1)
            {
                twoMSAA();
            }
            else if (msaaAmount == 2)
            {
                fourMSAA();
            }
            else if (msaaAmount == 3)
            {
                eightMSAA();
            }
            // Debug.Log("MSAA has been set to " + QualitySettings.antiAliasing + "x");
        }

        public void disableMSAA()
        {
            QualitySettings.antiAliasing = 0;
        }

        public void twoMSAA()
        {
            QualitySettings.antiAliasing = 2;
        }

        public void fourMSAA()
        {
            QualitySettings.antiAliasing = 4;
        }

        public void eightMSAA()
        {
            QualitySettings.antiAliasing = 8;
        }

        public void nextPreset()
        {
            QualitySettings.IncreaseLevel();
            int cur = QualitySettings.GetQualityLevel();
            presetLabel.text = presets[cur];
            shadowDistSlider.value = shadowDist[cur];
            aaCombo.value = cur;
            // Debug.Log("Graphics Preset has been set to " + presets[cur]);
        }

        public void lastPreset()
        {
            QualitySettings.DecreaseLevel();
            int cur = QualitySettings.GetQualityLevel();
            presetLabel.text = presets[cur];
            shadowDistSlider.value = shadowDist[cur];
            aaCombo.value = cur;
            // Debug.Log("Graphics Preset has been set to " + presets[cur]);
        }
        #endregion

        #region INI Settings
        void DefaultSettings()
        {
            Debug.Log("GameSettingsManager::DefaultSettings()");

            //  Default Audio
            masterVolumeINI = 0.75f;

            //  Default Graphics Preset
            currentQualityLevelINI = presets.Length - 1;

            //  Default Graphics Settings
            msaaINI = 2;
            vsyncINI = true;
            anisoFilterLevelINI = 1;
            renderDistINI = 800.0f;
            shadowDistINI = shadowDist[currentQualityLevelINI];
            shadowCascadeINI = 4;
            textureLimitINI = 0;
        }

        void ApplyINISettings()
        {
            // Debug.Log("INI Settings being applied : " + "Vol : " + masterVolumeINI + ", vsync : " + vsyncINI +
            //     ", Preset " + currentQualityLevelINI + ", RenderDist : " + renderDistINI + ", ShadowDist : " + shadowDistINI +
            //     ", cascade " + shadowCascadeINI + ", MSAA : " + msaaINI + ", aniso : " + anisoFilterLevelINI + ", texture limit : " + textureLimitINI);

            //  Master Volume
            if (AudioListener.volume != masterVolumeINI)
            {
                AudioListener.volume = masterVolumeINI;
            }
            if (audioMasterVolumeSlider.value != masterVolumeINI)
            {
                MuteEventListener(audioMasterVolumeSlider.onValueChanged);
                audioMasterVolumeSlider.value = masterVolumeINI;
                UnMuteEventListener(audioMasterVolumeSlider.onValueChanged);
            }

            //  Graphics Quality Preset
            if (QualitySettings.GetQualityLevel() != currentQualityLevelINI)
            {
                QualitySettings.SetQualityLevel(currentQualityLevelINI);
            }
            if (!presetLabel.text.Contains(presets[currentQualityLevelINI]))
            {
                presetLabel.text = presets[currentQualityLevelINI];
            }

            //  Vsync
            if (vsyncINI && QualitySettings.vSyncCount == 0)
            {
                QualitySettings.vSyncCount = 1;
            }
            else if (!vsyncINI && QualitySettings.vSyncCount > 0)
            {
                QualitySettings.vSyncCount = 0;
            }
            if (vSyncToggle.isOn != vsyncINI)
            {
                MuteEventListener(vSyncToggle.onValueChanged);
                vSyncToggle.isOn = vsyncINI;
                UnMuteEventListener(vSyncToggle.onValueChanged);
            }

            //  MSAA
            if (msaaINI == 0 && QualitySettings.antiAliasing != 0)
            {
                disableMSAA();
            }
            else if (msaaINI == 1 && QualitySettings.antiAliasing != 2)
            {
                twoMSAA();
            }
            else if (msaaINI == 2 && QualitySettings.antiAliasing != 4)
            {
                fourMSAA();
            }
            else if (msaaINI > 2 && QualitySettings.antiAliasing < 8)
            {
                eightMSAA();
            }
            if (aaCombo.value != msaaINI)
            {
                MuteEventListener(aaCombo.onValueChanged);
                aaCombo.value = msaaINI;
                UnMuteEventListener(aaCombo.onValueChanged);
            }

            //  Anisotropic Texture Filtering
            if ((int)QualitySettings.anisotropicFiltering != anisoFilterLevelINI)
            {
                QualitySettings.anisotropicFiltering = (AnisotropicFiltering)anisoFilterLevelINI;
            }
            if (afCombo.value != anisoFilterLevelINI)
            {
                MuteEventListener(afCombo.onValueChanged);
                afCombo.value = anisoFilterLevelINI;
                UnMuteEventListener(afCombo.onValueChanged);
            }

            //  Render Distance
            if (Camera.main.farClipPlane != renderDistINI)
            {
                Camera.main.farClipPlane = renderDistINI;
            }
            if (renderDistSlider.value != renderDistINI)
            {
                MuteEventListener(renderDistSlider.onValueChanged);
                renderDistSlider.value = renderDistINI;
                UnMuteEventListener(renderDistSlider.onValueChanged);
            }

            //  Shadow Distance
            if (QualitySettings.shadowDistance != shadowDistINI)
            {
                QualitySettings.shadowDistance = shadowDistINI;
            }
            if (shadowDistSlider.value != shadowDistINI)
            {
                MuteEventListener(shadowDistSlider.onValueChanged);
                shadowDistSlider.value = shadowDistINI;
                UnMuteEventListener(shadowDistSlider.onValueChanged);
            }

            //  Master Texture Limit
            if (QualitySettings.masterTextureLimit != textureLimitINI)
            {
                QualitySettings.masterTextureLimit = textureLimitINI;
            }
            if (masterTexSlider.value != textureLimitINI)
            {
                MuteEventListener(masterTexSlider.onValueChanged);
                masterTexSlider.value = textureLimitINI;
                UnMuteEventListener(masterTexSlider.onValueChanged);
            }

            //  Shadow Cascade
            if (QualitySettings.shadowCascades != shadowCascadeINI)
            {
                QualitySettings.shadowCascades = shadowCascadeINI;
            }
            if (shadowCascadesSlider.value != shadowCascadeINI)
            {
                MuteEventListener(shadowCascadesSlider.onValueChanged);
                shadowCascadesSlider.value = shadowCascadeINI;
                UnMuteEventListener(shadowCascadesSlider.onValueChanged);
            }

            // Debug.Log("Applied Settings : " + "Vol : " + AudioListener.volume + ", vsync : " + QualitySettings.vSyncCount +
            //     ", Preset " + QualitySettings.GetQualityLevel() + ", RenderDist : " + Camera.main.farClipPlane +
            //     ", Shadow Dist : " + QualitySettings.shadowDistance + ", cascade " + QualitySettings.shadowCascades +
            //     ", MSAA : " + QualitySettings.antiAliasing + ", aniso : " + (int)QualitySettings.anisotropicFiltering +
            //     ", texture limit : " + QualitySettings.masterTextureLimit
            // );

            settingsLoadedINI = true;
        }
        #endregion

        #region Toggle OnValueChanged Listeners
        void MuteEventListener(UnityEngine.Events.UnityEventBase eventBase)
        {
            int count = eventBase.GetPersistentEventCount();
            for (int x = 0; x < count; x++)
            {
                eventBase.SetPersistentListenerState(x, UnityEngine.Events.UnityEventCallState.Off);
            }
        }

        void UnMuteEventListener(UnityEngine.Events.UnityEventBase eventBase)
        {
            int count = eventBase.GetPersistentEventCount();
            for (int x = 0; x < count; x++)
            {
                eventBase.SetPersistentListenerState(x, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
            }
        }
        #endregion
    }
}