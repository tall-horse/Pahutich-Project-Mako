using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace Mako.HealthNamespace
{

    public abstract class Health : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] protected int _healthMax;
        protected int _currentHealth;
        [SerializeField] protected string _healthHolder;
        public event Action<Health> OnHealthChanged;
        public event Action OnDead;
        public event Action OnGotDamaged;
        [Header("Components")]
        protected Collider _hitBox;
        protected AudioSource _destructionAudioSource;
        protected MeshRenderer _meshRenderer;
        [SerializeField] protected AudioSource respectiveAudioImpact;
        [SerializeField] protected GameObject _radarPoint;
        public event Action OnPlayDestructionSound;
        //public Action OnDamageTaken;
        public void Initialize(AudioSource audioSource)
        {
            _destructionAudioSource = audioSource;
            _hitBox = GetComponent<Collider>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }
        public void Initialize()
        {
            _destructionAudioSource = GetComponent<AudioSource>();
            _hitBox = GetComponent<Collider>();
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }
        protected virtual void Awake()
        {
            _currentHealth = _healthMax;
        }

        protected virtual void OnEnable()
        {

        }
        public void PlayImpactSound()
        {
            respectiveAudioImpact.Play();
        }
        public void Damage(int damageAmount)
        {
            // if (_currentHealth == _healthMax)
            _currentHealth -= damageAmount;
            OnGotDamaged?.Invoke();
            OnHealthChanged?.Invoke(this);
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                OnDead?.Invoke();
            }
        }
        public void Heal(int healAmount)
        {
            _currentHealth += healAmount;
            if (_currentHealth > _healthMax) _currentHealth = _healthMax;
            OnHealthChanged?.Invoke(this);
        }
        public float GetPercent()
        {
            return (float)_currentHealth / _healthMax;
        }
        public string GetHolder() => _healthHolder;
    }

}
