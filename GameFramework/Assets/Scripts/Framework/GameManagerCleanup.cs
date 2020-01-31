/*
 * GameManagerCleanup- Destroys the GameManager once the credits scene has finished loading
 * Created by : Allan N. Murillo
 */

using UnityEngine;

namespace ANM.Framework
{
    public class GameManagerCleanup : MonoBehaviour
    {
        private void Start()
        {
            Invoke($"ApplicationQuit", 3f);
            if (GameManager.Instance == null) return;
            GameManager.Instance.sceneTransitionManager.ScreenMaskBrightness = 0f;
            GameManager.Instance.UnloadAllLoadedScenes();
        }

        private void ApplicationQuit()
        {
            if (GameManager.Instance != null)
            {
                Destroy(obj: GameManager.Instance.gameObject);
            }
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
