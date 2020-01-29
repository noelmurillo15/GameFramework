/*
 * SaveSettings - Save/Loads game settings to/from a JSON file
 * Created by : Allan N. Murillo
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
            var path = Application.persistentDataPath + _fileName;
            if (!VerifyDirectory(path)) return false;
            OverwriteGameSettings(File.ReadAllText(path));
            return true;
        }

        public void SaveGameSettings()
        {
            var path = Application.persistentDataPath + _fileName;
            if (VerifyDirectory(path)) { File.Delete(path); }
            
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
            File.WriteAllText(path, _jsonString);
        }

        private void OverwriteGameSettings(string jsonString)
        {
            try
            {
                SaveSettings read = (SaveSettings)CreateJsonObj(jsonString);
                MasterVolumeIni = read.masterVolume;
                EffectVolumeIni = read.effectVolume;
                BackgroundVolumeIni = read.backgroundVolume;
                RenderDistIni = read.renderDist;
                ShadowDistIni = read.shadowDist;
                MsaaIni = read.msaa;
                VsyncIni = read.vsync;
                TextureLimitIni = read.textureLimit;
                CurrentQualityLevelIni = read.currentQualityLevel;
                ShadowCascadeIni = read.shadowCascade;
                AnisoFilterLevelIni = read.anisoFilterLevel;
            }
            catch (FileLoadException)
            {
                Debug.LogError("Could not read game settings from json file");
            }
        }

        private bool VerifyDirectory(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
