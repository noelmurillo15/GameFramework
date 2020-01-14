/*
    *   GameManagerCleanup- Destroys the GameManager once the credits scene has finished loading
    *   Created by : Allan N. Murillo
 */
using UnityEngine;

namespace GameFramework.Core
{
    public class GameManagerCleanup : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.sceneTransitionManager.ScreenMaskBrightness = 0f;
            Invoke($"ApplicationQuit", 3f);
        }

        private void ApplicationQuit()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.Quit();
            Destroy(obj: GameManager.Instance.gameObject);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
            // GameManager.Instance.Quit() -> calls a js library function to quit from browser
#else
            Application.Quit();
#endif
        }
    }
}
