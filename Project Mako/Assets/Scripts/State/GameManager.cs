using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Mako.Input;
using UnityEngine.InputSystem;

namespace Mako.State
{
    public class GameManager : MonoBehaviour
    {
        private bool gameOver = false;
        public static bool GameIsPaused { get; private set; } = false;
        private GameObject _player;
        private HealthNamespace.Health _playerHealth;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI pauseOrLossText;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _settingsButton;
        private const string PauseText = "Pause";
        private const string LossText = "You lost";
        private void Awake()
        {
            gameOverPanel.SetActive(false);
        }
        public void Initialize(GameObject player, HealthNamespace.Health playerHealth)
        {
            _player = player;
            _playerHealth = playerHealth;
        }
        void Start()
        {
            InputManager.Instance.actions.Player.Pause.canceled += CallPauseFormInput;
            _playerHealth.OnDead += GameOver;
        }
        private void OnDisable()
        {
            InputManager.Instance.actions.Player.Pause.canceled -= CallPauseFormInput;
            _playerHealth.OnDead -= GameOver;
        }

        public void CallPauseFormInput(InputAction.CallbackContext obj)
        {
            Pause();
        }
        public void Pause()
        {
            if (gameOver) return;

            if (!GameIsPaused)
            {
                GameIsPaused = true;
                Time.timeScale = 0;
                gameOverPanel.SetActive(true);
                pauseOrLossText.text = PauseText;
                //Cursor.visible = true;
            }
            else
            {
                GameIsPaused = false;
                Time.timeScale = 1;
                gameOverPanel.SetActive(false);
                //Cursor.visible = false;
            }
        }
        private void GameOver()
        {
            Time.timeScale = 0;
            pauseOrLossText.text = LossText;
            pauseOrLossText.color = Color.red;
            _resumeButton.gameObject.SetActive(false);
            _settingsButton.gameObject.SetActive(false);
            gameOverPanel.SetActive(true);
            gameOver = true;
        }
    }
}
