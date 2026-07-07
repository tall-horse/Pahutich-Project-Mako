using System.Collections;
using System.Collections.Generic;
using Mako.State;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mako.UI
{

    public class Crosshair : MonoBehaviour
    {
        private Image _cursorImage;
        [SerializeField] private Sprite _menuSprite;
        [SerializeField] private Sprite _combatSprite;

        private void Awake()
        {
            _cursorImage = GetComponent<Image>();
        }

        void Start()
        {
            Cursor.visible = false;
        }

        void Update()
        {
            if (GameManager.GameIsPaused == false && SceneManager.GetActiveScene().buildIndex >= 1)
            {
                _cursorImage.sprite = _combatSprite;
            }

            if (GameManager.GameIsPaused == true || SceneManager.GetActiveScene().buildIndex == 0)
            {
                _cursorImage.sprite = _menuSprite;
            }
            transform.position = Mouse.current.position.ReadValue();
        }
    }
}
