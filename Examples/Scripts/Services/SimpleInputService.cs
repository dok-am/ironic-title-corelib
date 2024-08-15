using IT.CoreLib.Interfaces;
using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace IT.CoreLib.Examples.Services
{
    public class SimpleInputService : IService, IUpdatable
    {
        public enum InputState
        {
            Gameplay = 0,
            Dialogue
        }

        public event Action OnUsePressed;
        public event Action OnMenuPressed;
        public event Action OnSkipPressed;

        public Vector2 MoveValue { get; private set; }


        private InputAction _moveAction;
        private InputAction _useAction;
        private InputAction _menuAction;

        private InputState _inputState;
        private bool _isPaused;


        public void Initialize()
        {
            _moveAction = InputSystem.actions.FindAction("Move");
            _useAction = InputSystem.actions.FindAction("Use");
            _menuAction = InputSystem.actions.FindAction("Menu");

            _useAction.started += UsePressed;
            _menuAction.started += MenuPressed;
            _inputState = InputState.Gameplay;
        }

        public void OnPaused(bool paused)
        {
            _isPaused = paused;
            if (paused)
                MoveValue = Vector2.zero;
        }

        public void Destroy()
        {
            _useAction.started -= UsePressed;
            _menuAction.started -= MenuPressed;
        }

        public void Update(float dt)
        {
            if (_inputState == InputState.Gameplay && _moveAction.IsPressed())
                MoveValue = _moveAction.ReadValue<Vector2>();
            else
                MoveValue = Vector2.zero;
        }

        public void SetInputState(InputState state)
        {
            _inputState = state;
        }


        private void UsePressed(InputAction.CallbackContext context)
        {
            if (_isPaused)
                return;

            if (_inputState == InputState.Gameplay)
                OnUsePressed?.Invoke();
            else if (_inputState == InputState.Dialogue)
                OnSkipPressed?.Invoke();
        }

        private void MenuPressed(InputAction.CallbackContext context)
        {
            OnMenuPressed?.Invoke();
        }
    }
}
