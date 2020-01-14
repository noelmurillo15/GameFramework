/*
    *   SaveSettings - Save/Loads game settings to/from a JSON file
    *   Created by : Allan N. Murillo
 */
using System.IO;
using UnityEngine;

namespace GameFramework.Core
{
    [System.Serializable]
    public class SaveSettings
    {
        private static string _jsonString;
        private static string _fileName = "GameSettings.json";

        public float masterVolume;
        public int currentQualityLevel;
        public bool vsync;
        public float renderDistance;
        public float shadowDist;
        public int shadowCascade;
        public int msaa;
        public int anisoLevel;
        public int textureLimit;


        private static object CreateJsonObj(string jsonString)
        {
            return JsonUtility.FromJson<SaveSettings>(jsonString);
        }

        public bool LoadGameSettings()
        {
            string path = Application.persistentDataPath + "/" + _fileName;
            if (!VerifyDirectory(path)) return false;
            OverwriteGameSettings(File.ReadAllText(path));
            return true;
        }

        public void SaveGameSettings()
        {
            string path = Application.persistentDataPath + "/" + _fileName;
            if (VerifyDirectory(path)) { File.Delete(path); }

            masterVolume = GameSettingsManager.MasterVolumeIni;
            vsync = GameSettingsManager.VsyncIni;
            msaa = GameSettingsManager.MsaaIni;
            renderDistance = GameSettingsManager.RenderDistIni;
            textureLimit = GameSettingsManager.TextureLimitIni;
            shadowDist = GameSettingsManager.ShadowDistIni;
            shadowCascade = GameSettingsManager.ShadowCascadeIni;
            anisoLevel = GameSettingsManager.AnisoFilterLevelIni;
            currentQualityLevel = GameSettingsManager.CurrentQualityLevelIni;

            _jsonString = JsonUtility.ToJson(this);
            File.WriteAllText(path, _jsonString);

            // Debug.Log("Saving these Settings to JSON : " + "Vol : " + masterVolume + ", vsync : " + vsync +
            //     ", Preset " + currentQualityLevel + ", RenderDist : " + renderDistance + ", ShadowDist : " + shadowDist +
            //     ", cascade " + shadowCascade + ", MSAA : " + msaa + ", aniso : " + anisoLevel + ", texture limit : " + textureLimit);
            
            //  Sync with Web Browser's IndexedDB via JavaScript
            if (Application.platform == RuntimePlatform.WebGLPlayer) {  Sync();  }
        }

        private void OverwriteGameSettings(string jsonString)
        {
            try
            {
                SaveSettings read = (SaveSettings)CreateJsonObj(jsonString);
                masterVolume = read.masterVolume;
                renderDistance = read.renderDistance;
                shadowDist = read.shadowDist;
                msaa = read.msaa;
                vsync = read.vsync;
                textureLimit = read.textureLimit;
                currentQualityLevel = read.currentQualityLevel;
                shadowCascade = read.shadowCascade;
                anisoLevel = read.anisoLevel;
            }
            catch (FileLoadException)
            {
                Debug.LogError("Could not read game settings from json file");
                return;
            }

            // Debug.Log("Loaded JSON Settings : " + "Vol : " + masterVolume + ", vsync : " + vsync +
            //     ", Preset " + currentQualityLevel + ", RenderDist : " + renderDistance + ", ShadowDist : " + shadowDist +
            //     ", cascade " + shadowCascade + ", MSAA : " + msaa + ", aniso : " + anisoLevel + ", texture limit : " + textureLimit);

            GameSettingsManager.MasterVolumeIni = masterVolume;
            GameSettingsManager.VsyncIni = vsync;
            GameSettingsManager.MsaaIni = msaa;
            GameSettingsManager.RenderDistIni = renderDistance;
            GameSettingsManager.TextureLimitIni = textureLimit;
            GameSettingsManager.ShadowDistIni = shadowDist;
            GameSettingsManager.ShadowCascadeIni = shadowCascade;
            GameSettingsManager.AnisoFilterLevelIni = anisoLevel;
            GameSettingsManager.CurrentQualityLevelIni = currentQualityLevel;
            GameSettingsManager.SettingsLoadedIni = true;
        }

        private bool VerifyDirectory(string filePath)
        {
            return File.Exists(filePath);
        }
        
        #region External JS LIBRARY
#if UNITY_WEBGL && !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("__Internal")]
        static extern void InitilaizeJsLib();
        [System.Runtime.InteropServices.DllImport("__Internal")]
        static extern void SyncPersistantData();


        public void Initialize() { InitilaizeJsLib(); }

        //  Unity WebGL stores all files that must persist between sessions to the browser IndexedDB.
        //  This function makes sure Unity flushes all pending file system write operations to the IndexedDB file system from memory   
        public void Sync() { SyncPersistantData(); }
#else
        public void Initialize() {  LoadGameSettings();  }
        public void Sync() { }
#endif
        #endregion
    }
}