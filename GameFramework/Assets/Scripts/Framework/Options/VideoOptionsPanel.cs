/*
 * VideoOptionsPanel - Handles displaying / configuring graphics options
 * Created by : Allan N. Murillo
 * Last Edited : 3/4/2020
 */

using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ANM.Framework.Utils;
using ANM.Framework.Managers;
using ANM.Framework.Extensions;
using UnityEngine.EventSystems;

namespace ANM.Framework.Options
{
    public class VideoOptionsPanel : MonoBehaviour, IPanel
    {
        [SerializeField] private Toggle fpsDisplayToggle = null;
        [SerializeField] private Toggle vsyncToggle = null;
        
        [SerializeField] private TMP_Dropdown msaaDropdown = null;
        [SerializeField] private TMP_Dropdown anisotropicDropdown = null;
        [SerializeField] private Slider renderDistSlider = null;
        [SerializeField] private Slider shadowDistSlider = null;

        [SerializeField] private Slider masterTexSlider = null;
        [SerializeField] private Slider shadowCascadesSlider = null;
        [SerializeField] private TMP_Text presetLabel = null;
        [SerializeField] private float[] shadowDist = null;

        [SerializeField] private Button videoPanelSelectedObj = null;

        private Camera _myCamera;
        private string[] _presets;
        private GameObject _panel;
        private Animator _videoPanelAnimator;


        private void Awake()
        {
            _myCamera = transform.root.GetComponentInChildren<Camera>();
            _presets = QualitySettings.names;
        }

        private void Start()
        {
            _videoPanelAnimator = GetComponent<Animator>();
            _panel = _videoPanelAnimator.transform.GetChild(0).gameObject;
        }

        public void TurnOnPanel()
        {
            if (!_panel.activeSelf)
                _videoPanelAnimator.Play("Video Panel In");
        }

        public void TurnOffPanel()
        {
            if (_panel.activeSelf)
                _videoPanelAnimator.Play("Video Panel Out");
        }

        public void VideoPanelIn(EventSystem eventSystem)
        {
            TurnOnPanel();
            eventSystem.SetSelectedGameObject(GetSelectObject());
            videoPanelSelectedObj.OnSelect(null);
        }

