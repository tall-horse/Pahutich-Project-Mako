using UnityEngine;
using UnityEngine.UI;
using Mako.HealthNamespace;

namespace Mako.UI
{

    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image healthBarImage;
        [SerializeField] private GameObject? player;
        private Health _health;
        private void Start()
        {
            if (player != null)
            {
                _health = player.GetComponent<Health>();
                _health.OnHealthChanged += UpdateHealthStatus;
            }
        }

        private void UpdateHealthStatus(Health health)
        {
            healthBarImage.fillAmount = health.GetPercent();
        }
        public void ReconfigureHealthHolder(Health health)
        {
            UpdateHealthStatus(health);
            health.OnHealthChanged -= UpdateHealthStatus;
            health.OnHealthChanged += UpdateHealthStatus;
        }
    }
}
