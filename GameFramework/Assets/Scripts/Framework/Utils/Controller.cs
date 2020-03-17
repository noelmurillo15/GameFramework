/*
 * Controller - Holds a reference to PlayerController, used to setup various inputs from anywhere
 * Created by : Allan N. Murillo
 * Last Edited : 3/17/2020
 */

using UnityEngine;

namespace ANM.Framework.Utils
{
    [CreateAssetMenu(menuName = "Single Instances/Controller")]
    public class Controller : ScriptableObject
    {
        public PlayerControls controller;

        private void OnEnable()
        {
            if(controller == null) controller = new PlayerControls();
            controller.Enable();
            controller.Player.Enable();
        }

        private void OnDisable()
        {
            if (controller == null) return;
            controller.Disable();
            controller = null;
        }
    }
}