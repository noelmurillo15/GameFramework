/*
 * SaveSettings - Save/Loads game settings to/from a JSON file
 * Created by : Allan N. Murillo
 * Last Edited : 2/7/2020
 */

using System.IO;
using UnityEngine;

namespace ANM.Framework
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
        public bool vsync;
        public int msaa;
        public float renderDist;
        public float shadowDist;
        public int textureLimit;
        public int anisoFilterLevel;
        public int shadowCascade;

        internal static float MasterVolumeIni;
        internal static float EffectVolumeIni;
        internal static float BackgroundVolumeIni;
        internal static int CurrentQualityLevelIni;
        internal static bool VsyncIni;
        internal static int MsaaIni;
        internal static float RenderDistIni;
        internal static float ShadowDistIni;
        internal static int TextureLimitIni;
        internal static int AnisoFilterLevelIni;
        internal static int ShadowCascadeIni;
        internal static bool SettingsLoadedIni;


        public void Initialize() {  SettingsLoadedIni = LoadGameSettings();  }
        
        private static object CreateJsonObj(string jsonString)
        {
            return JsonUtility.FromJson<SaveSettings>(jsonString);
        }

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
            if (VerifyDirectory(filePath)) { File.Delete(filePath); }
            
            masterVolume = MasterVolumeIni;
            effectVolume = EffectVolumeIni;
            backgroundVolume = BackgroundVolumeIni;
            renderDist = RenderDistIni;
            shadowDist = ShadowDistIni;
            msaa = MsaaIni;
            vsync = VsyncIni;
            textureLimit = TextureLimitIni;
            currentQualityLevel = CurrentQualityLevelIni;
            shadowCascade = ShadowCascadeIni;
            anisoFilterLevel = AnisoFilterLevelIni;
            
            _jsonString = JsonUtility.ToJson(this);
            File.WriteAllText(filePath, _jsonString);
        }

        private void OverwriteGameSettings(string jsonString)
        {
            var jsonObj = (SaveSettings) CreateJsonObj(jsonString);
            MasterVolumeIni = jsonObj.masterVolume;
            EffectVolumeIni = jsonObj.effectVolume;
            BackgroundVolumeIni = jsonObj.backgroundVolume;
            RenderDistIni = jsonObj.renderDist;
            ShadowDistIni = jsonObj.shadowDist;
            MsaaIni = jsonObj.msaa;
            VsyncIni = jsonObj.vsync;
            TextureLimitIni = jsonObj.textureLimit;
            CurrentQualityLevelIni = jsonObj.currentQualityLevel;
            ShadowCascadeIni = jsonObj.shadowCascade;
            AnisoFilterLevelIni = jsonObj.anisoFilterLevel;
        }

        private bool VerifyDirectory(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
