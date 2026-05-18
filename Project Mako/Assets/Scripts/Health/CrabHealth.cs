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
        }
        protected override void OnEnable()
        {
            base.SubscribeEvents();
            healthSystem.OnRespondToFire += RespondToFire;
        }

        protected override void OnDisable()
        {
            base.UnsubscribeEvents();
            healthSystem.OnRespondToFire -= RespondToFire;
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
            audioSource.Play();
            //animator.enabled = false;
            yield return new WaitForSeconds(1f);
            meshRenderer.enabled = false;
            hitBox.enabled = false;
            // foreach (Transform item in transform)
            // {
            //     Destroy(item.gameObject);
            // }
            // Destroy(gameObject);
        }
    }
}
