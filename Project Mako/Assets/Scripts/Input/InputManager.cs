using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mako.Input
{
    public sealed class InputManager : MonoBehaviour
    {
        private static InputManager _instance;
        public static InputManager Instance => _instance ?? throw new System.Exception("InputManager not initialised");

        public PlayerInputActions Actions { get; private set; }

        void Awake()
        {
            // Guard against duplicate managers
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;

            Actions = new PlayerInputActions();
            Actions.Enable();
        }

        void OnDestroy()
        {
            Actions?.Disable();
            if (_instance == this) _instance = null;
        }
    }
}
