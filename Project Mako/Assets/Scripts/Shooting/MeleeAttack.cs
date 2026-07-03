using Mako.VehicleDevices;
using UnityEngine;

namespace Mako.Shooting
{
    public class MeleeAttack : MonoBehaviour
    {
        [SerializeField] private int _attackPower = 8;
        [SerializeField] private Transform _targetTransform;
        public void Initialize(Transform targetTransform)
        {
            _targetTransform = targetTransform;
        }
        public void DealDamage()
        {
            HealthNamespace.Health health = _targetTransform.gameObject.GetComponent<HealthNamespace.Health>();
            Shields shields = _targetTransform.gameObject.GetComponent<Shields>();
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
    }
}
