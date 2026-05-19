using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mako.Movement;

namespace Mako.State
{

    public class NextLevel : MonoBehaviour
    {
        private Collider hitBox;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI gameOverText;
        [SerializeField] private Button resumeButton;

        private void Awake()
        {
            hitBox = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerController>())
            {
                FinishLevel();
            }
        }
        private void FinishLevel()
        {
            Time.timeScale = 0;
            resumeButton.gameObject.SetActive(false);
            gameOverText.text = "Congratulations! You've completed the demo level!";
            gameOverPanel.SetActive(true);
        }
    }
}