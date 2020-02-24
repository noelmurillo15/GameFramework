/*
 * ScreenFade - Fades the screen in-between loading scenes
 * Created by : Allan N. Murillo
 * Last Edited : 2/22/2020
 */

using UnityEngine;
using System.Collections;
using ANM.Framework.Managers;
using ANM.Framework.Extensions;

namespace ANM.Framework.Utils
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ScreenFade : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeOutDelay = 1f;
        [SerializeField] private float fadeInDelay = 1f;
        private Coroutine _currentFade;
        
        
        private void Start()
        {
            if (gameObject.GetComponentInParent<GameManager>() != GameManager.Instance) return;
            SceneExtension.StartSceneLoadEvent += StartLoadSceneEvent;
            SceneExtension.FinishSceneLoadEvent += FinishLoadSceneEvent;
            canvasGroup = GetComponent<CanvasGroup>();
            FadeInImmediate();
        }
        
        private void OnDestroy()
        {
            if (gameObject.GetComponentInParent<GameManager>() != GameManager.Instance) return;
            SceneExtension.StartSceneLoadEvent -= StartLoadSceneEvent;
            SceneExtension.FinishSceneLoadEvent -= FinishLoadSceneEvent;
        }

        private void FadeOutImmediate() { canvasGroup.alpha = 1f; }
        private void FadeInImmediate() { canvasGroup.alpha = 0f; }
        
        private void StartLoadSceneEvent(bool wait)
        {
            if(!wait)
                FadeOutImmediate();
            else
            {
                FadeInImmediate();
                FadeOut();
            }
        }
        
        private void FinishLoadSceneEvent(bool wait)
        {
            if(!wait)
                FadeInImmediate();
            else
            {
                FadeOutImmediate();
                FadeIn();
            }
        }

        private Coroutine FadeOut()  {  return Fade(1f, fadeOutDelay);  }
        private Coroutine FadeIn()  {  return Fade(0f, fadeInDelay);  }

        private Coroutine Fade(float target, float time)
        {
            if (_currentFade != null) { StopCoroutine(_currentFade); }
            _currentFade = StartCoroutine(FadeRoutine(target, time));
            return _currentFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}
