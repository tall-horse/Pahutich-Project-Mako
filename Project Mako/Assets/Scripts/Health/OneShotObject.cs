using System.Collections;
using UnityEngine;

namespace Mako.Health
{
  public class OneShotObject : BasicHealth
  {
        protected override IEnumerator SelfDestroy()
        {
            throw new System.NotImplementedException();
        }

        private void Awake()
    {
      base.SetupHealthObject();
    }
    private void OnTriggerEnter(Collider other)
    {
      if (other.GetComponent<Mako.Shooting.Projectile>())
      {
        StartCoroutine(SelfDestroy());
      }
    }
  }
}
