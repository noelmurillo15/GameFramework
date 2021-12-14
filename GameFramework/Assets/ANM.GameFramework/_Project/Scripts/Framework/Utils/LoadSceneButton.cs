/*
 * LoadSceneButton -
 * Created by : Allan N. Murillo
 * Last Edited : 8/10/2020
 */

using UnityEngine;
using ANM.Framework.Extensions;

namespace ANM.Framework.Utils
{
    public class LoadSceneButton : MonoBehaviour
    {
        [SerializeField] private string levelName = string.Empty;


        public void ButtonPressed(bool multiScene = false)
        {
            if (!SceneExtension.IsThisSceneActive(SceneExtension.MenuUiSceneName)) return;
            StartCoroutine(multiScene
                ? SceneExtension.LoadMultiSceneSequence(levelName, true)
                : SceneExtension.LoadSingleSceneSequence(levelName, true));
        }
    }
}
