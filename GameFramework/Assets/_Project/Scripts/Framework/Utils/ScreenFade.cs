/*
 * ScreenFade - Fades the screen in-between loading scenes
 * Created by : Allan N. Murillo
 * Last Edited : 5/27/2020
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


        private void Awake()
        {
            if (gameObject.GetComponentInParent<GameManager>() != GameManager.Instance) return;
            SceneExtension.FinishSceneLoadEvent += FinishLoadScene;
            SceneExtension.StartSceneLoadEvent += StartLoadScene;
            canvasGroup = GetComponent<CanvasGroup>();
            FadeInImmediate();
        }

        private void OnDestroy()
        {
            if (gameObject.GetComponentInParent<GameManager>() != GameManager.Instance) return;
            SceneExtension.FinishSceneLoadEvent -= FinishLoadScene;
            SceneExtension.StartSceneLoadEvent -= StartLoadScene;
        }

        private void StartLoadScene(bool fade, bool save)
        {
            if (!fade) FadeOutImmediate();
            else
            {
                FadeInImmediate();
                FadeOut();
            }
        }

        private void FinishLoadScene(bool fade, bool save)
        {
            if (!fade) FadeInImmediate();
            else
            {
                FadeOutImmediate();
                FadeIn();
            }
        }

        public void FadeInImmediate()
        {
            canvasGroup.alpha = 0f;
        }

        public void FadeOutImmediate(float amount)
        {
            canvasGroup.alpha = amount;
        }

        private void FadeOutImmediate()
        {
            canvasGroup.alpha = 1f;
        }

        private void FadeOut()
        {
            Fade(1f, fadeOutDelay);
        }

        private void FadeIn()
        {
            Fade(0f, fadeInDelay);
        }

        private Coroutine Fade(float target, float time)
        {
            if (_currentFade != null)
            {
                StopCoroutine(_currentFade);
            }

            _currentFade = StartCoroutine(FadeRoutine(target, time));
            return _currentFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha,
                    target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}
