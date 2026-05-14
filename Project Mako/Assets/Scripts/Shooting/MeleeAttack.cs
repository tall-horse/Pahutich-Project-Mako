using System.Collections;
using System.Collections.Generic;
using Mako.VehicleDevices;
using UnityEngine;

namespace Mako.Shooting
{
  public class MeleeAttack : MonoBehaviour
  {
    [SerializeField] private Transform targetTransform;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void DealDamage()
    {
      Health.Health health = targetTransform.gameObject.GetComponent<Health.Health>();
      Shields shields = targetTransform.gameObject.GetComponent<Shields>();
      if (shields != null)
      {
        if (shields.GetShieldCapacity() >= 0)
          shields.OnHitReceived(8);
      }
      if (health != null && shields == null || shields != null && shields.GetShieldCapacity() <= 0)
      {
        health.GetHealthSystem().Damage(8);
        health.PlayImpactSound();
      }
    }
  }
}
