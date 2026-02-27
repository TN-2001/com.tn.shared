using UnityEngine;
using System;
using UnityEngine.InputSystem;
using Library.Generic;

namespace Library.Input
{
    /// <summary>
    /// 入力モードを管理するシングルトンクラス
    /// </summary>
    public class InputManager : Singleton<InputManager>
    {
        public enum InputMode
        {
            MouseKeyboard,
            Gamepad
        }

        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private InputMode currentMode = InputMode.MouseKeyboard;
        [SerializeField] private bool isAlwaysSelectButtonOnMouseKeyboard = false;
        
        public PlayerInput PlayerInput => playerInput;
        public InputMode CurrentMode => currentMode;
        public bool IsAlwaysSelectButton => currentMode == InputMode.Gamepad || (currentMode == InputMode.MouseKeyboard && isAlwaysSelectButtonOnMouseKeyboard);

        public static event Action<InputMode> OnInputModeChanged;


        protected override void Awake()
        {
            base.Awake();

            if (Instance != this) return;

            playerInput.enabled = true;
            playerInput.onControlsChanged += OnControlsChanged;
        }

        private void OnDestroy()
        {
            if (playerInput != null)
            {
                playerInput.onControlsChanged -= OnControlsChanged;
            }
        }


        private void OnControlsChanged(PlayerInput input)
        {
            if (input == null || this == null) return;

            string scheme = input.currentControlScheme;
            if (string.IsNullOrEmpty(scheme)) return;

            InputMode newMode = currentMode;

            if (scheme == "Gamepad")
            {
                newMode = InputMode.Gamepad;
            }
            else if (scheme == "Keyboard&Mouse")
            {
                newMode = InputMode.MouseKeyboard;
            }

            if (newMode != currentMode)
            {
                currentMode = newMode;
                OnInputModeChanged?.Invoke(currentMode);
            }
        }
    }
}