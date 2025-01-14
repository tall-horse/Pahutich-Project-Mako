using UnityEngine;

namespace Mako.Health
{
  public class OneShotObject : Health
  {
    private void Awake()
    {
      base.SetupHealthObject();
    }
    private void OnTriggerEnter(Collider other)
    {
      if (other.GetComponent<Mako.Shooting.Projectile>())
      {
        base.SelfDestroy();
      }
    }
  }
}
