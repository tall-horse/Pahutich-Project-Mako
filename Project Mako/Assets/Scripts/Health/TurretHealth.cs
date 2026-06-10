using System.Collections;
using System.Collections.Generic;
using Mako.Health;
using Mako.Shooting;
using UnityEngine;

namespace Mako
{
    public class TurretHealth : BasicHealth
    {
        private Turret _turret;
        private IEnumerable _turretVisuals;
        protected override void Awake()
        {
            base.Awake();
            _turret = GetComponent<Turret>();
            _audioSource = GetComponentInChildren<AudioSource>();
            _hitBox = GetComponent<Collider>();
        }
        protected override void OnEnable()
        {
            base.SubscribeEvents();
            _healthSystem.OnRespondToFire += RespondToFire;
            _turretVisuals = _turret.GetTurretVisuals();
        }

        protected override void OnDisable()
        {
            base.UnsubscribeEvents();
            _healthSystem.OnRespondToFire -= RespondToFire;
        }

        private void RespondToFire()
        {
            _turret.playerInRange = true;
            _turret.ExtendRange();
        }

        protected override IEnumerator SelfDestroy()
        {
            _audioSource.Play();
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
