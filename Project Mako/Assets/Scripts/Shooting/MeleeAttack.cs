using System.Collections;
using System.Collections.Generic;
using Mako.VehicleDevices;
using UnityEngine;

namespace Mako.Shooting
{
    public class MeleeAttack : MonoBehaviour
    {
        [SerializeField] private int _attackPower = 8;
        [SerializeField] private Transform targetTransform;
        public void DealDamage()
        {
            Debug.Log("dealing damage");
            HealthNamespace.Health health = targetTransform.gameObject.GetComponent<HealthNamespace.Health>();
            Shields shields = targetTransform.gameObject.GetComponent<Shields>();
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
