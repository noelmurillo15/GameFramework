/*
    *   SaveSettings - Save/Loads game settings to/from a JSON file
    *   Created by : Allan N. Murillo
 */
using System;
using System.IO;
using UnityEngine;
using GameFramework.Managers;


namespace GameFramework.External
{
    [System.Serializable]
    public class SaveSettings
    {
        #region Settings
        //  File
        static string jsonString;
        static string fileName = "GameSettings.json";

        //  Audio
        public float masterVolume;

        //  Graphics Preset
        public int currentQualityLevel;

        //  Advanced Graphics        
        public bool vsync;
        public float renderDistance;
        public float shadowDist;
        public int shadowCascade;
        public int msaa;
        public int anisoLevel;
        public int textureLimit;
        #endregion


        static object CreateJsonObj(string jsonString)
        {
            Debug.Log("GameSettings::Creating Json Object");
            return JsonUtility.FromJson<SaveSettings>(jsonString);
        }

        public bool LoadGameSettings()
        {
            Debug.Log("GameSettings::Loading from JSON");
            string path = Application.persistentDataPath + "/" + fileName;
            if (VerifyDirectory(path))
            {
                Debug.Log("GameSettings.Json Exists!");
                OverwriteGameSettings(File.ReadAllText(path));
                return true;
            }
            Debug.Log("GameSettings.Json does not exist");
            return false;
        }

        public void SaveGameSettings()
        {
            Debug.Log("GameSetting::Saving to JSON");
            string path = Application.persistentDataPath + "/" + fileName;

            //  Delete existing file
            if (VerifyDirectory(path)) { File.Delete(path); }

            //  Get Current Game Settings
            masterVolume = GameSettingsManager.masterVolumeINI;
            vsync = GameSettingsManager.vsyncINI;
            msaa = GameSettingsManager.msaaINI;
            renderDistance = GameSettingsManager.renderDistINI;
            textureLimit = GameSettingsManager.textureLimitINI;
            shadowDist = GameSettingsManager.shadowDistINI;
            shadowCascade = GameSettingsManager.shadowCascadeINI;
            anisoLevel = GameSettingsManager.anisoFilterLevelINI;
            currentQualityLevel = GameSettingsManager.currentQualityLevelINI;

            //  Write to Json Save file
            jsonString = JsonUtility.ToJson(this);
            File.WriteAllText(path, jsonString);

            // Debug.Log("Saving these Settings to JSON : " + "Vol : " + masterVolume + ", vsync : " + vsync +
            //     ", Preset " + currentQualityLevel + ", RenderDist : " + renderDistance + ", ShadowDist : " + shadowDist +
            //     ", cascade " + shadowCascade + ", MSAA : " + msaa + ", aniso : " + anisoLevel + ", texture limit : " + textureLimit);

            //  Sync with Browser's IndexedDB
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                Sync();
            }
        }

        void OverwriteGameSettings(String jsonString)
        {
            Debug.Log("GameSetting::Overwriting INI game settings");
            try
            {   //  Read settings from JSON file                
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

            //  Overwrite Game Settings
            GameSettingsManager.masterVolumeINI = masterVolume;
            GameSettingsManager.vsyncINI = vsync;
            GameSettingsManager.msaaINI = msaa;
            GameSettingsManager.renderDistINI = renderDistance;
            GameSettingsManager.textureLimitINI = textureLimit;
            GameSettingsManager.shadowDistINI = shadowDist;
            GameSettingsManager.shadowCascadeINI = shadowCascade;
            GameSettingsManager.anisoFilterLevelINI = anisoLevel;
            GameSettingsManager.currentQualityLevelINI = currentQualityLevel;
            GameSettingsManager.settingsLoadedINI = true;
        }

        bool VerifyDirectory(string filePath)
        {
            if (File.Exists(filePath)) { return true; }
            else { return false; }
        }


        #region External JS LIBRARY
#if UNITY_WEBGL && !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("__Internal")]
        static extern void InitilaizeJsLib();
        [System.Runtime.InteropServices.DllImport("__Internal")]
        static extern void SyncPersistantData();


        public void Initialize()
        {
            InitilaizeJsLib();
        }

        public void Sync()
        {
            //  Unity WebGL stores all files that must persist between sessions to the browser IndexedDB.
            //  This function makes sure Unity flushes all pending file system write operations to the IndexedDB file system from memory   
            SyncPersistantData();
        }
#else
        public void Initialize()
        {
            LoadGameSettings();
        }

        public void Sync()
        {
            Debug.Log("WebGL is not enabled -SyncPersistantData");
        }
#endif
        #endregion
    }
}