/*
 * GameManagerCleanup- Destroys the GameManager after a certain delay (In Credits Scene)
 * Created by : Allan N. Murillo
 * Last Edited : 2/17/2020
 */

using UnityEngine;
using ANM.Framework.Managers;

namespace ANM.Framework.Utils
{
    public class GameManagerCleanup : MonoBehaviour
    {
        [SerializeField] private float quitDelay = 3f;

        private void Start()
        {
            Invoke($"ApplicationQuit", quitDelay);
        }

        private void ApplicationQuit()
        {
            if (GameManager.Instance != null) Destroy(obj: GameManager.Instance.gameObject);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
