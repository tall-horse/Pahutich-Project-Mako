using UnityEngine;

namespace Mako.Input
{
    public sealed class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }
        public PlayerInputActions actions;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            if (Instance != null) return;

            GameObject go = new GameObject("InputManager");
            Instance = go.AddComponent<InputManager>();
            DontDestroyOnLoad(go);
        }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            actions = new PlayerInputActions();
            actions.Enable();
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                actions.Disable();
                actions = null;
                Instance = null;
            }
        }

        public void ChangeInputMap(InputType inputType)
        {
            switch (inputType)
            {
                case InputType.Player:
                    actions.Player.Enable();
                    //actions.UI.Disable();
                    break;
                case InputType.UI:
                    //actions.UI.Enable();
                    actions.Player.Disable();
                    break;
            }
        }

        public enum InputType
        {
            Player, UI
        }
    }
}
