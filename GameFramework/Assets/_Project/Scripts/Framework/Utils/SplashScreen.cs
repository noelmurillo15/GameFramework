/*
 * SplashScreen -
 * Created by : Allan N. Murillo
 * Last Edited : 3/1/2020
 */

using UnityEngine;
using ANM.Framework.Extensions;

namespace ANM.Framework.Utils
{
    public class SplashScreen : MonoBehaviour
    {
        private void Start()
        {
            Invoke(nameof(TransitionToMainMenu), 3f);
        }

        private void TransitionToMainMenu()
        {
            if (!SceneExtension.IsThisSceneActive(SceneExtension.MenuUiSceneName))
            {
                StartCoroutine(SceneExtension.ForceMenuSceneSequence(true));
            }
        }
    }
}
