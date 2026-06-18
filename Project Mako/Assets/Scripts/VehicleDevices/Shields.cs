using System;
using Mako.HealthNamespace;
using UnityEngine;

namespace Mako.VehicleDevices
{

    public class Shields : MonoBehaviour
    {
        [SerializeField] private float maxShieldCapacity;
        [SerializeField] private float shieldCapacity;
        [SerializeField] private float timeToStartRegenerating;
        [SerializeField] private float regenerationSpeed;
        [SerializeField] private float timeSinceLastHit;
        private AudioSource _respectiveSoundEffect;
        private Health _playerHealth;
        public event Action OnShieldCapacityChanged;
        public void Initialize(Health playerHealth, AudioSource audioSource)
        {
            _playerHealth = playerHealth;
            _respectiveSoundEffect = audioSource;
        }
        private void Awake()
        {
            shieldCapacity = maxShieldCapacity;
            timeSinceLastHit = timeToStartRegenerating;
        }
        // Update is called once per frame
        void Update()
        {
            if (timeSinceLastHit < timeToStartRegenerating)
            {
                timeSinceLastHit += Time.deltaTime;
                if (timeSinceLastHit > timeToStartRegenerating)
                    timeSinceLastHit = timeToStartRegenerating;
            }
            else
            {
                shieldCapacity += Time.deltaTime * regenerationSpeed;
                OnShieldCapacityChanged?.Invoke();
                if (shieldCapacity < 0)
                    shieldCapacity = 0;
                if (shieldCapacity > maxShieldCapacity)
                    shieldCapacity = maxShieldCapacity;
            }
        }

        public float GetShieldCapacity()
        {
            return shieldCapacity;
        }

        public void OnHitReceived(float damageAmount)
        {
            float shieldCapacityBeforeHit = shieldCapacity;
            if (shieldCapacity > 0)
                shieldCapacity -= damageAmount;
            if (shieldCapacity < 0)
                shieldCapacity = 0;
            OnShieldCapacityChanged?.Invoke();
            timeSinceLastHit = 0;
            if (damageAmount > shieldCapacity)
                _playerHealth.Damage((int)damageAmount - (int)shieldCapacityBeforeHit);
            else
            {
                _respectiveSoundEffect.Play();
            }

        }
        public float GetPercent()
        {
            return (float)shieldCapacity / maxShieldCapacity;
        }
    }
}