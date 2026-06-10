using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mako.Input
{
    public sealed class InputManager : MonoBehaviour
    {
        public PlayerInputActions Actions { get; private set; }

        void Awake()
        {
            Actions = new PlayerInputActions();
            Actions.Enable();
        }

        void OnDestroy()
        {
            Actions?.Disable();
        }
    }
}
