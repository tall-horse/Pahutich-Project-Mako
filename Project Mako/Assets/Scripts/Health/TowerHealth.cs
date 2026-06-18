using System.Collections;
using Mako.HealthNamespace;
using UnityEngine;

namespace Mako
{
    public class TowerHealth : Health, ISelfDesctructable
    {
        [SerializeField] private float _timeToRestoreSignal = 1f;
        protected override void Awake()
        {
            base.Awake();
            //meshRenderer = GetComponentInChildren<MeshRenderer>();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            _healthSystem.OnDead += StartCorSelfDestroy;
        }
        private void StartCorSelfDestroy()
        {
            StartCoroutine(SelfDestroy());
        }
        public IEnumerator SelfDestroy()
        {
            _destructionAudioSource.Play();
            _hitBox.enabled = false;
            _meshRenderer.enabled = false;
            yield return new WaitForSeconds(_timeToRestoreSignal);
            Destroy(gameObject);
        }
    }
}
