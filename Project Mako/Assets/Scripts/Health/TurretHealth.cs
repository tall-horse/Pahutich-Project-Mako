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
        }
        protected override void OnEnable()
        {
            base.SubscribeEvents();
            healthSystem.OnRespondToFire += RespondToFire;
            _turretVisuals = _turret.GetTurretVisuals();
        }

        protected override void OnDisable()
        {
            base.UnsubscribeEvents();
            healthSystem.OnRespondToFire -= RespondToFire;
        }

        private void RespondToFire()
        {
            _turret.playerInRange = true;
            _turret.ExtendRange();
        }

        protected override IEnumerator SelfDestroy()
        {
            audioSource.Play();
            _turret.dead = true;
            hitBox.enabled = false;
            foreach (MeshRenderer t in _turretVisuals)
            {
                t.enabled = false;
            }
            yield return null;
        }
    }
}
