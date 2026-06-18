using System.Collections;
using System.Collections.Generic;
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
            _healthSystem.OnGotDamaged += RespondToFire;
            _turretVisuals = _turret.GetTurretVisuals();
        }

        private void OnDisable()
        {
            _healthSystem.OnGotDamaged -= RespondToFire;
        }

        private void RespondToFire()
        {
            _turret.playerInRange = true;
            _turret.ExtendRange();
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
