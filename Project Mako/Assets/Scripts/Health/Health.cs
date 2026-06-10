using System.Collections;
using UnityEngine;

namespace Mako.Health
{

    public abstract class BasicHealth : MonoBehaviour
    {
        protected Collider _hitBox;
        protected AudioSource _audioSource;
        protected MeshRenderer _meshRenderer;
        protected HealthSystem _healthSystem;
        [SerializeField] protected int health;
        [SerializeField] protected string healthHolderName;
        [SerializeField] protected AudioSource respectiveAudioImpact;
        public void Initialize(AudioSource audioSource)
        {
            if (_healthSystem == null)
                _healthSystem = new HealthSystem(health, healthHolderName);

            _audioSource = audioSource;
            _hitBox = GetComponent<Collider>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }
        protected virtual void Awake()
        {
            SetupHealthObject();
        }

        protected virtual void OnEnable()
        {
            SubscribeEvents();
        }

        protected virtual void SubscribeEvents()
        {
            if (_healthSystem == null)
                SetupHealthObject();
            _healthSystem.OnHealthChanged += StartDestructionProcess;
            _healthSystem.OnDead += StartCorSelfDestroy;
        }

        protected virtual void OnDisable()
        {
            UnsubscribeEvents();
        }

        protected virtual void UnsubscribeEvents()
        {
            _healthSystem.OnHealthChanged -= StartDestructionProcess;
            _healthSystem.OnDead -= StartCorSelfDestroy;
        }

        public void SetupHealthObject()
        {
            _healthSystem = new HealthSystem(health, healthHolderName);
        }

        public HealthSystem GetHealthSystem()
        {
            return _healthSystem;
        }
        protected virtual void StartDestructionProcess(HealthSystem hs)
        {
            if (hs.GetHealth() <= 0)
                StartCoroutine(SelfDestroy());
        }
        public void PlayImpactSound()
        {
            respectiveAudioImpact.Play();
        }
        protected void StartCorSelfDestroy()
        {
            StartCoroutine(SelfDestroy());
        }
        protected abstract IEnumerator SelfDestroy();
    }

}
