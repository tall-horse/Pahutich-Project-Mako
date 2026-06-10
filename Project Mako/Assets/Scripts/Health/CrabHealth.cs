using System.Collections;
using System.Collections.Generic;
using Mako.AI;
using Mako.Health;
using UnityEngine;

namespace Mako
{
    public class CrabHealth : BasicHealth
    {
        private CrabMonsterAI _crabMonsterAI;
        protected override void Awake()
        {
            base.Awake();
            _crabMonsterAI = GetComponent<CrabMonsterAI>();
            _audioSource = GetComponentInChildren<AudioSource>();
            _hitBox = GetComponent<Collider>();
        }
        protected override void OnEnable()
        {
            base.SubscribeEvents();
            _healthSystem.OnRespondToFire += RespondToFire;
        }

        protected override void OnDisable()
        {
            base.UnsubscribeEvents();
            _healthSystem.OnRespondToFire -= RespondToFire;
        }

        private void RespondToFire()
        {
            _crabMonsterAI.GoOnSound();
        }
        protected override void StartDestructionProcess(HealthSystem hs)
        {
            if (hs.GetHealth() <= 0)
            {
                _crabMonsterAI.SetDeadState();
                StartCoroutine(SelfDestroy());
            }
        }

        protected override IEnumerator SelfDestroy()
        {
            _audioSource.Play();
            //animator.enabled = false;
            yield return new WaitForSeconds(1f);
            //_meshRenderer.enabled = false;
            _hitBox.enabled = false;
            // foreach (Transform item in transform)
            // {
            //     Destroy(item.gameObject);
            // }
            // Destroy(gameObject);
        }
    }
}
