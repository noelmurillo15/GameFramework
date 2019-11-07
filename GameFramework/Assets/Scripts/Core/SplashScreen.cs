/*
    *   SplashScreen - Transitions to Main Menu after a delay
    *   Created by : Allan N. Murillo
 */
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


namespace GameFramework
{
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField] TMP_Text titleText = null;
        [SerializeField] int sceneIndex = 1;
        [SerializeField] float waitDelay = 3f;


        void Awake()
        {
            if (titleText != null)
            {
                titleText.text = Application.productName;
            }
            StartCoroutine(LoadNewScene());
        }

        IEnumerator LoadNewScene()
        {
            yield return new WaitForSeconds(waitDelay);
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneIndex);
            while (!async.isDone) { yield return null; }
        }
    }
}