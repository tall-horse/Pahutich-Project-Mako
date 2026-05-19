using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mako.UI
{
    public class Crosshair : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Cursor.visible = false;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = Mouse.current.position.ReadValue();
        }
    }
}
