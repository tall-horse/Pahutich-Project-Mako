using System;

namespace Mako.HealthNamespace
{

    public class HealthSystem
    {
        private int healthMax;
        private int health;
        private string healthHolder;
        public event Action<HealthSystem> OnHealthChanged;
        public event Action OnDead;
        public event Action OnGotDamaged;
        public HealthSystem(int healthMax, string healthHolder)
        {
            this.healthMax = healthMax;
            health = healthMax;
            this.healthHolder = healthHolder;
        }

        public int GetHealth()
        {
            return health;
        }

        public float GetPercent()
        {
            return (float)health / healthMax;
        }

        public string GetHolder()
        {
            return healthHolder;
        }

        public void Damage(int damageAmount)
        {
            if (health == healthMax)
                OnGotDamaged?.Invoke();
            health -= damageAmount;
            OnHealthChanged?.Invoke(this);
            if (health <= 0)
            {
                health = 0;
                OnDead?.Invoke();
            }
        }
        public void Heal(int healAmount)
        {
            health += healAmount;
            if (health > healthMax) health = healthMax;
            OnHealthChanged?.Invoke(this);
        }
    }
}