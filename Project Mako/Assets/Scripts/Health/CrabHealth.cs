using System.Collections;
using Mako.AI;
using UnityEngine;

namespace Mako.HealthNamespace
{
    public class CrabHealth : Health, ISelfDesctructable
    {
        private CrabMonsterAI _crabMonsterAI;
        protected override void Awake()
        {
            base.Awake();
            _crabMonsterAI = GetComponent<CrabMonsterAI>();
            _destructionAudioSource = GetComponentInChildren<AudioSource>();
            _hitBox = GetComponent<Collider>();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            OnGotDamaged += RespondToFire;
            OnDead += StartDestructionProcess;
        }

        private void OnDisable()
        {
            OnGotDamaged -= RespondToFire;
            OnDead -= StartDestructionProcess;
        }
        private void RespondToFire()
        {
            _crabMonsterAI.SetPlayerTarget();
        }
        private void StartDestructionProcess()
        {
            if (_currentHealth <= 0)
            {
                _crabMonsterAI.SetDeadState();
                StartCoroutine(SelfDestroy());
            }
        }

        public IEnumerator SelfDestroy()
        {
            _crabMonsterAI.GetEnemyManager().Deregister(_crabMonsterAI);
            _destructionAudioSource.Play();
            yield return new WaitForSeconds(1f);
            _hitBox.enabled = false;
        }
    }
}
