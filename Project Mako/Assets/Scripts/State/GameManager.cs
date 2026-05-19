using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Mako.State
{
    public class GameManager : MonoBehaviour
    {
        private bool gameOver = false;
        public bool GameIsPaused { get; private set; } = false;
        private GameObject player;
        [SerializeField] private Health.BasicHealth playerHealth;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI pauseOrLossText;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _settingsButton;

        PlayerInputActions playerInputActions;
        private const string PauseText = "Pause";
        private const string LossText = "You lost";
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerHealth = player.GetComponent<Health.BasicHealth>();
            gameOverPanel.SetActive(false);
            playerInputActions = new PlayerInputActions();
            playerInputActions.Player.Enable();
        }
        // Start is called before the first frame update
        void OnEnable()
        {
            if (playerHealth == null)
            {
                Debug.LogWarning("Player health not found");
                return;
            }
            if (playerHealth.GetHealthSystem() == null)
            {
                playerHealth.SetupHealthObject();
            }
            playerHealth.GetHealthSystem().OnDead += GameOver;
        }
        private void OnDisable()
        {
            playerHealth.GetHealthSystem().OnDead -= GameOver;
        }

        // Update is called once per frame
        void Update()
        {
            if (gameOver) return;
            bool pauseTime = playerInputActions.Player.Pause.triggered;
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
