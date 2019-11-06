using TMPro;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


namespace GameFramework
{
    public class PauseManager : MonoBehaviour
    {
        #region PauseManager Members
        /// <summary>
        /// This is the main panel holder, which holds the main panel and should be called "main panel"
        /// </summary> 
        public GameObject mainPanel;
        /// <summary>
        /// This is the video panel holder, which holds all of the controls for the video panel and should be called "vid panel"
        /// </summary>
        public GameObject vidPanel;
        /// <summary>
        /// This is the audio panel holder, which holds all of the silders for the audio panel and should be called "audio panel"
        /// </summary>
        public GameObject audioPanel;
        /// <summary>
        /// These are the game objects with the title texts like "Pause menu" and "Game Title" 
        /// </summary>
        public GameObject TitleTexts;
        /// <summary>
        /// The mask that makes the scene darker  
        /// </summary>
        public GameObject mask;
        /// <summary>
        /// Audio Panel animator
        /// </summary>
        public Animator audioPanelAnimator;
        /// <summary>
        /// Video Panel animator  
        /// </summary>
        public Animator vidPanelAnimator;
        /// <summary>
        /// Quit Panel animator  
        /// </summary>
        public Animator quitPanelAnimator;
        /// <summary>
        /// Pause menu text 
        /// </summary>
        public TMP_Text pauseMenu;

        /// <summary>
        /// Main menu level string used for loading the main menu. This means you'll need to type in the editor text box, the name of the main menu level, ie: "mainmenu";
        /// </summary>
        public String mainMenu;
        //DOF script name
        /// <summary>
        /// The Depth of Field script name, ie: "DepthOfField". You can leave this blank in the editor, but will throw a null refrence exception, which is harmless.
        /// </summary>
        public String DOFScriptName;

        /// <summary>
        /// The Ambient Occlusion script name, ie: "AmbientOcclusion". You can leave this blank in the editor, but will throw a null refrence exception, which is harmless.
        /// </summary>
        public String AOScriptName;
        /// <summary>
        /// The main camera, assign this through the editor. 
        /// </summary>        
        public Camera mainCam;
        internal static Camera mainCamShared;
        /// <summary>
        /// The main camera game object, assign this through the editor. 
        /// </summary> 
        public GameObject mainCamObj;

        /// <summary>
        /// The terrain detail density float. It's only public because you may want to adjust it in editor
        /// </summary> 
        public float detailDensity;

        /// <summary>
        /// Timescale value. The defualt is 1 for most games. You may want to change it if you are pausing the game in a slow motion situation 
        /// </summary> 
        public float timeScale = 1f;
        /// <summary>
        /// Inital shadow distance 
        /// </summary>
        internal static float shadowDistINI;
        /// <summary>
        /// Inital render distance 
        /// </summary>
        internal static float renderDistINI;
        /// <summary>
        /// Inital AA quality 2, 4, or 8
        /// </summary>
        internal static float aaQualINI;
        /// <summary>
        /// Inital terrain detail density
        /// </summary>
        internal static float densityINI;
        /// <summary>
        /// Inital fov 
        /// </summary>
        internal static float fovINI;
        /// <summary>
        /// Inital msaa amount 
        /// </summary>
        internal static int msaaINI;
        /// <summary>
        /// Inital vsync count, the Unity docs say,
        /// <code> 
        /// //This will set the game to have one VSync per frame
        /// QualitySettings.vSyncCount = 1;
        /// </code>
        /// <code>
        /// //This will disable vsync
        /// QualitySettings.vSyncCount = 0;
        /// </code>
        /// </summary>
        internal static int vsyncINI;
        /// <summary>
        /// AA drop down menu.
        /// </summary>
        public Dropdown aaCombo;
        /// <summary>
        /// Aniso drop down menu.
        /// </summary>
        public Dropdown afCombo;

