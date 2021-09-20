/*
 * InputController - Holds a reference to PlayerControls & contains all Input Events
 * Created by : Allan N. Murillo
 * Last Edited : 7/4/2021
 */

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ANM.Framework.Input
{
    [CreateAssetMenu(menuName = "Single Instance/InputController")]
    public class InputController : ScriptableObject, PlayerControls.IGameplayActions, PlayerControls.IMenuActions
    {
        public bool log;

        public event UnityAction<bool> OnLeftClickEvent = delegate { };
        public event UnityAction OnPauseToggleEvent = delegate { };
        public event UnityAction OnCancelEvent = delegate { };

        private PlayerControls _playerControls;
        private bool _isPressed;


        #region Unity Funcs

        private void OnEnable()
        {
            Log("OnEnable");
            if(_playerControls == null)
                _playerControls = new PlayerControls();
            _playerControls.Enable();
            _playerControls.Menu.SetCallbacks(this);
            _playerControls.Gameplay.SetCallbacks(this);
            _playerControls.Gameplay.Enable();
            _playerControls.Menu.Enable();
            _isPressed = false;
        }

        private void OnDisable()
        {
            Log("OnDisable");
            if (_playerControls == null) return;
            _playerControls.Gameplay.Disable();
            _playerControls.Menu.Disable();
            _playerControls.Disable();
            _playerControls = null;
            _isPressed = false;
        }

        #endregion

        #region Public Funcs

        public static Vector3 GetMousePos() => Mouse.current.position.ReadValue();

        public static Ray GetMouseRay() => Camera.main.ScreenPointToRay(GetMousePos());

        #endregion

        #region Input Event Callbacks

        public void OnNavigate(InputAction.CallbackContext context)
        {
            Log("OnNavigate");
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            Log("OnSubmit");
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed) return;
            OnCancelEvent?.Invoke();
            Log("OnCancel");
        }

        //  Called whenever the mouse pointer moves
        public void OnPoint(InputAction.CallbackContext context)
        {
            Log("OnPoint");
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed) return;
            _isPressed = !_isPressed;
            OnLeftClickEvent?.Invoke(_isPressed);
            Log("OnClick");
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            Log("OnScrollWheel");
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            Log("OnMiddleClick");
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            Log("OnRightClick");
        }

        public void OnPauseToggle(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed) return;
            OnPauseToggleEvent?.Invoke();
            Log("OnPauseToggle");
        }

        #endregion

        #region Private Funcs

        private void Log(string msg)
        {
            if (!log) return;
            Debug.Log("[InputController] : " + msg);
        }

        #endregion
    }
}
