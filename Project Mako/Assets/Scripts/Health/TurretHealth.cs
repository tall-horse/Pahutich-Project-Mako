using System.Collections;
using Mako.HealthNamespace;
using Mako.Shooting;
using UnityEngine;

namespace Mako
{
    public sealed class TurretHealth : Health, ISelfDesctructable
    {
        private Turret _turret;
        private IEnumerable _turretVisuals;
        protected override void Awake()
        {
            base.Awake();
            _turret = GetComponent<Turret>();
            _destructionAudioSource = GetComponentInChildren<AudioSource>();
            _hitBox = GetComponent<Collider>();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            OnGotDamaged += RespondToFire;
            OnDead += StartCorSelfDestroy;
            _turretVisuals = _turret.GetTurretVisuals();
        }

        private void OnDisable()
        {
            OnGotDamaged -= RespondToFire;
            OnDead -= StartCorSelfDestroy;
        }

        private void RespondToFire()
        {
            _turret.playerInRange = true;
            _turret.ExtendRange();
        }
        private void StartCorSelfDestroy()
        {
            StartCoroutine(SelfDestroy());
        }

        public IEnumerator SelfDestroy()
        {
            _destructionAudioSource.Play();
            _turret.dead = true;
            _hitBox.enabled = false;
            foreach (MeshRenderer t in _turretVisuals)
            {
                t.enabled = false;
            }
            yield return null;
        }
    }
}