        public Slider fovSlider;
        public Slider modelQualSlider;
        public Slider terrainQualSlider;
        public Slider highQualTreeSlider;
        public Slider renderDistSlider;
        public Slider terrainDensitySlider;
        public Slider shadowDistSlider;
        public Slider audioMasterSlider;
        public Slider audioMusicSlider;
        public Slider audioEffectsSlider;
        public Slider masterTexSlider;
        public Slider shadowCascadesSlider;
        public Toggle vSyncToggle;
        public Toggle aoToggle;
        public Toggle dofToggle;
        public Toggle fullscreenToggle;
        /// <summary>
        /// The preset text label.
        /// </summary>
        public Text presetLabel;
        /// <summary>
        /// Resolution text label.
        /// </summary>
        public Text resolutionLabel;
        /// <summary>
        /// Lod bias float array. You should manually assign these based on the quality level.
        /// </summary>
        public float[] LODBias;
        /// <summary>
        /// Shadow distance array. You should manually assign these based on the quality level.
        /// </summary>
        public float[] shadowDist;
        /// <summary>
        /// An array of music audio sources
        /// </summary>
        public AudioSource[] music;
        /// <summary>
        /// An array of sound effect audio sources
        /// </summary>
        public AudioSource[] effects;
        /// <summary>
        /// An array of the other UI elements, which is used for disabling the other elements when the game is paused.
        /// </summary>
        public GameObject[] otherUIElements;
        /// <summary>
        /// Editor boolean for hardcoding certain video settings. It will allow you to use the values defined in LOD Bias and Shadow Distance
        /// </summary>
        public Boolean hardCodeSomeVideoSettings;
        /// <summary>
        /// Event system
        /// </summary>
        public EventSystem uiEventSystem;
        /// <summary>
        /// Defualt selected on the video panel
        /// </summary>
        public GameObject defualtSelectedVideo;
        /// <summary>
        /// Defualt selected on the video panel
        /// </summary>
        public GameObject defualtSelectedAudio;
        /// <summary>
        /// Defualt selected on the video panel
        /// </summary>
        public GameObject defualtSelectedMain;
        //last music multiplier; this should be a value between 0-1
        internal static float lastMusicMult;
        //last audio multiplier; this should be a value between 0-1
        internal static float lastAudioMult;
        //Initial master volume
        internal static float beforeMaster;
        //last texture limit 
        internal static int lastTexLimit;
        //int for amount of effects
        private int _audioEffectAmt = 0;
        //Inital audio effect volumes
        private float[] _beforeEffectVol;

        //Initial music volume
        private float _beforeMusic = 1f;
        //Preset level
        private int _currentLevel;
        //Resoutions
        private Resolution[] allRes;
        //Camera dof script
        private MonoBehaviour tempScript;
        //Presets 
        private String[] presets;
        //Fullscreen Boolean
        private Boolean isFullscreen;
        //current resoultion
        internal static Resolution currentRes;
        //Last resoultion 
        private Resolution beforeRes;

        //last shadow cascade value
        internal static int lastShadowCascade;

        public static Boolean aoBool;
        public static Boolean dofBool;
        private Boolean lastAOBool;
        private Boolean lastDOFBool;
        public static Terrain readTerrain;
        public static Terrain readSimpleTerrain;

        private SaveSettings saveSettings = new SaveSettings();
        #endregion


        public void Start()
        {   //  Default Values
            mainCamShared = mainCam;
            uiEventSystem.firstSelectedGameObject = defualtSelectedMain;

            //  Graphics Quality Presets
            presets = QualitySettings.names;
            presetLabel.text = presets[QualitySettings.GetQualityLevel()].ToString();
            _currentLevel = QualitySettings.GetQualityLevel();

            //  Resolutions
            allRes = Screen.resolutions;
            currentRes = Screen.currentResolution;
            resolutionLabel.text = Screen.currentResolution.width.ToString() + " x " + Screen.currentResolution.height.ToString();
            isFullscreen = Screen.fullScreen;

            //  Audio Volume
            _beforeEffectVol = new float[_audioEffectAmt];
            beforeMaster = AudioListener.volume;

            lastMusicMult = audioMusicSlider.value;
            lastAudioMult = audioEffectsSlider.value;

            //  Graphics Settings
            aaQualINI = QualitySettings.antiAliasing;
            renderDistINI = mainCam.farClipPlane;
            shadowDistINI = QualitySettings.shadowDistance;
            fovINI = mainCam.fieldOfView;
            msaaINI = QualitySettings.antiAliasing;
            vsyncINI = QualitySettings.vSyncCount;
            lastAOBool = aoToggle.isOn;
            lastDOFBool = dofToggle.isOn;
            lastTexLimit = QualitySettings.masterTextureLimit;
            lastShadowCascade = QualitySettings.shadowCascades;

            if (saveSettings.DirectoryExists(Application.persistentDataPath + "/" + saveSettings.fileName))
            {   //  Override Settings if file Exists
                saveSettings.LoadGameSettings(File.ReadAllText(Application.persistentDataPath + "/" + saveSettings.fileName));
            }

            //  Enable Menu Panels
            mask.SetActive(true);
            mainPanel.SetActive(true);
            TitleTexts.SetActive(true);
            audioPanel.SetActive(false);
            vidPanel.SetActive(false);

            if (TitleTexts != null)
            {
                TitleTexts.transform.GetChild(0).GetComponent<TMP_Text>().text = Application.productName;
            }

            for (int i = 0; i < otherUIElements.Length; i++)
            {
                otherUIElements[i].gameObject.SetActive(false);
            }
        }

