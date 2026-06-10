using System.Collections;
using System.Collections.Generic;
using Mako.Health;
using UnityEngine;

namespace Mako
{
    public class TowerHealth : BasicHealth
    {
        [SerializeField] private float _timeToRestoreSignal = 1f;
        protected override void Awake()
        {
            base.Awake();
            //meshRenderer = GetComponentInChildren<MeshRenderer>();
        }
        protected override IEnumerator SelfDestroy()
        {
            _audioSource.Play();
            _hitBox.enabled = false;
            _meshRenderer.enabled = false;
            yield return new WaitForSeconds(_timeToRestoreSignal);
            Destroy(gameObject);
        }
    }
}
