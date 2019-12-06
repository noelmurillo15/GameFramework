/*
    *   SceneLoader - Transitions to Main Menu after a delay
    *   Created by : Allan N. Murillo
 */
using System.IO;
using UnityEngine;
using System.Collections;
using GameFramework.Events;
using UnityEngine.SceneManagement;


namespace GameFramework
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] string[] sceneNames = null;
        [SerializeField] int currentSceneIndex = 0;
        [SerializeField] float transitionDelay = 2f;

        public GameEvent OnLoadScene;
        public GameEvent OnFinishLoadScene;


        public void Initialize()
        {
            Debug.Log("SceneLoader::Initialize()");
            var sceneNumber = SceneManager.sceneCountInBuildSettings;
            sceneNames = new string[sceneNumber];

            for (int i = 0; i < sceneNumber; i++)
            {
                sceneNames[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            }

            if (SceneManager.GetActiveScene().name == sceneNames[0])
            {
                LoadMainMenu();
            }
        }

        public void LoadMainMenu()
        {
            Debug.Log("SceneLoader::LoadMainMenu()");
            StartCoroutine(LoadNewScene("MainMenu"));
        }

        public void LoadCreditsScene()
        {
            Debug.Log("SceneLoader::LoadCreditsScene()");
            StartCoroutine(LoadNewScene(sceneNames[sceneNames.Length - 1]));
        }

        public void LoadLevel(int index)
        {
            Debug.Log("SceneLoader::LoadLevel() : " + index);
            if (index > sceneNames.Length - 1)
            {
                Debug.Log("Scene Index out of range : " + index);
            }
            else
            {
                StartCoroutine(LoadNewScene(index));
            }
        }

        public string GetCurrentSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }

        IEnumerator LoadNewScene(int index)
        {
            OnLoadScene.Raise();
            yield return new WaitForSeconds(transitionDelay);
            AsyncOperation async = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
            while (!async.isDone) { yield return null; }
            yield return new WaitForSeconds(0.25f);
            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            OnFinishLoadScene.Raise();
        }

        IEnumerator LoadNewScene(string name)
        {
            OnLoadScene.Raise();
            yield return new WaitForSeconds(transitionDelay);
            AsyncOperation async = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
            while (!async.isDone) { yield return null; }
            yield return new WaitForSeconds(0.25f);
            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            OnFinishLoadScene.Raise();
        }
    }
}