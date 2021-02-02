/*
 * InputController - Holds a reference to PlayerControls, used to setup various inputs from anywhere
 * Created by : Allan N. Murillo
 * Last Edited : 4/27/2020
 */

using UnityEngine;

namespace ANM.Input
{
    [CreateAssetMenu(menuName = "Single Instance/InputController")]
    public class InputController : ScriptableObject
    {
        public PlayerControls playerControls;


        private void OnEnable()
        {
            if (playerControls == null) playerControls = new PlayerControls();
            playerControls.Enable();
            playerControls.Gameplay.Enable();
        }

        private void OnDisable()
        {
            if (playerControls == null) return;
            playerControls.Disable();
            playerControls = null;
        }
    }
}
