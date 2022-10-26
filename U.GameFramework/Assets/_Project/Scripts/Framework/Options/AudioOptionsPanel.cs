/*
 * AudioOptionsPanel - Handles displaying / configuring audio options
 * Created by : Allan N. Murillo
 * Last Edited : 10/26/2022
 */

using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ANM.Framework.Utils;
using ANM.Framework.Managers;
using ANM.Framework.Extensions;
using UnityEngine.EventSystems;

namespace ANM.Framework.Options
{
    public class AudioOptionsPanel : MonoBehaviour
    {
        [SerializeField] private Slider audioMasterVolumeSlider = null;
        [SerializeField] private Slider effectsVolumeSlider = null;
        [SerializeField] private Slider backgroundVolumeSlider = null;
        [SerializeField] private Button audioPanelSelectedObj = null;

        [SerializeField] private string busMasterPath = "";
        private FMOD.Studio.Bus masterBus;

        [SerializeField] private string busUiPath = "";
        private FMOD.Studio.Bus uiBus;

        [SerializeField] private string busAmbiencePath = "";
        private FMOD.Studio.Bus ambienceBus;

        private Animator _audioPanelAnimator;
        private GameObject _panel;


        private void Start()
        {
            _audioPanelAnimator = GetComponent<Animator>();
            _panel = _audioPanelAnimator.transform.GetChild(0).gameObject;
            _panel.SetActive(false);

            if (busMasterPath != "")
            {
                masterBus = RuntimeManager.GetBus(busMasterPath);
            }

            if (busUiPath != "")
            {
                uiBus = RuntimeManager.GetBus(busUiPath);
            }

            if (busAmbiencePath != "")
            {
                ambienceBus = RuntimeManager.GetBus(busAmbiencePath);
            }
        }

        public void AudioPanelIn(EventSystem eventSystem)
        {
            _panel.SetActive(true);
            eventSystem.SetSelectedGameObject(GetSelectObject());
            audioPanelSelectedObj.OnSelect(null);
        }

        public IEnumerator SaveAudioSettings()
        {
            SaveSettings.MasterVolumeIni = audioMasterVolumeSlider.value;
            SaveSettings.EffectVolumeIni = effectsVolumeSlider.value;
            SaveSettings.BackgroundVolumeIni = backgroundVolumeSlider.value;
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.5f));
            GameManager.Instance.SaveGameSettings();
            _panel.SetActive(false);
        }

        public IEnumerator RevertAudioSettings()
        {
            audioMasterVolumeSlider.value = SaveSettings.MasterVolumeIni;
            effectsVolumeSlider.value = SaveSettings.EffectVolumeIni;
            backgroundVolumeSlider.value = SaveSettings.BackgroundVolumeIni;
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.5f));
            _panel.SetActive(false);
        }

        public void ApplyAudioSettings()
        {
            OverrideMasterVolume();
            OverrideBackgroundVolume();
            OverrideEffectsVolume();
        }

        public void UpdateMasterVolume(float amount)
        {
            masterBus.setVolume(amount);
        }

        public void UpdateEffectsVolume(float amount)
        {
            uiBus.setVolume(amount);
        }

        public void UpdateBackgroundVolume(float amount)
        {
            ambienceBus.setVolume(amount);
        }

        private void OverrideMasterVolume()
        {
            masterBus.getVolume(out var volume);
            if (Math.Abs(volume - SaveSettings.BackgroundVolumeIni) > 0f)
                masterBus.setVolume(SaveSettings.BackgroundVolumeIni);

            if (!(Math.Abs(audioMasterVolumeSlider.value - SaveSettings.MasterVolumeIni) > 0f)) return;
            EventExtension.MuteEventListener(audioMasterVolumeSlider.onValueChanged);
            audioMasterVolumeSlider.value = SaveSettings.MasterVolumeIni;
            EventExtension.UnMuteEventListener(audioMasterVolumeSlider.onValueChanged);
        }

        private void OverrideBackgroundVolume()
        {
            ambienceBus.getVolume(out var volume);
            if (Math.Abs(volume - SaveSettings.BackgroundVolumeIni) > 0f)
                ambienceBus.setVolume(SaveSettings.BackgroundVolumeIni);

            if (!(Math.Abs(backgroundVolumeSlider.value - SaveSettings.BackgroundVolumeIni) > 0f)) return;
            EventExtension.MuteEventListener(backgroundVolumeSlider.onValueChanged);
            backgroundVolumeSlider.value = SaveSettings.BackgroundVolumeIni;
            EventExtension.UnMuteEventListener(backgroundVolumeSlider.onValueChanged);
        }

        private void OverrideEffectsVolume()
        {
            uiBus.getVolume(out var volume);
            if (Math.Abs(volume - SaveSettings.EffectVolumeIni) > 0f)
                uiBus.setVolume(SaveSettings.EffectVolumeIni);

            if (!(Math.Abs(effectsVolumeSlider.value - SaveSettings.EffectVolumeIni) > 0f)) return;
            EventExtension.MuteEventListener(effectsVolumeSlider.onValueChanged);
            effectsVolumeSlider.value = SaveSettings.EffectVolumeIni;
            EventExtension.UnMuteEventListener(effectsVolumeSlider.onValueChanged);
        }

        private GameObject GetSelectObject()
        {
            return audioPanelSelectedObj.gameObject;
        }
    }
}
