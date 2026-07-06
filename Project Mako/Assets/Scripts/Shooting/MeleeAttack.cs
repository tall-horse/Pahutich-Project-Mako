using System;
using Mako.VehicleDevices;
using UnityEngine;

namespace Mako.Shooting
{
    public class MeleeAttack : MonoBehaviour
    {
        [SerializeField] private int _attackPower = 8;
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private float _attackDistanceSquare = 25f;
        public event Action OnReloadAttack;
        public void Initialize(Transform targetTransform)
        {
            _targetTransform = targetTransform;
        }
        /// <summary>
        /// Dealing Damage. Called as animator event
        /// </summary>
        public void DealDamage()
        {
            HealthNamespace.Health health = _targetTransform.gameObject.GetComponent<HealthNamespace.Health>();
            Shields shields = _targetTransform.gameObject.GetComponent<Shields>();
            //using squared distance instead of Vector3.Distance to avoid costly Mathf.Sqrt and save performance
            float distanceSquare = (_targetTransform.position - transform.position).sqrMagnitude;
            if (distanceSquare > _attackDistanceSquare)
                return;
            if (shields != null)
            {
                if (shields.GetShieldCapacity() >= 0)
                    shields.OnHitReceived(_attackPower);
            }
            if (health != null && shields == null || shields != null && shields.GetShieldCapacity() <= 0)
            {
                health.Damage(_attackPower);
                health.PlayImpactSound();
            }
        }
        public void ReloadAttack()
        {
            OnReloadAttack?.Invoke();
        }
    }
}
