using IT.CoreLib.Interfaces;
using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace IT.CoreLib.Examples.Services
{
    public class SimpleInputService : IService, IUpdatable
    {
        public event Action OnUsePressed;
        public event Action OnMenuPressed;


        public Vector2 MoveValue { get; private set; }


        private InputAction _moveAction;
        private InputAction _useAction;
        private InputAction _menuAction;

        private bool _isPaused;


        public void Initialize()
        {
            _moveAction = InputSystem.actions.FindAction("Move");
            _useAction = InputSystem.actions.FindAction("Use");
            _menuAction = InputSystem.actions.FindAction("Menu");

            _useAction.started += UsePressed;
            _menuAction.started += MenuPressed;
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
            MoveValue = _moveAction.IsPressed() ? _moveAction.ReadValue<Vector2>() : Vector2.zero;
        }


        private void UsePressed(InputAction.CallbackContext context)
        {
            if (_isPaused)
                return;

            OnUsePressed?.Invoke();
        }

        private void MenuPressed(InputAction.CallbackContext context)
        {
            OnMenuPressed?.Invoke();
        }
    }
}
