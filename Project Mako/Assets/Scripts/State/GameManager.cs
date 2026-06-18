using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Mako.Input;

namespace Mako.State
{
    public class GameManager : MonoBehaviour
    {
        private bool gameOver = false;
        public bool GameIsPaused { get; private set; } = false;
        private GameObject _player;
        private HealthNamespace.Health _playerHealth;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI pauseOrLossText;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _settingsButton;
        private InputManager _inputManager;
        private const string PauseText = "Pause";
        private const string LossText = "You lost";
        private void Awake()
        {
            gameOverPanel.SetActive(false);
        }
        // Start is called before the first frame update
        public void Initialize(InputManager inputManager, GameObject player, HealthNamespace.Health playerHealth)
        {
            _inputManager = inputManager;
            _player = player;
            _playerHealth = playerHealth;
        }
        void Start()
        {
            _playerHealth.GetHealthSystem().OnDead += GameOver;
        }
        private void OnDisable()
        {
            _playerHealth.GetHealthSystem().OnDead -= GameOver;
        }

        // Update is called once per frame
        void Update()
        {
            if (gameOver) return;
            bool pauseTime = _inputManager.Actions.Player.Pause.triggered;
            if (pauseTime)
            {
                Pause();
            }
        }

        public void Pause()
        {
            if (!GameIsPaused)
            {
                GameIsPaused = true;
                Time.timeScale = 0;
                gameOverPanel.SetActive(true);
                pauseOrLossText.text = PauseText;
                Cursor.visible = true;
            }
            else
            {
                GameIsPaused = false;
                Time.timeScale = 1;
                gameOverPanel.SetActive(false);
                Cursor.visible = false;
            }
        }
        private void GameOver()
        {
            Debug.Log("game over");
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
