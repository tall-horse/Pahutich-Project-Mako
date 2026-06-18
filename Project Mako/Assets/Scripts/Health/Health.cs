using System;
using System.Collections;
using UnityEngine;

namespace Mako.HealthNamespace
{

    public abstract class Health : MonoBehaviour
    {
        protected Collider _hitBox;
        protected AudioSource _destructionAudioSource;
        protected MeshRenderer _meshRenderer;
        protected HealthSystem _healthSystem;
        [SerializeField] protected int health;
        [SerializeField] protected string healthHolderName;
        [SerializeField] protected AudioSource respectiveAudioImpact;
        public event Action OnPlayDestructionSound;
        public Action OnDamageTaken;
        public void Initialize(AudioSource audioSource)
        {
            if (_healthSystem == null)
                SetupHealthObject();

            _destructionAudioSource = audioSource;
            _hitBox = GetComponent<Collider>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }
        protected virtual void Awake()
        {
            SetupHealthObject();
        }

        protected virtual void OnEnable()
        {
            if (_healthSystem == null)
                SetupHealthObject();
        }

        public void SetupHealthObject()
        {
            _healthSystem = new HealthSystem(health, healthHolderName);
        }

        public HealthSystem GetHealthSystem()
        {
            return _healthSystem;
        }
        public void PlayImpactSound()
        {
            respectiveAudioImpact.Play();
        }
    }

}
