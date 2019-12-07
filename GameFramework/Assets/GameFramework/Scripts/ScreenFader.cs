/*
    *   ScreenFader - Used for scene transitions
    *   Created by : Allan N. Murillo
 */
using UnityEngine;
using System.Collections;


namespace GameFramework
{
    public class ScreenFader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentFade = null;


        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public Coroutine FadeOut(float _time)
        {
            return Fade(1f, _time);
        }

        public Coroutine FadeIn(float _time)
        {
            return Fade(0f, _time);
        }

        public Coroutine Fade(float _target, float _time)
        {
            if (currentFade != null) { StopCoroutine(currentFade); }
            currentFade = StartCoroutine(FadeRoutine(_target, _time));
            return currentFade;
        }

        IEnumerator FadeRoutine(float _target, float _time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, _target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, _target, Time.deltaTime / _time);
                yield return null;
            }
        }

        public void LoadSceneEvent()
        {
            FadeOut(2f);
        }

        public void FinishLoadSceneEvent()
        {
            FadeIn(1f);
        }
    }
}