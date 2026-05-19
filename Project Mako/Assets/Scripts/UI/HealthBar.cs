using UnityEngine;
using UnityEngine.UI;
using Mako.Health;

namespace Mako.UI
{

    public class HealthBar : MonoBehaviour
    {
        private HealthSystem healthSystem;
        [SerializeField] private Image healthBarImage;
        [SerializeField] private GameObject? player;
        private void Start()
        {
            if (player != null)
            {
                healthSystem = player.GetComponent<Health.BasicHealth>().GetHealthSystem();
                healthSystem.OnHealthChanged += UpdateHealthStatus;
            }
        }

        private void UpdateHealthStatus(HealthSystem hs)
        {
            healthBarImage.fillAmount = hs.GetPercent();
        }
        public void ReconfigureHealthHolder(Health.BasicHealth holder)
        {
            healthSystem = holder.GetHealthSystem();
            UpdateHealthStatus(healthSystem);
            healthSystem.OnHealthChanged -= UpdateHealthStatus;
            healthSystem.OnHealthChanged += UpdateHealthStatus;
        }
    }
}
