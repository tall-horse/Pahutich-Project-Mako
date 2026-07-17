using UnityEngine;
using UnityEngine.InputSystem;
namespace Mako.UI
{
    public class Crosshair : MonoBehaviour
    {
        [SerializeField] private bool _isCursorVisible;

        void Start()
        {
            Cursor.visible = _isCursorVisible;
        }

        void Update()
        {
            transform.position = Mouse.current.position.ReadValue();
        }
    }
}