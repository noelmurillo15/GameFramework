/*
 * SaveSettings - Save/Loads game settings (audio, video) to/from a JSON file
 * Created by : Allan N. Murillo
 * Last Edited : 7/4/2021
 */

using System.IO;
using UnityEngine;

namespace ANM.Framework.Options
{
    [System.Serializable]
    public class SaveSettings
    {
        private static string _jsonString;
        private static string _fileName = "/GameSettings.json";

        public float masterVolume;
        public float effectVolume;
        public float backgroundVolume;
        public int currentQualityLevel;
        public int msaa;
        public float renderDist;
        public float shadowDist;
        public int textureLimit;
        public int anisotropicFilteringLevel;
        public int shadowCascade;
        public bool displayFps;

        internal static float MasterVolumeIni;
        internal static float EffectVolumeIni;
        internal static float BackgroundVolumeIni;
        internal static int CurrentQualityLevelIni;
        internal static int MsaaIni;
        internal static float RenderDistIni;
        internal static float ShadowDistIni;
        internal static int TextureLimitIni;
        internal static int AnisotropicFilteringLevelIni;
        internal static int ShadowCascadeIni;
        internal static bool DisplayFpsIni;
        internal static bool SettingsLoadedIni;


        private static object CreateJsonObj(string jsonString)
        {
            return JsonUtility.FromJson<SaveSettings>(jsonString);
        }

        public void Initialize() => SettingsLoadedIni = LoadGameSettings();

        public bool LoadGameSettings()
        {
            var filePath = Application.persistentDataPath + _fileName;
            if (!VerifyDirectory(filePath)) return false;
            OverwriteGameSettings(File.ReadAllText(filePath));
            return true;
        }

        public void SaveGameSettings()
        {
            var filePath = Application.persistentDataPath + _fileName;
            if (VerifyDirectory(filePath))
            {
                File.Delete(filePath);
            }

            masterVolume = MasterVolumeIni;
            effectVolume = EffectVolumeIni;
            backgroundVolume = BackgroundVolumeIni;
            renderDist = RenderDistIni;
            shadowDist = ShadowDistIni;
            msaa = MsaaIni;
            textureLimit = TextureLimitIni;
            currentQualityLevel = CurrentQualityLevelIni;
            shadowCascade = ShadowCascadeIni;
            anisotropicFilteringLevel = AnisotropicFilteringLevelIni;
            displayFps = DisplayFpsIni;

            _jsonString = JsonUtility.ToJson(this);
            File.WriteAllText(filePath, _jsonString);
        }

        private void OverwriteGameSettings(string jsonString)
        {
            var jsonObj = (SaveSettings) CreateJsonObj(jsonString);
            DefaultSettings();
            MasterVolumeIni = jsonObj.masterVolume;
            EffectVolumeIni = jsonObj.effectVolume;
            BackgroundVolumeIni = jsonObj.backgroundVolume;
            RenderDistIni = jsonObj.renderDist;
            ShadowDistIni = jsonObj.shadowDist;
            MsaaIni = jsonObj.msaa;
            TextureLimitIni = jsonObj.textureLimit;
            CurrentQualityLevelIni = jsonObj.currentQualityLevel;
            ShadowCascadeIni = jsonObj.shadowCascade;
            AnisotropicFilteringLevelIni = jsonObj.anisotropicFilteringLevel;
            DisplayFpsIni = jsonObj.displayFps;
        }

        public static void DefaultSettings()
        {
            MasterVolumeIni = 0.8f;
            EffectVolumeIni = 0.8f;
            BackgroundVolumeIni = 0.8f;
            CurrentQualityLevelIni = 2;
            MsaaIni = 2;
            AnisotropicFilteringLevelIni = 1;
            RenderDistIni = 1000.0f;
            ShadowDistIni = 150;
            ShadowCascadeIni = 3;
            TextureLimitIni = 0;
            DisplayFpsIni = true;
            SettingsLoadedIni = true;
        }

        private bool VerifyDirectory(string filePath) => File.Exists(filePath);
    }
}
