using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static readonly string _gamepadScheme = "Gamepad";
    private static readonly string _keyboardAndMouseScheme = "Keyboard&Mouse";
    public static InputManager Instance { get; private set; }

    public bool IsGamepadSchemeActive { get; private set; }


    void Awake()
    {
        CheckInstance();
    }

    private void CheckInstance()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void OnControlChanged(PlayerInput playerInput)
    {
        if (playerInput.user.controlScheme.Value.name.Equals(_gamepadScheme))
        {
            IsGamepadSchemeActive = true;
        }
        else if (playerInput.user.controlScheme.Value.name.Equals(_keyboardAndMouseScheme))
        {
            IsGamepadSchemeActive = false;
        }
    }
}