        public void Update()
        {
            if (vidPanel.activeSelf == true)
            {
                pauseMenu.text = "Video Menu";
            }
            else if (audioPanel.activeSelf == true)
            {
                pauseMenu.text = "Audio Menu";
            }
            else if (mainPanel.activeSelf == true)
            {
                pauseMenu.text = "Main Menu";
            }

            if (Input.GetKeyDown(KeyCode.Escape) && mainPanel.activeSelf == false)
            {
                Pause();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && mainPanel.activeSelf == true)
            {
                Resume();
            }
        }

        #region Buttons
        /// <summary>
        /// Restart the level by loading the loaded level.
        /// </summary>
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            uiEventSystem.firstSelectedGameObject = defualtSelectedMain;
        }
        /// <summary>
        /// 
        /// </summary>  
        public void LoadGame()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public void StartGame()
        {
            StartCoroutine(LoadNewScene());
        }

        IEnumerator LoadNewScene()
        {
            yield return GameManager.Instance.screenFader.FadeOut(2f);
            AsyncOperation async = SceneManager.LoadSceneAsync("Level 1");
            GameManager.Instance.screenFader.FadeIn(2f);
            while (!async.isDone) { yield return null; }
        }
        /// <summary>
        /// Method to resume the game, so disable the pause menu and re-enable all other ui elements
        /// </summary>
        public void Pause()
        {
            Time.timeScale = 0;
            uiEventSystem.SetSelectedGameObject(defualtSelectedMain);
            mainPanel.SetActive(true);
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            TitleTexts.SetActive(true);
            mask.SetActive(true);
            for (int i = 0; i < otherUIElements.Length; i++)
            {
                otherUIElements[i].gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// Method to resume the game, so disable the pause menu and re-enable all other ui elements
        /// </summary>
        public void Resume()
        {
            Time.timeScale = timeScale;
            mainPanel.SetActive(false);
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            TitleTexts.SetActive(false);
            mask.SetActive(false);
            for (int i = 0; i < otherUIElements.Length; i++)
            {
                otherUIElements[i].gameObject.SetActive(true);
            }
        }
        /// <summary>
        /// All the methods relating to qutting should be called here.
        /// </summary>
        public void quitOptions()
        {
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            quitPanelAnimator.enabled = true;
            quitPanelAnimator.Play("QuitPanelIn");

        }
        /// <summary>
        /// Method to quit the game. Call methods such as auto saving before qutting here.
        /// </summary>
        public void quitGame()
        {
            GameManager.Instance.QuitApplicationEvent();
            Invoke("Quit", 1f);
        }
        void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        /// <summary>
        /// Cancels quittting by playing an animation.
        /// </summary>
        public void quitCancel()
        {
            quitPanelAnimator.Play("QuitPanelOut");
        }
        /// <summary>
        ///Loads the main menu scene.
        /// </summary>
        public void returnToMenu()
        {
            SceneManager.LoadScene(mainMenu);
            uiEventSystem.SetSelectedGameObject(defualtSelectedMain);
        }
        #endregion

        #region Audio Panel
        /// <summary>
        /// Show the audio panel 
        /// </summary>
        public void Audio()
        {
            vidPanel.SetActive(false);
            audioPanel.SetActive(true);
            audioPanelAnimator.enabled = true;
            audioIn();
        }
        /// <summary>
        /// Play the "audio panel in" animation.
        /// </summary>
        public void audioIn()
        {
            uiEventSystem.SetSelectedGameObject(defualtSelectedAudio);
            audioPanelAnimator.Play("Audio Panel In");
            audioMasterSlider.value = AudioListener.volume;
            //Perform modulo to find factor f to allow for non uniform music volumes
            float a; float b; float f;
            try
            {
                a = music[0].volume;
                b = music[1].volume;
                f = a % b;
                audioMusicSlider.value = f;
            }
            catch
            {
                Debug.Log("You do not have multiple audio sources");
                audioMusicSlider.value = lastMusicMult;
            }
            //Do this with the effects
            try
            {
                a = effects[0].volume;
                b = effects[1].volume;
                f = a % b;
                audioEffectsSlider.value = f;
            }
            catch
            {
                Debug.Log("You do not have multiple audio sources");
                audioEffectsSlider.value = lastAudioMult;
            }

        }
        /// <summary>
        /// Audio Option Methods
        /// </summary>
        /// <param name="f"></param>
        public void updateMasterVol(float f)
        {

            //Controls volume of all audio listeners 
            AudioListener.volume = f;
        }
        /// <summary>
        /// Update music effects volume
        /// </summary>
        /// <param name="f"></param>
        public void updateMusicVol(float f)
        {
            try
            {
                for (int _musicAmt = 0; _musicAmt < music.Length; _musicAmt++)
                {
                    music[_musicAmt].volume *= f;
                }
            }
            catch
            {
                Debug.Log("Please assign music sources in the manager");
            }
            //_beforeMusic = music.volume;
        }
        /// <summary>
        /// Update the audio effects volume
        /// </summary>
        /// <param name="f"></param>
        public void updateEffectsVol(float f)
        {
            try
            {
                for (_audioEffectAmt = 0; _audioEffectAmt < effects.Length; _audioEffectAmt++)
                {
                    //get the values for all effects before the change
                    _beforeEffectVol[_audioEffectAmt] = effects[_audioEffectAmt].volume;

                    //lower it by a factor of f because we don't want every effect to be set to a uniform volume
                    effects[_audioEffectAmt].volume *= f;
                }
            }
            catch
            {
                Debug.Log("Please assign audio effects sources in the manager.");
            }

        }
        /// <summary> 
        /// The method for changing the applying new audio settings
        /// </summary>
        public void applyAudio()
        {
            StartCoroutine(applyAudioMain());
            uiEventSystem.SetSelectedGameObject(defualtSelectedMain);

        }
        /// <summary>
        /// Use an IEnumerator to first play the animation and then change the audio settings
        /// </summary>
        /// <returns></returns>
        internal IEnumerator applyAudioMain()
        {
            audioPanelAnimator.Play("Audio Panel Out");
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.5f));
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            beforeMaster = AudioListener.volume;
            lastMusicMult = audioMusicSlider.value;
            lastAudioMult = audioEffectsSlider.value;
            saveSettings.SaveGameSettings();
        }
        /// <summary>
        /// Cancel the audio setting changes
        /// </summary>
        public void cancelAudio()
        {
            uiEventSystem.SetSelectedGameObject(defualtSelectedMain);
            StartCoroutine(cancelAudioMain());
        }
        /// <summary>
        /// Use an IEnumerator to first play the animation and then change the audio settings
        /// </summary>
        /// <returns></returns>
        internal IEnumerator cancelAudioMain()
        {
            audioPanelAnimator.Play("Audio Panel Out");
            Debug.Log(audioPanelAnimator.GetCurrentAnimatorClipInfo(0).Length);
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.5f));
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            AudioListener.volume = beforeMaster;
            //Debug.Log(_beforeMaster + AudioListener.volume);
            try
            {


                for (_audioEffectAmt = 0; _audioEffectAmt < effects.Length; _audioEffectAmt++)
                {
                    //get the values for all effects before the change
                    effects[_audioEffectAmt].volume = _beforeEffectVol[_audioEffectAmt];
                }
                for (int _musicAmt = 0; _musicAmt < music.Length; _musicAmt++)
                {
                    music[_musicAmt].volume = _beforeMusic;
                }
            }
            catch
            {
                Debug.Log("please assign the audio sources in the manager");
            }
        }
        #endregion

        #region Video Panel
        /////Video Options
        /// <summary>
        /// Show video
        /// </summary>
        public void Video()
        {
            vidPanel.SetActive(true);
            audioPanel.SetActive(false);
            vidPanelAnimator.enabled = true;
            videoIn();
        }
        /// <summary>
        /// Play the "video panel in" animation
        /// </summary>
        public void videoIn()
        {
            uiEventSystem.SetSelectedGameObject(defualtSelectedVideo);
            vidPanelAnimator.Play("Video Panel In");

            if (QualitySettings.antiAliasing == 0)
            {
                aaCombo.value = 0;
            }
            else if (QualitySettings.antiAliasing == 2)
            {
                aaCombo.value = 1;
            }
            else if (QualitySettings.antiAliasing == 4)
            {
                aaCombo.value = 2;
            }
            else if (QualitySettings.antiAliasing == 8)
            {
                aaCombo.value = 3;
            }
            if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.ForceEnable)
            {
                afCombo.value = 1;
            }
            else if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.Disable)
            {
                afCombo.value = 0;
            }
            else if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.Enable)
            {
                afCombo.value = 2;
            }
            presetLabel.text = presets[QualitySettings.GetQualityLevel()].ToString();
            fovSlider.value = mainCam.fieldOfView;
            modelQualSlider.value = QualitySettings.lodBias;
            renderDistSlider.value = mainCam.farClipPlane;
            shadowDistSlider.value = QualitySettings.shadowDistance;
            masterTexSlider.value = QualitySettings.masterTextureLimit;
            shadowCascadesSlider.value = QualitySettings.shadowCascades;
            fullscreenToggle.isOn = Screen.fullScreen;
            aoToggle.isOn = aoBool;
            dofToggle.isOn = dofBool;
            if (QualitySettings.vSyncCount == 0)
            {
                vSyncToggle.isOn = false;
            }
            else if (QualitySettings.vSyncCount == 1)
            {
                vSyncToggle.isOn = true;
            }
        }

        /// <summary>
        /// Cancel the video setting changes 
        /// </summary>
        public void cancelVideo()
        {
            uiEventSystem.SetSelectedGameObject(defualtSelectedMain);
            StartCoroutine(cancelVideoMain());
        }
        /// <summary>
        /// Use an IEnumerator to first play the animation and then changethe video settings
        /// </summary>
        /// <returns></returns>
        internal IEnumerator cancelVideoMain()
        {
            vidPanelAnimator.Play("Video Panel Out");

            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.5f));
            try
            {
                mainCam.farClipPlane = renderDistINI;
                Terrain.activeTerrain.detailObjectDensity = densityINI;
                mainCam.fieldOfView = fovINI;
                vidPanel.SetActive(false);
                audioPanel.SetActive(false);
                aoBool = lastAOBool;
                dofBool = lastDOFBool;
                Screen.SetResolution(beforeRes.width, beforeRes.height, Screen.fullScreen);
                QualitySettings.shadowDistance = shadowDistINI;
                QualitySettings.antiAliasing = (int)aaQualINI;
                QualitySettings.antiAliasing = msaaINI;
                QualitySettings.vSyncCount = vsyncINI;
                QualitySettings.masterTextureLimit = lastTexLimit;
                QualitySettings.shadowCascades = lastShadowCascade;
                Screen.fullScreen = isFullscreen;
            }
            catch
            {

                Debug.Log("A problem occured (chances are the terrain was not assigned )");
                mainCam.farClipPlane = renderDistINI;
                mainCam.fieldOfView = fovINI;
                vidPanel.SetActive(false);
                audioPanel.SetActive(false);
                aoBool = lastAOBool;
                dofBool = lastDOFBool;
                QualitySettings.shadowDistance = shadowDistINI;
                Screen.SetResolution(beforeRes.width, beforeRes.height, Screen.fullScreen);
                QualitySettings.antiAliasing = (int)aaQualINI;
                QualitySettings.antiAliasing = msaaINI;
                QualitySettings.vSyncCount = vsyncINI;
                QualitySettings.masterTextureLimit = lastTexLimit;
                QualitySettings.shadowCascades = lastShadowCascade;
                //Screen.fullScreen = isFullscreen;

            }

        }
        //Apply the video prefs
        /// <summary>
        /// Apply the video settings
        /// </summary>
        public void apply()
        {
            StartCoroutine(applyVideo());
            uiEventSystem.SetSelectedGameObject(defualtSelectedMain);

        }
        /// <summary>
        /// Use an IEnumerator to first play the animation and then change the video settings.
        /// </summary>
        /// <returns></returns>
        internal IEnumerator applyVideo()
        {
            vidPanelAnimator.Play("Video Panel Out");
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.5f));
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            renderDistINI = mainCam.farClipPlane;
            shadowDistINI = QualitySettings.shadowDistance;
            Debug.Log("Shadow dist ini" + shadowDistINI);
            fovINI = mainCam.fieldOfView;
            aoBool = aoToggle.isOn;
            dofBool = dofToggle.isOn;
            lastAOBool = aoBool;
            lastDOFBool = dofBool;
            beforeRes = currentRes;
            lastTexLimit = QualitySettings.masterTextureLimit;
            lastShadowCascade = QualitySettings.shadowCascades;
            vsyncINI = QualitySettings.vSyncCount;
            isFullscreen = Screen.fullScreen;
            saveSettings.SaveGameSettings();

        }
        /// <summary>
        /// Video Options
        /// </summary>
        /// <param name="B"></param>
        public void toggleVSync(Boolean B)
        {
            vsyncINI = QualitySettings.vSyncCount;
            if (B == true)
            {
                QualitySettings.vSyncCount = 1;
            }
            else
            {
                QualitySettings.vSyncCount = 0;
            }

        }
        /// <summary>
        /// Change the lod bias using
        /// <c>
        /// QualitySettings.lodBias = LoDBias / 2.15f;
        /// </c> 
        /// LoDBias is only divided by 2.15 because the max is set to 10 on the slider, and dividing by 2.15 results in 4.65, our desired max. However, deleting or changing 2.15 is compeletly fine.
        /// </summary>
        /// <param name="LoDBias"></param>
        public void lodBias(float LoDBias)
        {
            QualitySettings.lodBias = LoDBias / 2.15f;
        }
        /// <summary>
        /// Update the render distance using 
        /// <c>
        /// mainCam.farClipPlane = f;
        /// </c>
        /// </summary>
        /// <param name="f"></param>
        public void updateRenderDist(float f)
        {
            try
            {
                mainCam.farClipPlane = f;

            }
            catch
            {
                Debug.Log(" Finding main camera now...it is still suggested that you manually assign this");
                mainCam = Camera.main;
                mainCam.farClipPlane = f;

            }

        }
        /// <summary>
        /// Update the texture quality using  
        /// <c>QualitySettings.masterTextureLimit </c>
        /// </summary>
        /// <param name="qual"></param>
        public void updateTex(float qual)
        {

            int f = Mathf.RoundToInt(qual);
            QualitySettings.masterTextureLimit = f;
        }
        /// <summary>
        /// Update the shadow distance using 
        /// <c>
        /// QualitySettings.shadowDistance = dist;
        /// </c>
        /// </summary>
        /// <param name="dist"></param>
        public void updateShadowDistance(float dist)
        {
            QualitySettings.shadowDistance = dist;

        }
        /// <summary>
        /// Change the fov using a float. The defualt should be 60.
        /// </summary>
        /// <param name="fov"></param>
        public void updateFOV(float fov)
        {
            mainCam.fieldOfView = fov;
        }
        /// <summary>
        /// Toggle on or off Depth of Field. This is meant to be used with a checkbox.
        /// </summary>
        /// <param name="b"></param>
        public void toggleDOF(Boolean b)
        {
            try
            {
                tempScript = (MonoBehaviour)mainCamObj.GetComponent(DOFScriptName);

                if (b == true)
                {
                    tempScript.enabled = true;
                    dofBool = true;
                }
                else
                {
                    tempScript.enabled = false;
                    dofBool = false;
                }
            }
            catch
            {
                Debug.Log("No AO post processing found");
                return;
            }



        }
        /// <summary>
        /// Toggle on or off Ambient Occulusion. This is meant to be used with a checkbox.
        /// </summary>
        /// <param name="b"></param>
        public void toggleAO(Boolean b)
        {
            try
            {

                tempScript = (MonoBehaviour)mainCamObj.GetComponent(AOScriptName);

                if (b == true)
                {
                    tempScript.enabled = true;
                    aoBool = true;
                }
                else
                {
                    tempScript.enabled = false;
                    aoBool = false;
                }
            }
            catch
            {
                Debug.Log("No AO post processing found");
                return;
            }
        }
        /// <summary>
        /// Set the game to windowed or full screen. This is meant to be used with a checkbox
        /// </summary>
        /// <param name="b"></param>
        public void setFullScreen(Boolean b)
        {


            if (b == true)
            {
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            }
            else
            {
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
            }
        }
        /// <summary>
        /// Method for moving to the next resoution in the allRes array. WARNING: This is not finished/buggy.  
        /// </summary>
        //Method for moving to the next resoution in the allRes array. WARNING: This is not finished/buggy.  
        public void nextRes()
        {
            beforeRes = currentRes;
            //Iterate through all of the resoultions. 
            for (int i = 0; i < allRes.Length; i++)
            {
                //If the resoultion matches the current resoution height and width then go through the statement.
                if (allRes[i].height == currentRes.height && allRes[i].width == currentRes.width)
                {
                    //Debug.Log("found " + i);
                    //If the user is playing fullscreen. Then set the resoution to one element higher in the array, set the full screen boolean to true, reset the current resolution, and then update the resolution label.
                    if (isFullscreen == true) { Screen.SetResolution(allRes[i + 1].width, allRes[i + 1].height, true); isFullscreen = true; currentRes = Screen.resolutions[i + 1]; resolutionLabel.text = currentRes.width.ToString() + " x " + currentRes.height.ToString(); }
                    //If the user is playing in a window. Then set the resoution to one element higher in the array, set the full screen boolean to false, reset the current resolution, and then update the resolution label.
                    if (isFullscreen == false) { Screen.SetResolution(allRes[i + 1].width, allRes[i + 1].height, false); isFullscreen = false; currentRes = Screen.resolutions[i + 1]; resolutionLabel.text = currentRes.width.ToString() + " x " + currentRes.height.ToString(); }

                    //Debug.Log("Res after: " + currentRes);
                }
            }

        }
        /// <summary>
        /// Method for moving to the last resoution in the allRes array. WARNING: This is not finished/buggy.  
        /// </summary>
        //Method for moving to the last resoution in the allRes array. WARNING: This is not finished/buggy.  
        public void lastRes()
        {
            beforeRes = currentRes;
            //Iterate through all of the resoultions. 
            for (int i = 0; i < allRes.Length; i++)
            {
                if (allRes[i].height == currentRes.height && allRes[i].width == currentRes.width)
                {

                    //Debug.Log("found " + i);
                    //If the user is playing fullscreen. Then set the resoution to one element lower in the array, set the full screen boolean to true, reset the current resolution, and then update the resolution label.
                    if (isFullscreen == true) { Screen.SetResolution(allRes[i - 1].width, allRes[i - 1].height, true); isFullscreen = true; currentRes = Screen.resolutions[i - 1]; resolutionLabel.text = currentRes.width.ToString() + " x " + currentRes.height.ToString(); }
                    //If the user is playing in a window. Then set the resoution to one element lower in the array, set the full screen boolean to false, reset the current resolution, and then update the resolution label.
                    if (isFullscreen == false) { Screen.SetResolution(allRes[i - 1].width, allRes[i - 1].height, false); isFullscreen = false; currentRes = Screen.resolutions[i - 1]; resolutionLabel.text = currentRes.width.ToString() + " x " + currentRes.height.ToString(); }

                    //Debug.Log("Res after: " + currentRes);
                }
            }

        }
        /// <summary>
        /// Force aniso on using quality settings
        /// </summary>
        //Force the aniso on.
        public void forceOnANISO()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        }
        /// <summary>
        /// Per texture aniso using quality settings
        /// </summary>
        //Use per texture aniso settings.
        public void perTexANISO()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
        }
        /// <summary>
        /// Disable aniso using quality setttings
        /// </summary>
        //Disable aniso all together.
        public void disableANISO()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
        }
        /// <summary>
        /// The method for changing aniso settings
        /// </summary>
        /// <param name="anisoSetting"></param>
        public void updateANISO(int anisoSetting)
        {
            if (anisoSetting == 0)
            {
                disableANISO();
            }
            else if (anisoSetting == 1)
            {
                forceOnANISO();
            }
            else if (anisoSetting == 2)
            {
                perTexANISO();
            }
        }

        /// <summary>
        /// The method for setting the amount of shadow cascades
        /// </summary>
        /// <param name="cascades"></param>
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
        }
        /// <summary>
        /// Update MSAA quality using quality settings
        /// </summary>
        /// <param name="msaaAmount"></param>
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

        }
        /// <summary>
        /// Set MSAA to 0x (disabling it) using quality settings
        /// </summary>
        public void disableMSAA()
        {

            QualitySettings.antiAliasing = 0;
            // aaOption.text = "MSAA: " + QualitySettings.antiAliasing.ToString();
        }
        /// <summary>
        /// Set MSAA to 2x using quality settings
        /// </summary>
        public void twoMSAA()
        {

            QualitySettings.antiAliasing = 2;
            // aaOption.text = "MSAA: " + QualitySettings.antiAliasing.ToString();
        }
        /// <summary>
        /// Set MSAA to 4x using quality settings
        /// </summary>
        public void fourMSAA()
        {

            QualitySettings.antiAliasing = 4;

            // aaOption.text = "MSAA: " + QualitySettings.antiAliasing.ToString();
        }
        /// <summary>
        /// Set MSAA to 8x using quality settings
        /// </summary>
        public void eightMSAA()
        {

            QualitySettings.antiAliasing = 8;
            // aaOption.text = "MSAA: " + QualitySettings.antiAliasing.ToString();
        }
        /// <summary>
        /// Set the quality level one level higher. This is done by getting the current quality level, then using 
        /// <c> 
        /// QualitySettings.IncreaseLevel();
        /// </c>
        /// to increase the level. The current level variable is set to the new quality setting, and the label is updated.
        /// </summary>
        public void nextPreset()
        {
            _currentLevel = QualitySettings.GetQualityLevel();
            QualitySettings.IncreaseLevel();
            _currentLevel = QualitySettings.GetQualityLevel();
            presetLabel.text = presets[_currentLevel].ToString();
            if (hardCodeSomeVideoSettings == true)
            {
                QualitySettings.shadowDistance = shadowDist[_currentLevel];
                QualitySettings.lodBias = LODBias[_currentLevel];
            }
        }
        /// <summary>
        /// Set the quality level one level lower. This is done by getting the current quality level, then using 
        /// <c> 
        /// QualitySettings.DecreaseLevel();
        /// </c>
        /// to decrease the level. The current level variable is set to the new quality setting, and the label is updated.
        /// </summary>
        public void lastPreset()
        {
            _currentLevel = QualitySettings.GetQualityLevel();
            QualitySettings.DecreaseLevel();
            _currentLevel = QualitySettings.GetQualityLevel();
            presetLabel.text = presets[_currentLevel].ToString();
            if (hardCodeSomeVideoSettings == true)
            {
                QualitySettings.shadowDistance = shadowDist[_currentLevel];
                QualitySettings.lodBias = LODBias[_currentLevel];
            }

        }
        /// <summary>
        /// Hard code the minimal settings
        /// </summary>
        public void setMinimal()
        {
            QualitySettings.SetQualityLevel(0);
            //QualitySettings.shadowDistance = 12.6f;
            QualitySettings.shadowDistance = shadowDist[0];
            //QualitySettings.lodBias = 0.3f;
            QualitySettings.lodBias = LODBias[0];
        }
        /// <summary>
        /// Hard code the very low settings
        /// </summary>
        public void setVeryLow()
        {
            QualitySettings.SetQualityLevel(1);
            //QualitySettings.shadowDistance = 17.4f;
            QualitySettings.shadowDistance = shadowDist[1];
            //QualitySettings.lodBias = 0.55f;
            QualitySettings.lodBias = LODBias[1];
        }
        /// <summary>
        /// Hard code the low settings
        /// </summary>
        public void setLow()
        {
            QualitySettings.SetQualityLevel(2);
            //QualitySettings.shadowDistance = 29.7f;
            //QualitySettings.lodBias = 0.68f;
            QualitySettings.lodBias = LODBias[2];
            QualitySettings.shadowDistance = shadowDist[2];
        }
        /// <summary>
        /// Hard code the normal settings
        /// </summary>
        public void setNormal()
        {
            QualitySettings.SetQualityLevel(3);
            //QualitySettings.shadowDistance = 82f;
            //QualitySettings.lodBias = 1.09f;
            QualitySettings.shadowDistance = shadowDist[3];
            QualitySettings.lodBias = LODBias[3];
        }
        /// <summary>
        /// Hard code the very high settings
        /// </summary>
        public void setVeryHigh()
        {
            QualitySettings.SetQualityLevel(4);
            //QualitySettings.shadowDistance = 110f;
            //QualitySettings.lodBias = 1.22f;
            QualitySettings.shadowDistance = shadowDist[4];
            QualitySettings.lodBias = LODBias[4];
        }
        /// <summary>
        /// Hard code the ultra settings
        /// </summary>
        public void setUltra()
        {
            QualitySettings.SetQualityLevel(5);
            //QualitySettings.shadowDistance = 338f;
            //QualitySettings.lodBias = 1.59f;
            QualitySettings.shadowDistance = shadowDist[5];
            QualitySettings.lodBias = LODBias[5];
        }
        /// <summary>
        /// Hard code the extreme settings
        /// </summary>
        public void setExtreme()
        {
            QualitySettings.SetQualityLevel(6);
            //QualitySettings.shadowDistance = 800f;
            //QualitySettings.lodBias = 4.37f;
            QualitySettings.shadowDistance = shadowDist[6];
            QualitySettings.lodBias = LODBias[6];
        }
        #endregion
    }
}