        public IEnumerator SaveVideoSettings()
        {
            TurnOffPanel();
            SaveSettings.CurrentQualityLevelIni = QualitySettings.GetQualityLevel();
            SaveSettings.MsaaIni = msaaDropdown.value;
            SaveSettings.AnisotropicFilteringLevelIni = anisotropicDropdown.value;
            SaveSettings.RenderDistIni = renderDistSlider.value;
            SaveSettings.TextureLimitIni = (int) masterTexSlider.value;
            SaveSettings.ShadowDistIni = shadowDistSlider.value;
            SaveSettings.ShadowCascadeIni = (int) shadowCascadesSlider.value;
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.5f));
            GameManager.Instance.SaveGameSettings();
        }

        public IEnumerator RevertVideoSettings()
        {
            TurnOffPanel();
            QualitySettings.SetQualityLevel(SaveSettings.CurrentQualityLevelIni);
            msaaDropdown.value = SaveSettings.MsaaIni;
            anisotropicDropdown.value = SaveSettings.AnisotropicFilteringLevelIni;
            renderDistSlider.value = SaveSettings.RenderDistIni;
            masterTexSlider.value = SaveSettings.TextureLimitIni;
            shadowDistSlider.value = SaveSettings.ShadowDistIni;
            shadowCascadesSlider.value = SaveSettings.ShadowCascadeIni;
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.5f));
        }

        public void ApplyVideoSettings()
        {
            OverrideFpsDisplay(SaveSettings.DisplayFpsIni);
            OverrideVsync(SaveSettings.VsyncIni);
            OverrideAnisotropicFiltering();
            OverrideMasterTextureQuality();
            OverrideGraphicsPreset();
            OverrideRenderDistance();
            OverrideShadowDistance();
            OverrideShadowCascade();
            OverrideMsaa();
        }
        
        public void ToggleVsync(bool b)
        {
            QualitySettings.vSyncCount = b ? 1 : 0;
        }

        public void ToggleFpsDisplay(bool b)
        {
            GameManager.Instance.SetDisplayFps(b);
        }

        public void UpdateRenderDistance(float renderDistance)
        {
            if (_myCamera == null) return;
            _myCamera.farClipPlane = renderDistance;
        }

        public void UpdateMasterTextureQuality(float textureQuality)
        {
            var f = Mathf.RoundToInt(textureQuality);
            QualitySettings.masterTextureLimit = f;
        }

        public void UpdateShadowDistance(float shadowDistance)
        {
            QualitySettings.shadowDistance = shadowDistance;
        }

        public void UpdateAnisotropicFiltering(int level)
        {
            switch (level)
            {
                case 0:
                    DisableAnisotropicFilter();
                    break;
                case 1:
                    PerTextureAnisotropicFilter();
                    break;
                case 2:
                    ForceOnAnisotropicFilter();
                    break;
            }
        }

        public void UpdateShadowCascades(float cascades)
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

        public void UpdateMsaa(int level)
        {
            switch (level)
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

        public void NextPreset()
        {
            QualitySettings.IncreaseLevel();
            var cur = QualitySettings.GetQualityLevel();
            PresetOverride(cur);
        }

        public void LastPreset()
        {
            QualitySettings.DecreaseLevel();
            var cur = QualitySettings.GetQualityLevel();
            PresetOverride(cur);
        }

        private static void ForceOnAnisotropicFilter()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        }

        private static void PerTextureAnisotropicFilter()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
        }

        private static void DisableAnisotropicFilter()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
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

        private void OverrideFpsDisplay(bool b)
        {
            EventExtension.MuteEventListener(fpsDisplayToggle.onValueChanged);
            GameManager.Instance.SetDisplayFps(b);
            fpsDisplayToggle.isOn = b;
            SaveSettings.DisplayFpsIni = b;
            EventExtension.UnMuteEventListener(fpsDisplayToggle.onValueChanged);
        }
        
        private void OverrideVsync(bool b)
        {
            EventExtension.MuteEventListener(vsyncToggle.onValueChanged);
            QualitySettings.vSyncCount = b ? 1 : 0;
            vsyncToggle.isOn = b;
            SaveSettings.VsyncIni = b;
            EventExtension.UnMuteEventListener(vsyncToggle.onValueChanged);
        }

        private void OverrideGraphicsPreset()
        {
            if (QualitySettings.GetQualityLevel() != SaveSettings.CurrentQualityLevelIni)
                QualitySettings.SetQualityLevel(SaveSettings.CurrentQualityLevelIni);

            if (!presetLabel.text.Contains(_presets[SaveSettings.CurrentQualityLevelIni]))
                presetLabel.text = _presets[SaveSettings.CurrentQualityLevelIni];
        }

        private void OverrideMsaa()
        {
            switch (SaveSettings.MsaaIni)
            {
                case 0 when QualitySettings.antiAliasing != 0:
                    DisableMsaa();
                    break;
                case 1 when QualitySettings.antiAliasing != 2:
                    TwoMsaa();
                    break;
                case 2 when QualitySettings.antiAliasing != 4:
                    FourMsaa();
                    break;
                case 3 when QualitySettings.antiAliasing < 8:
                    EightMsaa();
                    break;
            }

            if (msaaDropdown.value == SaveSettings.MsaaIni) return;
            EventExtension.MuteEventListener(msaaDropdown.onValueChanged);
            msaaDropdown.value = SaveSettings.MsaaIni;
            EventExtension.UnMuteEventListener(msaaDropdown.onValueChanged);
        }

        private void OverrideAnisotropicFiltering()
        {
            if ((int) QualitySettings.anisotropicFiltering != SaveSettings.AnisotropicFilteringLevelIni)
                QualitySettings.anisotropicFiltering = (AnisotropicFiltering) SaveSettings.AnisotropicFilteringLevelIni;

            if (anisotropicDropdown.value == SaveSettings.AnisotropicFilteringLevelIni) return;
            EventExtension.MuteEventListener(anisotropicDropdown.onValueChanged);
            anisotropicDropdown.value = SaveSettings.AnisotropicFilteringLevelIni;
            EventExtension.UnMuteEventListener(anisotropicDropdown.onValueChanged);
        }

        private void OverrideRenderDistance()
        {
            if (_myCamera == null) return;

            if (Math.Abs(_myCamera.farClipPlane - SaveSettings.RenderDistIni) > 0f)
                _myCamera.farClipPlane = SaveSettings.RenderDistIni;

            if (!(Math.Abs(renderDistSlider.value - SaveSettings.RenderDistIni) > 0f)) return;
            EventExtension.MuteEventListener(renderDistSlider.onValueChanged);
            renderDistSlider.value = SaveSettings.RenderDistIni;
            EventExtension.UnMuteEventListener(renderDistSlider.onValueChanged);
        }

        private void OverrideShadowDistance()
        {
            if (Math.Abs(QualitySettings.shadowDistance - SaveSettings.ShadowDistIni) > 0f)
                QualitySettings.shadowDistance = SaveSettings.ShadowDistIni;

            if (!(Math.Abs(shadowDistSlider.value - SaveSettings.ShadowDistIni) > 0f)) return;
            EventExtension.MuteEventListener(shadowDistSlider.onValueChanged);
            shadowDistSlider.value = SaveSettings.ShadowDistIni;
            EventExtension.UnMuteEventListener(shadowDistSlider.onValueChanged);
        }

        private void OverrideShadowCascade()
        {
            if (QualitySettings.shadowCascades != SaveSettings.ShadowCascadeIni)
                QualitySettings.shadowCascades = SaveSettings.ShadowCascadeIni;

            if (!(Math.Abs(shadowCascadesSlider.value - SaveSettings.ShadowCascadeIni) > 0f)) return;
            EventExtension.MuteEventListener(shadowCascadesSlider.onValueChanged);
            shadowCascadesSlider.value = SaveSettings.ShadowCascadeIni;
            EventExtension.UnMuteEventListener(shadowCascadesSlider.onValueChanged);
        }

        private void OverrideMasterTextureQuality()
        {
            if (QualitySettings.masterTextureLimit != SaveSettings.TextureLimitIni)
                QualitySettings.masterTextureLimit = SaveSettings.TextureLimitIni;

            if (!(Math.Abs(masterTexSlider.value - SaveSettings.TextureLimitIni) > 0f)) return;
            EventExtension.MuteEventListener(masterTexSlider.onValueChanged);
            masterTexSlider.value = SaveSettings.TextureLimitIni;
            EventExtension.UnMuteEventListener(masterTexSlider.onValueChanged);
        }

        private void PresetOverride(int currentValue)
        {
            presetLabel.text = _presets[currentValue];
            shadowDistSlider.value = shadowDist[currentValue];
            msaaDropdown.value = currentValue;
        }

        private GameObject GetSelectObject()
        {
            return videoPanelSelectedObj.gameObject;
        }
    }
}
