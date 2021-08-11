/*
 * ResourcesManager - Contains important Game Resources such as the Input Controller
 * Created by : Allan N. Murillo
 * Last Edited : 7/4/2021
 */

using UnityEngine;
using ANM.Framework.Input;

namespace ANM.Framework.Scriptables
{
    [CreateAssetMenu(menuName = "Single Instance/Resources Manager")]
    public class ResourcesManager : ScriptableObject
    {
        private InputController _inputController;


        public void Initialize()
        {
            if (_inputController == null) _inputController = Resources.Load("PlayerControls") as InputController;
            if (_inputController == null)
            {
                Debug.LogWarning("[ResourcesManager]: Player Controls was not loaded properly");
            }
        }

        public InputController GetInput() => _inputController;

        public static Object FindResource(string resourceFilePath)
        {
            var resource = Resources.Load(resourceFilePath);
            return resource;
        }
    }
}
