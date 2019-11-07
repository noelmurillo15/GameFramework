/*
    *   SaveSettings - Save/Loads game settings from a JSON file 
 */
using System;
using System.IO;
using UnityEngine;


namespace GameFramework.Core
{
    [System.Serializable]
    public class SaveSettings
    {
        #region Settings
        //  File
        static string jsonString;
        static string fileName = "GameSettings.json";

        //  Audio
        public float musicVolume;
        public float effectsVolume;
        public float masterVolume;

        //  Graphics
        public int vsyncINI;
        public int curQualityLevel;
        public bool fullscreenBool;
        public int resHeight;
        public int resWidth;

        //  Advanced Graphics        
        public int msaaINI;
        public float msaaQualityINI;
        public float renderDistanceINI;
        public float shadowDistINI;
        public int textureLimit;
        public int anisoTextureLevel;
        public bool aoBool;
        public bool dofBool;
        public int lastShadowCascade;

        //  Camera
        public float fovINI;
        #endregion


        bool DirectoryExists(string path)
        {
            if (File.Exists(path)) { return true; }
            else { return false; }
        }

        public bool LoadFromJson(bool full)
        {
            if (DirectoryExists(Application.persistentDataPath + "/" + fileName))
            {
                Load(File.ReadAllText(Application.persistentDataPath + "/" + fileName), full);
                return true;
            }
            return false;
        }

        void Load(String readString, bool full)
        {
            Debug.Log("LoadSettings::ApplySettings() : " + readString);
            try
            {
                SaveSettings read = (SaveSettings)createJSONOBJ(readString);
                resWidth = read.resWidth;
                resHeight = read.resHeight;
                fullscreenBool = read.fullscreenBool;
                masterVolume = read.masterVolume;
                musicVolume = read.musicVolume;
                effectsVolume = read.effectsVolume;
                renderDistanceINI = read.renderDistanceINI;
                fovINI = read.fovINI;
                msaaQualityINI = read.msaaQualityINI;
                shadowDistINI = read.shadowDistINI;
                msaaINI = read.msaaINI;
                vsyncINI = read.vsyncINI;
                textureLimit = read.textureLimit;
                curQualityLevel = read.curQualityLevel;
                lastShadowCascade = read.lastShadowCascade;
                dofBool = read.dofBool;
                aoBool = read.aoBool;
                anisoTextureLevel = read.anisoTextureLevel;
            }
            catch (FileNotFoundException)
            {
                Debug.LogError("Game settings not found in: " + readString);
                return;
            }

#if UNITY_STANDALONE
            Screen.SetResolution(resWidth, resHeight, fullscreenBool);
#endif
            //  Audio Settings
            AudioListener.volume = masterVolume;
            // myManager.pauseManager.lastMusicMult = musicVolume;
            // myManager.pauseManager.lastAudioMult = effectsVolume;

            //  Camera Settings
            Camera.main.farClipPlane = renderDistanceINI;
            Camera.main.fieldOfView = fovINI;

            //  Graphics Settings
            if (full)
            {
                QualitySettings.antiAliasing = (int)msaaQualityINI;
                QualitySettings.shadowDistance = shadowDistINI;
                QualitySettings.antiAliasing = msaaINI;
                QualitySettings.vSyncCount = vsyncINI;
                QualitySettings.masterTextureLimit = textureLimit;
                QualitySettings.SetQualityLevel(curQualityLevel);
                QualitySettings.shadowCascades = lastShadowCascade;
                // myManager.pauseManager.dofBool = dofBool;
                // myManager.pauseManager.aoBool = aoBool;

                if (anisoTextureLevel == 0)
                {
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                }
                else if (anisoTextureLevel == 1)
                {
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                }
                else if (anisoTextureLevel == 2)
                {
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                }
            }
        }

        public void SaveToJson()
        {
            //  Delete Existing Json File
            if (DirectoryExists(Application.persistentDataPath + "/" + fileName))
            {
                File.Delete(Application.persistentDataPath + "/" + fileName);
            }

            //  Screen Settings
            resHeight = Screen.currentResolution.height;
            resWidth = Screen.currentResolution.width;
            fullscreenBool = Screen.fullScreen;

            //  Audio Settings
            masterVolume = AudioListener.volume;
            effectsVolume = 1f;
            musicVolume = 1f;

            //  Camera Settings
            renderDistanceINI = Camera.main.farClipPlane;
            fovINI = Camera.main.fieldOfView;

            //  Graphics Settings
            curQualityLevel = QualitySettings.GetQualityLevel();
            vsyncINI = QualitySettings.vSyncCount;
            msaaINI = QualitySettings.antiAliasing;
            msaaQualityINI = QualitySettings.antiAliasing;
            textureLimit = QualitySettings.masterTextureLimit;
            shadowDistINI = QualitySettings.shadowDistance;
            lastShadowCascade = QualitySettings.shadowCascades;
            aoBool = false;
            dofBool = false;

            if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.Disable)
            {
                anisoTextureLevel = 0;
            }
            else if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.ForceEnable)
            {
                anisoTextureLevel = 1;
            }
            else if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.Enable)
            {
                anisoTextureLevel = 2;
            }

            //  Write to Json file
            jsonString = JsonUtility.ToJson(this);
            File.WriteAllText(Application.persistentDataPath + "/" + fileName, jsonString);
            Debug.Log("SaveSettings::SaveToJson() : " + jsonString);
        }

        public static object createJSONOBJ(string jsonString)
        {
            return JsonUtility.FromJson<SaveSettings>(jsonString);
        }
    }
}