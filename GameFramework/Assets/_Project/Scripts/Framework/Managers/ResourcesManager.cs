/*
 * ResourcesManager - Handles the initialization of Game Resources such as the Input Controller
 * Created by : Allan N. Murillo
 * Last Edited : 8/6/2020
 */

using ANM.Input;
using UnityEngine;

namespace ANM.Scriptables
{
    [CreateAssetMenu(menuName = "Single Instance/Resources Manager")]
    public class ResourcesManager : ScriptableObject
    {
        private InputController _inputController;


        public void Initialize()
        {
            if (_inputController == null) _inputController = Resources.Load("InputController") as InputController;
            if (_inputController == null)
            {
                Debug.LogWarning("[ResourcesManager]: Player Controls was not loaded properly");
            }
        }

        public InputController GetInputController() => _inputController;
    }
}
