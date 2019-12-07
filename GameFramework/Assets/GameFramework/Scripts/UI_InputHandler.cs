using UnityEngine;
using GameFramework.Managers;
using UnityEngine.InputSystem;


[CreateAssetMenu]
public class UI_InputHandler : ScriptableObject
{
    Gamepad gamepad;
    Mouse mouse;
    Keyboard keyboard;
    GameSettingsManager gameSettingsUI;
    InputAction pauseAction = null;


    #region Unity Events
    void Awake()
    {
        Debug.Log("UI_Input::Awake()");
        if (pauseAction != null)
        {
            pauseAction = new InputAction("pause", binding: "<Gamepad>/start");
            pauseAction.AddBinding("<Keyboard>/escape");
        }
    }
    void OnEnable()
    {
        Debug.Log("UI_Input::OnEnable()");
        if (Gamepad.current != null)
        {
            EnableGamepad();
        }
        else
        {
            Debug.Log("No gamepad input detected!");
            if (Keyboard.current != null)
            {
                keyboard = Keyboard.current;
            }
            else
            {
                Debug.LogError("No keyboard input detected!");
            }

            if (Mouse.current != null)
            {
                mouse = Mouse.current;
            }
            else
            {
                Debug.LogError("No mouse input detected!");
            }
        }
    }
    void OnDisable()
    {
        Debug.Log("UI_Input::OnDisable()");
        if (gamepad != null)
        {
            DisableGamepad();
        }
        keyboard = null;
        mouse = null;
    }

    void OnDestroy()
    {
        Debug.Log("UI_Input::OnDestroy()");
        pauseAction.Dispose();
        pauseAction = null;
    }
    #endregion

    #region Helper Functions
    void EnableGamepad()
    {
        Debug.Log("UI_Input::EnableGamepad()");
        keyboard = null;
        mouse = null;
        gamepad = Gamepad.current;

        if (pauseAction == null)
        {
            Debug.Log("-    Pause Action was NULL");
            pauseAction = new InputAction("pause", binding: "<Gamepad>/start");
            pauseAction.AddBinding("<Keyboard>/escape");
        }

        pauseAction.performed += OnPauseAction;
        pauseAction.Enable();
    }

    void DisableGamepad()
    {
        Debug.Log("UI_Input::DisableGamepad()");
        if (pauseAction != null)
        {
            pauseAction.performed -= OnPauseAction;
            pauseAction.Disable();
        }
        gamepad = null;
    }
    #endregion

    #region Public Functions
    public void AssignGameSettings(GameSettingsManager gsm)
    {
        Debug.Log("UI_Input::AssignGameSettings()");
        gameSettingsUI = gsm;
        if (gamepad == null)
        {
            Debug.Log("-    gamepad is NULL");
            if (Gamepad.current != null)
            {
                EnableGamepad();
            }
        }
    }
    #endregion

    #region Input Callbacks
    public void OnPauseAction(InputAction.CallbackContext context)
    {
        Debug.Log("UI_Input::OnPauseAction()");
        if (gameSettingsUI != null)
        {
            if (!gameSettingsUI.paused)
            {
                gameSettingsUI.Pause();
            }
            else
            {
                gameSettingsUI.Resume();
            }
        }
        else
        {
            Debug.Log("-    gameSettingsUI is NULL");
        }
    }

    public void DeviceLost()
    {
        Debug.Log("UI_Input::DeviceLost()");
        DisableGamepad();
    }

    public void DeviceConnected()
    {
        Debug.Log("UI_Input::DeviceConnected()");
        EnableGamepad();
    }
    #endregion
}
